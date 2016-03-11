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

namespace CompressionComponent
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

        private void openFileDialog(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 

            dlg.DefaultExt = ".txt";

            dlg.Filter = "Word Document (.doc)|*.doc *.docx|Acrobat Reader (.pdf)|*.pdf|All files (*.*)|*.*";



            // Display OpenFileDialog by calling ShowDialog method 

            Nullable<bool> result = dlg.ShowDialog();



            // Get the selected file name and display in a TextBox 

            if (result == true)
            {

                // Open document 

                string filename = dlg.FileName;

                FileNameTextBox.Text = filename;

            }


        }

        private void compress(object sender, RoutedEventArgs e)
        {
            string fileName = FileNameTextBox.Text;

        }
    }
}
