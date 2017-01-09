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
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Desktop
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppUserIdentityClaims claims;

        public MainWindow(AppUserIdentityClaims claims)
        {
            InitializeComponent();
            this.claims = claims;
        }

        private void removeText(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text = "";
        }

        private async void createDatabase_Click(object sender, RoutedEventArgs e)
        {
            var db = await DatabaseContext.GetInstanceAsync();
            MessageBox.Show("success");
        }
    }
}
