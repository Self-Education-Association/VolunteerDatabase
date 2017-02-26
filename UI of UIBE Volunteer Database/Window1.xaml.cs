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
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Click1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            contentControl1.Content = new UserControl1();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            contentControl2.Content = new UserControl2();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            contentControl2.Content = new UserControl3();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            contentControl2.Content = new UserControl4();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            contentControl2.Content = new UserControl5();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            contentControl2.Content = new UserControl6();
        }
    }
}
