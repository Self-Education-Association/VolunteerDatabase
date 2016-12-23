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
        private IdentityClaims claims;

        public MainWindow()
        {
            InitializeComponent();
            nameTextBox.Text = "用户名";

            nameTextBox.GotFocus += removeText;
        }

        private void removeText(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text = "";
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
            var helper = await IdentityHelper.GetInstanceAsync();
            helper.GetRole(AppRoleEnum.LogViewer);
            var result = helper.CreateUser(user: new AppUser { Name = nameTextBox.Text }, password: passwordTextBox.Password, roleEnum: AppRoleEnum.LogViewer);
            if (result.Succeeded)
            {
                claims = helper.CreateClaims(userName: nameTextBox.Text, password: passwordTextBox.Password);
            }
            if (claims != null)
            {
                MessageBox.Show("success");
            }
            else
            {
                MessageBox.Show(result.Errors.ToString());
            }

        }
    }
}
