using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// AuthenticationWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        private IdentityHelper helper;
        private AppUserIdentityClaims claims;

        public AuthenticationWindow()
        {
            InitializeComponent();
        }

        private void removeText(object sender, RoutedEventArgs e)
        {
            if (typeof(TextBox).IsAssignableFrom(sender.GetType()))
            {
                var textBox = sender as TextBox;
                textBox.Text = "";
            }
            if (typeof(PasswordBox).IsAssignableFrom(sender.GetType()))
            {
                var passwordBox = sender as PasswordBox;
                passwordBox.Password = "";
            }
        }

        private async void signInButton_Click(object sender, RoutedEventArgs e)
        {
            signInButton.IsEnabled = false;
            await initialHelper();
            claims = await helper.CreateClaimsAsync(userName: accountSignInTextBox.Text, password: passwordSignInTextBox.Password);
            if (claims != null && claims.IsAuthenticated)
            {
                showMainWindow(claims);
            }
            else
            {
                MessageBox.Show("Login Failed!");
            }
            signInButton.IsEnabled = true;
        }

        private void showMainWindow(AppUserIdentityClaims claims)
        {
            var main = new MainWindow(claims);
            Hide();
            main.Show();
        }

        private async void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(accountSignUpTextBox.Text) || string.IsNullOrWhiteSpace(passwordSignUpTextBox.Password) || comboSignUpBox.SelectedItem == null)
            {
                MessageBox.Show("请将信息填写完整！");
                return;
            }
            signUpButton.IsEnabled = false;
            await initialHelper();
            helper.CreateOrFindRole(AppRoleEnum.OrgnizationMember);
            var result = helper.CreateUser(user: new AppUser { AccountName = accountSignUpTextBox.Text, Name = nameSignUpTextBox.Text }, password: passwordSignUpTextBox.Password, roleEnum: AppRoleEnum.OrgnizationMember, orgEnum: (OrganizationEnum)comboSignUpBox.SelectedItem);
            if (result.Succeeded)
            {
                claims = helper.CreateClaims(userName: accountSignUpTextBox.Text, password: passwordSignUpTextBox.Password);
                if (claims != null)
                {
                    showMainWindow(claims);
                }
            }
            else
            {
                MessageBox.Show(string.Concat(result.Errors));
            }
            signUpButton.IsEnabled = true;
        }

        private async Task initialHelper()
        {
            if (helper == null)
            {
                helper = await IdentityHelper.GetInstanceAsync();
            }
        }
    }
}
