using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;

namespace App_2048
{
    public partial class MainWindow : Window
    {
        private bool _inputKey = true;
        private Direction _eventKeyDirection = Direction.None;
        private Board.GameState _currentGameState = Board.GameState.DEFAULT;
        private Board _currentBoard = new ();
        private StackPanel _boardStackPanel = null!;
        
        // timer
        private int _counterTime = 0;
        private TextBlock _timerTextBlock;
        private readonly DispatcherTimer _timer = new ();
        public MainWindow()
        {
            InitializeComponent();
            
            // timer of the game
            _timer.Interval = new TimeSpan(0,0,1);
            _timer.IsEnabled = true;
            _timer.Tick += (s, e) =>
            {
                TimeSpan time = TimeSpan.FromSeconds(this._counterTime++);
                _timerTextBlock.Text = string.Format("Time: {0}",time .ToString(@"mm\:ss"));
            };
            _timer.Start();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            
            // text timer block
            this._timerTextBlock = this.FindControl<TextBlock>("Timer");
            _timerTextBlock.FontStyle = FontStyle.Italic;
            _timerTextBlock.TextAlignment = TextAlignment.Center;
            
            // text result
            TextBlock resultGame = this.Find<TextBlock>("GameState");
            resultGame.FontWeight = FontWeight.Bold;
            resultGame.TextAlignment = TextAlignment.Center;
            resultGame.Margin = new Thickness(0, 20, 0,0);
            
            // text score
            TextBlock scoreText = this.FindControl<TextBlock>("Score");
            scoreText.FontStyle = FontStyle.Italic;
            scoreText.TextAlignment = TextAlignment.Center;
            
            // initialize board
            Board currentBoard = new Board() ;
            this._currentBoard = currentBoard;
            currentBoard.Print();
            
            StackPanel stackPanelCustom = new StackPanel() { Background = Brushes.Black };
            
            Grid GameGrid = this.FindControl<Grid>("GameGrid");
            this._boardStackPanel = stackPanelCustom;
            GameGrid.Children.Add(stackPanelCustom);
            
            Board.GameState currentGameState = Board.GameState.DEFAULT;
            this._currentGameState = currentGameState;
            RenderTable(this._currentBoard, stackPanelCustom, new BrushConverter());
        }
        
        private void UpdateResult()
        {
            // result game
            TextBlock resultGame = this.Find<TextBlock>("GameState");

            switch (_currentGameState)
            {
                case Board.GameState.LOOSE:
                    Console.WriteLine("--- YOU LOSE ---");
                    resultGame.Foreground = Brushes.Red;
                    resultGame.Text = "You lost..";
                    this._inputKey = false;
                    _timer.Stop();
                    _timer.IsEnabled = false;
                    break;
                case Board.GameState.WIN:
                    Console.WriteLine("--- YOU HAVE WON ! ---");
                    resultGame.Foreground = Brushes.Green;
                    resultGame.Text = "You have won !";
                    this._inputKey = false;
                    _timer.Stop();
                    _timer.IsEnabled = false;
                    break;
                default:
                    resultGame.Foreground = Brushes.Black;
                    resultGame.Text = "";
                    break;
            }
            
            // score game
            int total = 0;
            for (int i = 0; i < _currentBoard.width; i++)
            {
                for (int j = 0; j < _currentBoard.width; j++)
                    total += _currentBoard.BoardArray[i, j].value;
            }

            TextBlock scoreText = this.FindControl<TextBlock>("Score");
            scoreText.Text = string.Format("Score: {0}", total);
        }

        private void RenderTable(Board board, StackPanel stackPanelCustom, BrushConverter converter)
        {
            for (int j = 0; j < board.width; j++)
            {
                var dockPanel = new DockPanel(); // horizontal
                stackPanelCustom.Children.Add(dockPanel);
                for (int i = 0; i < board.width; i++)
                {
                    ISolidColorBrush color;
                    
                    switch (board.BoardArray[j,i].value)
                    {
                        case 2 :
                            color = (ISolidColorBrush)converter.ConvertFromString("#eee4da")!;
                            break;
                        case 4:
                            color = (ISolidColorBrush)converter.ConvertFromString("#ede0c8")!;
                            break;
                        case 8: 
                            color = (ISolidColorBrush)converter.ConvertFromString("#f2b179")!;
                            break;
                        case 16:
                            color = (ISolidColorBrush) converter.ConvertFromString("#f59563")!;
                            break;
                        case 32:
                            color = (ISolidColorBrush) converter.ConvertFromString("#f67c5f")!;
                            break;
                        case 64:
                            color = (ISolidColorBrush) converter.ConvertFromString("#f65e3b")!;
                            break;
                        case 128:
                            color = (ISolidColorBrush) converter.ConvertFromString("#edcf72")!;
                            break;
                        case 256:
                            color = (ISolidColorBrush) converter.ConvertFromString("#edcc61")!;
                            break;
                        case 512:
                            color = (ISolidColorBrush) converter.ConvertFromString("#edcc61")!; // color to change
                            break;
                        case 1024:
                            color = (ISolidColorBrush) converter.ConvertFromString("#edcc61")!; // color to change
                            break;
                        case 2048:
                            color = (ISolidColorBrush) converter.ConvertFromString("#edcc61")!; // color to change
                            break;
                        default:
                            color = Brushes.White;
                            break;
                    }

                    int size = 60;
                    var gridCustom = new Grid()
                    {
                        Background = color,
                        Width = size,
                        Height = size,
                        Margin = new Thickness(1)
                    };
                    var textBlock = new TextBlock()
                    {
                        Text = board.BoardArray[j,i].value == 0 ? "" : board.BoardArray[j,i].value.ToString() ,
                        FontWeight = FontWeight.Bold,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    gridCustom.Children.Add(textBlock);
                    dockPanel.Children.Add(gridCustom);
                }
            }
            
            UpdateResult();
        } 
        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!this._inputKey)
                return;
            
            Console.WriteLine(e.Key);
            base.OnKeyDown(e);
            switch (e.Key) {
                case Key.Right :
                    this._eventKeyDirection = Direction.Right;
                    break;
                case Key.Left : 
                    this._eventKeyDirection = Direction.Left;
                    break;
                case Key.Up:
                    this._eventKeyDirection = Direction.Up;
                    break;
                case Key.Down : 
                    this._eventKeyDirection = Direction.Down;
                    break;
                default :
                    Console.WriteLine("Use keyboard's arrow");
                    return; 
            }
            
            UpdateBoard();
        }

        private void UpdateBoard()
        {
            var converter = new BrushConverter();
            this._boardStackPanel.Children.Clear();
            this._currentGameState = this._currentBoard.Move(this._eventKeyDirection);
            _currentBoard.Print();
            this._currentGameState = _currentBoard.GetGameState();
            RenderTable(_currentBoard, this._boardStackPanel, converter);
        }
    }
}