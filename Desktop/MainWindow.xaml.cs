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

namespace VolunteerDatabase.Desktop
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private IdentityClaims claims;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void createDatabase_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Database())
            {
                if (db.Database.Exists())
                {
                    db.Database.Delete();
                }
                db.Database.Create();
            }
            MessageBox.Show("success");
        }

        private async void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            var helper = IdentityHelper.GetInstanceAsync();
            (await helper).GetRole(AppRoleEnum.Administrator);
        }
    }
}
