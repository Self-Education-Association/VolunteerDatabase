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

namespace Desktop.Pages
{
    /// <summary>
    /// AddManager.xaml 的交互逻辑
    /// </summary>
    public partial class AddManager : Window
    {
        private IdentityPage identitypage = IdentityPage.GetInstance();
        AppUserIdentityClaims Claims { get; set; }

        ProjectManageHelper helper;

        IdentityHelper identityhelper;

        Project sourceproject;

        List<AppUser> availableuserlist = new List<AppUser>();
        public AddManager(Project project)
        {
            sourceproject = project;
            helper = ProjectManageHelper.GetInstance();
            identityhelper = IdentityHelper.GetInstance();
            Claims = identitypage.Claims;
            availableuserlist = identityhelper.GetUnderlingsList(Claims.User);
            InitializeComponent();
            availableUsers.ItemsSource = availableuserlist;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            foreach ( item in collection)
            {

            }
        }


    }
}
