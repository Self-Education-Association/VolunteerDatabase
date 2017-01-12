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

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void createDatabase_Click(object sender, RoutedEventArgs e)
        {
            var db = await DatabaseContext.GetInstanceAsync();
            MessageBox.Show("success");
        }

        private void authorizeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = AuthorizeHelper<string, string>.Execute(AppUserAuthorizeInput<string>.Create(claims, "return"), say);
                MessageBox.Show(result);
            }
            catch (AppUserNotAuthorizedException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        //[AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        private string say(string input)
        {
            return input;
        }
    }
}
