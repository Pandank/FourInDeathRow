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
            game = new Game();
            var gameThread = new Thread(game.StartGame);
            gameThread.Start();
                        
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    Ellipse newDot = new Ellipse();
                    newDot.Width = 26;
                    newDot.Height = 26;
                    newDot.Margin = new System.Windows.Thickness(2);
                    newDot.StrokeThickness = 1;
                    newDot.Stroke = new SolidColorBrush(Colors.DarkGray);
                    newDot.Fill = new SolidColorBrush(Colors.White);
                    Canvas.SetLeft(newDot, (x*30));
                    Canvas.SetTop(newDot, (y*30));

                    graphicsBox.Children.Add(newDot);
                }
            }
        }


        private void GameBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            MessageBox.Show(button.Name);

            game.CalculateMove(button.Name);

        }
    }
}
