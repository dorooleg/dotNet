using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Linq.Enumerable;

namespace PrimeNumbers
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Dictionary<int, CancellationTokenSource> _cancelations =
            new Dictionary<int, CancellationTokenSource>();

        private int _globalTaskId;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!(sender is ListView listView))
                return;

            listView.UpdateLayout();
            UpdateColumnsWidth(listView);
        }

        private static void UpdateColumnsWidth(ListView listView)
        {
            if (!(listView.View is GridView grid))
                return;

            var autoFillColumnIndex = grid.Columns.Count - 1;
            if (double.IsNaN(listView.ActualWidth))
                listView.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var remainingSpace = grid.Columns.Where((t, i) => i != autoFillColumnIndex)
                .Aggregate(listView.ActualWidth, (current, t) => current - t.ActualWidth);
            grid.Columns[autoFillColumnIndex].Width = Math.Max(remainingSpace - 30, 0);
        }

        private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
        {
            if (XInput.Text.Length == 0)
                return;

            var cts = new CancellationTokenSource();

            _cancelations[_globalTaskId++] = cts;

            var row = new PrimeNumbersRow
            {
                Id = _globalTaskId - 1,
                X = int.Parse(XInput.Text),
                Result = 0,
                ResultVisibility = Visibility.Hidden,
                CancelEnabled = true,
                ProgressValue = 0
            };
            XInput.Clear();

            LvPrimesNumbers.Items.Add(row);


            Task.Factory.StartNew(() =>
            {
                if (cts.IsCancellationRequested)
                {
                    row.CurrentState = PrimeNumbersRow.State.Canceled;
                    row.CancelEnabled = false;
                    return;
                }

                row.CurrentState = PrimeNumbersRow.State.Running;

                var result = 0;

                foreach (var number in Range(2, row.X - 1))
                {
                    result += number.IsPrime() ? 1 : 0;

                    if (cts.IsCancellationRequested)
                    {
                        row.CurrentState = PrimeNumbersRow.State.Canceled;
                        row.CancelEnabled = false;
                        return;
                    }

                    row.ProgressValue = 100 * number / row.X;
                }

                row.ProgressValue = 100;
                row.CurrentState = PrimeNumbersRow.State.Done;
                row.ResultVisibility = Visibility.Visible;
                row.Result = result;
                row.CancelEnabled = false;
            }, cts.Token);
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            var id = int.Parse(((Button) sender).Tag.ToString());
            _cancelations[id].Cancel();
        }

        public class PrimeNumbersRow : INotifyPropertyChanged
        {
            public enum State
            {
                Wait,
                Running,
                Done,
                Canceled
            }

            private State _currentState;

            private int _progressValue;

            public int Id { get; set; }

            public State CurrentState
            {
                get => _currentState;
                set
                {
                    _currentState = value;
                    NotifyPropertyChanged();
                }
            }

            public int X { get; set; }

            public int Result { get; set; }

            public Visibility ResultVisibility { get; set; }

            public string CurrentStateString => CurrentState.ToString();

            public int ProgressValue
            {
                get => _progressValue;
                set
                {
                    _progressValue = value;
                    NotifyPropertyChanged();
                }
            }

            public bool CancelEnabled { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged(string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}