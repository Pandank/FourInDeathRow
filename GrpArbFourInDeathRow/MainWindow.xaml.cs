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
    }
}
