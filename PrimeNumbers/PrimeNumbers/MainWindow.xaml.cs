using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using static System.Linq.Enumerable;

namespace PrimeNumbers
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static readonly Regex Regex = new Regex("[^0-9]+", RegexOptions.Compiled | RegexOptions.Singleline);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text);
        }
    }

    public class PrimesRow : INotifyPropertyChanged
    {
        private int _progressValue;

        public int Id { get; set; }

        public TaskStatus CurrentState => Task.Status;

        public int X { get; set; }

        public int Result { get; set; }

        public Visibility ResultVisibility { get; set; }

        public Task Task { get; set; }

        public int ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = value;
                NotifyPropertyChanged();
            }
        }

        public bool CancelEnabled => !Task.IsCompleted && !Task.IsFaulted && !Task.IsCanceled;

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged()
        {
            NotifyPropertyChanged(string.Empty);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PrimesViewModel
    {
        private readonly Dictionary<int, CancellationTokenSource> _cancelations =
            new Dictionary<int, CancellationTokenSource>();

        private ICommand _adder;
        private ICommand _canceller;

        private int _globalTaskId;
        public ObservableCollection<PrimesRow> Primes { get; set; } = new ObservableCollection<PrimesRow>();

        public ICommand AddRow => _adder ?? (_adder = new SimpleCommand(async p =>
        {
            var x = (string) p;
            if (x.Length == 0)
            {
                return;
            }

            var cts = new CancellationTokenSource();

            _cancelations[_globalTaskId++] = cts;

            var row = new PrimesRow
            {
                Id = _globalTaskId - 1,
                X = int.Parse(x),
                Result = 0,
                ResultVisibility = Visibility.Hidden
            };

            Primes.Add(row);

            var task = Task.Run(() =>
            {
                IProgress<int> progress = new Progress<int>(i => { row.ProgressValue = i; });

                cts.Token.ThrowIfCancellationRequested();

                var result = 0;

                if (row.X >= 2)
                {
                    foreach (var number in Range(2, row.X - 1))
                    {
                        result += number.IsPrime() ? 1 : 0;

                        cts.Token.ThrowIfCancellationRequested();

                        progress.Report(100 * number / row.X);
                    }
                }

                row.ProgressValue = 100;
                row.ResultVisibility = Visibility.Visible;
                row.Result = result;
            }, cts.Token);

            row.Task = task;

            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
            }

            row.NotifyPropertyChanged();
        }));

        public ICommand CancelTask =>
            _canceller ?? (_canceller = new SimpleCommand(p => { _cancelations[(int) p].Cancel(); }));

        private class SimpleCommand : ICommand
        {
            #region ICommand Members

            private readonly Action<object> _execute;

            public SimpleCommand(Action<object> execute) => _execute = execute;

            public bool CanExecute(object parameter) => true;

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }

            #endregion
        }
    }

    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var enumString = Enum.GetName(value.GetType(), value);
                return enumString;
            }
            catch
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}