using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GrpArbFourInDeathRow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;

        public MainWindow()
        {
            InitializeComponent();
            DrawBoardInit();
            LockGameBoard();
        }

        public void DrawBoard()
        {
            for (int y = 5; y >= 0; y--)
            {
                for (int x = 6; x >= 0; x--)
                {
                    Ellipse newDot = new Ellipse
                    {
                        Width = 26,
                        Height = 26,
                        Margin = new System.Windows.Thickness(2),
                        StrokeThickness = 1,
                        Stroke = new SolidColorBrush(Colors.DarkGray)
                    };
                    if (game.GameBoard[x, y] == 1)
                        newDot.Fill = new SolidColorBrush(Colors.Red);
                    else if (game.GameBoard[x, y] == 2)
                        newDot.Fill = new SolidColorBrush(Colors.Yellow);
                    else
                        newDot.Fill = new SolidColorBrush(Colors.White);
                    Canvas.SetLeft(newDot, x * 30);
                    Canvas.SetTop(newDot, y * -30 + 150);
                    graphicsBox.Children.Add(newDot);
                }
            }
        }
        public void DrawBoardInit()
        {
            for (int y = 5; y >= 0; y--)
            {
                for (int x = 6; x >= 0; x--)
                {
                    Ellipse newDot = new Ellipse
                    {
                        Width = 26,
                        Height = 26,
                        Margin = new System.Windows.Thickness(2),
                        StrokeThickness = 1,
                        Stroke = new SolidColorBrush(Colors.DarkGray)
                    };

                    newDot.Fill = new SolidColorBrush(Colors.White);
                    Canvas.SetLeft(newDot, x * 30);
                    Canvas.SetTop(newDot, y * -30 + 150);
                    graphicsBox.Children.Add(newDot);
                }
            }
        }



        private void GameBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            int[] coords = game.IsValidMove(button);
            if (coords[0] != -1)
            {
                game.SendMoveToServer(coords);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void LockGameBoard()
        {
            foreach (object o in myGrid.Children)
            {
                if (o is Button button)
                    button.Click -= GameBtn_Click;
            }
        }

        public void UnlockGameBoard()
        {
            foreach (object o in myGrid.Children)
            {
                if (o is Button button)
                    button.Click += GameBtn_Click;
            }
        }

        public void UpdateInfoBox()
        {
            UserName.Content = game.UserName;
            InfoLabel.Content = game.Info;
        }


        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            
            game = new Game(this);
            var gameThread = new Thread(game.StartGame);
            gameThread.Start();
            DrawBoard();
        }
    }
}