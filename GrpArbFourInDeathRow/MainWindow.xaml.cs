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
        public MainWindow()
        {
            InitializeComponent();
                            
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    System.Windows.Media.EllipseGeometry svgItem = new EllipseGeometry();
                    svgItem.RadiusX = 26;
                    svgItem.RadiusY = 26;
                    svgItem.Center = new System.Windows.Point(15.0, 15.0);
                    /*< Ellipse Canvas.Top = "0" Canvas.Left = "0" Margin = "2" Fill = "White" Stroke = "Black" ></ Ellipse >

                    graphicsBox.Children.Add(svgItem);*/
                }
            }
        }

        private void test(object sender, RoutedEventArgs e)
        {

            var game = new Game();
            var thread = new Thread(game.StartGame);
            thread.Start();


        }
    }
}
