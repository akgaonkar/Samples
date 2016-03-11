using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace WPF_Sandbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //AdornerLayer.GetAdornerLayer(Btn).Add(new FourBoxes(Btn));
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
           // AdornerLayer.GetAdornerLayer(Btn).Add(new FourBoxes(Btn));
            Button inAdorner = new Button()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = "X",
            };
            ControlAdorner adorner = new ControlAdorner(Btn)
            {
                Child = inAdorner,
            };
            AdornerLayer.GetAdornerLayer(Btn).Add(adorner);

            Viewbox vb = new Viewbox();
            

            
        }
    }
}
