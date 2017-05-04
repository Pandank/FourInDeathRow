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
        }

        public void DrawBoard()
        {
            for (int y = 5; y >= 0; y--)
            {
                for (int x = 6; x >= 0; x--)
                {
                    Ellipse newDot = new Ellipse();
                    newDot.Width = 26;
                    newDot.Height = 26;
                    newDot.Margin = new System.Windows.Thickness(2);
                    newDot.StrokeThickness = 1;
                    newDot.Stroke = new SolidColorBrush(Colors.DarkGray);
                    if (game.GameBoard[x, y] == 1)
                        newDot.Fill = new SolidColorBrush(Colors.Red);
                    else if (game.GameBoard[x, y] == 2)
                        newDot.Fill = new SolidColorBrush(Colors.Yellow);
                    else
                        newDot.Fill = new SolidColorBrush(Colors.White);
                    Canvas.SetLeft(newDot, (x * 30));
                    Canvas.SetTop(newDot, (y * -30 + 150));

                    //game.

                    graphicsBox.Children.Add(newDot);
                }
            }
        }

        private void GameBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            //MessageBox.Show(button.Name);

            int[] coords = game.CalculateMove(button.Name);

            if (coords[1] <=5 && coords[1]>=0)
            {
                game.SendMoveToServer(coords[0], coords[1]);
                //we dont need this but keep it for now
                //if (coords[1]==5)
                //{
                //    //button.Click -= GameBtn_Click;
                //}
            }
            
            
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
