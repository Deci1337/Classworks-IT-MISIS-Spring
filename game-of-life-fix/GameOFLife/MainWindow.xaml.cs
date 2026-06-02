using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using GameOFLife.Core;

namespace GameOFLife
{
    public partial class MainWindow : Window
    {
        private Game _game;
        private DispatcherTimer _timer;
        private bool _running;

        public MainWindow()
        {
            InitializeComponent();
            _game = new Game(rows: 30, columns: 50);

            _timer = new DispatcherTimer();
            _timer.Interval = System.TimeSpan.FromMilliseconds(300);
            _timer.Tick += (s, e) =>
            {
                _game.NextGeneration();
                RenderField();
                GenLabel.Text = $"Generation: {_game.Generation}";
            };

            PlaceInitialCells();
            RenderField();
        }

        private void PlaceInitialCells()
        {
            // Пульсар в центре (period-3, симметричный осциллятор 13x13)
            int pr = 9, pc = 19;
            int[] bars = { 2, 3, 4, 8, 9, 10 };
            foreach (int dc in bars)
            {
                _game.SetCell(pr, pc + dc, true);
                _game.SetCell(pr + 5, pc + dc, true);
                _game.SetCell(pr + 7, pc + dc, true);
                _game.SetCell(pr + 12, pc + dc, true);
            }
            foreach (int dr in new[] { 2, 3, 4, 8, 9, 10 })
            {
                _game.SetCell(pr + dr, pc, true);
                _game.SetCell(pr + dr, pc + 5, true);
                _game.SetCell(pr + dr, pc + 7, true);
                _game.SetCell(pr + dr, pc + 12, true);
            }

            // 4 глайдера из углов → летят к центру, встречаются ~на 50й генерации

            // Top-left → SE
            _game.SetCell(2, 3, true); _game.SetCell(3, 4, true);
            _game.SetCell(4, 2, true); _game.SetCell(4, 3, true); _game.SetCell(4, 4, true);

            // Top-right → SW
            _game.SetCell(2, 45, true); _game.SetCell(3, 44, true);
            _game.SetCell(4, 43, true); _game.SetCell(4, 44, true); _game.SetCell(4, 45, true);

            // Bottom-left → NE
            _game.SetCell(25, 2, true); _game.SetCell(25, 3, true); _game.SetCell(25, 4, true);
            _game.SetCell(26, 2, true); _game.SetCell(27, 3, true);

            // Bottom-right → NW
            _game.SetCell(25, 44, true); _game.SetCell(25, 45, true); _game.SetCell(25, 46, true);
            _game.SetCell(26, 46, true); _game.SetCell(27, 45, true);
        }

        private void RenderField()
        {
            GameGrid.Rows = _game.Rows;
            GameGrid.Columns = _game.Columns;
            GameGrid.Children.Clear();

            for (int r = 0; r < _game.Rows; r++)
            {
                for (int c = 0; c < _game.Columns; c++)
                {
                    int row = r, col = c;
                    bool alive = _game.Field[r, c];

                    var cell = new Border
                    {
                        Background = alive
                            ? Brushes.White
                            : new SolidColorBrush(Color.FromRgb(40, 40, 40)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(110, 110, 110)),
                        BorderThickness = new Thickness(1),
                        Cursor = Cursors.Hand
                    };

                    cell.MouseLeftButtonDown += (s, e) =>
                    {
                        if (_running) return;
                        bool current = _game.Field[row, col];
                        _game.SetCell(row, col, !current);
                        ((Border)s).Background = !current
                            ? Brushes.White
                            : new SolidColorBrush(Color.FromRgb(40, 40, 40));
                    };

                    GameGrid.Children.Add(cell);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            switch (btn.Content.ToString())
            {
                case "Start":
                    _running = true;
                    _timer.Start();
                    break;
                case "Finish":
                    _running = false;
                    _timer.Stop();
                    _game = new Game(_game.Rows, _game.Columns, _game.Speed);
                    PlaceInitialCells();
                    RenderField();
                    GenLabel.Text = "Generation: 0";
                    break;
                case "Clear":
                    _running = false;
                    _timer.Stop();
                    _game = new Game(_game.Rows, _game.Columns, _game.Speed);
                    RenderField();
                    GenLabel.Text = "Generation: 0";
                    break;
                case "Speed":
                    _game.Speed = _game.Speed % 3 + 1;
                    int ms = _game.Speed == 1 ? 300 : _game.Speed == 2 ? 200 : 100;
                    _timer.Interval = System.TimeSpan.FromMilliseconds(ms);
                    SpeedLabel.Text = $"Speed: x{_game.Speed}";
                    break;
            }
        }
    }
}
