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

namespace Notepad
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void newClick(object sender, RoutedEventArgs e)
        {
            var newTab = new TabItem();
            newTab.Header = "New File";

            Grid grid = new Grid();
            grid.Children.Add(new TextBox());

            newTab.Content = grid;

            /*tabControl.Items.Add(newTab);
            tabControl.SelectedItem = newTab;*/
        }
    }
}
