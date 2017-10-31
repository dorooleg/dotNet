using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TickTacToe
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Button[,] _buttons = new Button[GameBoard.Size, GameBoard.Size];
        private readonly Statistics _statistics = new Statistics();
        private GameBoard _board = new GameBoard();
        private IPlayer _first;
        private IPlayer _second;
        private Task<MessageBoxResult> _task;

        public MainWindow()
        {
            InitializeComponent();
            for (var x = 0; x < GameBoard.Size; x++)
            for (var y = 0; y < GameBoard.Size; y++)
            {
                _buttons[x, y] = new Button();
                var x1 = x;
                var y1 = y;
                _buttons[x, y].Click += (s, e) =>
                {
                    _first?.Update(x1, y1);
                    _buttons[x1, y1].Content = (char) _board.GetElement(x1, y1);
                    _buttons[x1, y1].Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
                    if (_task != null)
                        Task.WaitAll(_task);
                };

                ButtonsGrid.Children.Add(_buttons[x, y]);
            }
            UpdateStatistics();
        }

        private static readonly Action EmptyDelegate = delegate { };

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private bool CheckDraw()
        {
            if (!_board.Draw) return false;
            _task = Task.Run(() => MessageBox.Show("Draw"));
            return true;
        }

        private bool CheckWinX()
        {
            if (!_board.WinX) return false;
            _task = Task.Run(() => MessageBox.Show("Win X"));
            return true;
        }

        private bool CheckWinO()
        {
            if (!_board.WinO) return false;
            _task = Task.Run(() => MessageBox.Show("Win O"));
            return true;
        }

        private void User_OnClick(object sender, RoutedEventArgs e)
        {
            Clear();
            _board = new GameBoard();
            _first = new Player(_board, r =>
            {
                if (!r) return;
                if (!_board.EndGame) return;
                CheckDraw();
                CheckWinO();
                CheckWinX();
            });
            Statistics.Visibility = Visibility.Collapsed;
        }

        private void UpdateStatistics()
        {
            CountDraw.Content = _statistics.CountDraw;
            CountLose.Content = _statistics.CountLose;
            CountWin.Content = _statistics.CountWin;
        }

        private void Clear()
        {
            _board = new GameBoard();
            _buttons.Cast<Button>().ToList().ForEach(x => x.Content = string.Empty);
        }

        private void CreateRobotGame(IPlayer robot)
        {
            _second = robot;
            _first = new Player(_board, r =>
            {
                if (!r) return;

                _second.Update(0, 0);

                if (!_board.EndGame) return;

                if (CheckDraw())
                    _statistics.CountDraw += 1;

                if (CheckWinO())
                    _statistics.CountLose += 1;

                if (CheckWinX())
                    _statistics.CountWin += 1;

                UpdateStatistics();
            });
            Statistics.Visibility = Visibility.Visible;
        }

        private void Easy_OnClick(object sender, RoutedEventArgs e)
        {
            Clear();
            CreateRobotGame(new EasyRobot(_board, (x, y) =>
            {
                _buttons[x, y].Content = (char) _board.GetElement(x, y);
                _buttons[x, y].Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
            }));
        }

        private void Medium_OnClick(object sender, RoutedEventArgs e)
        {
            Clear();
            CreateRobotGame(new MediumRobot(_board, (x, y) =>
            {
                _buttons[x, y].Content = (char) _board.GetElement(x, y);
                _buttons[x, y].Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
            }));
        }
    }
}