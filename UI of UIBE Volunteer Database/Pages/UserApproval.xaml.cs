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
using VolunteerDatabase.Helper;
using VolunteerDatabase.Entity;

namespace Desktop.Pages
{
    /// <summary>
    /// Interaction logic for UserApproval.xaml
    /// </summary>
    public partial class UserApproval : UserControl
    {
        //注意：使用测试列表

        private IdentityPage identitypage = IdentityPage.GetInstance();

        private IdentityHelper helper = IdentityHelper.GetInstance();
        private AppUserIdentityClaims Claims { get; set; }
        public UserApproval()
        {
            InitializeComponent();
            Claims = identitypage.Claims;
            List<AppUser> approvallist = helper.ShowNotApprovedMembers(Claims.User.Organization);
            //approval_list.Items.Clear();
            approval_list.ItemsSource = approvallist;
            ConfirmApproval dialog = new ConfirmApproval();
        }

        private void search_volunteer_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            ConfirmApproval dialog = new ConfirmApproval();
        }
    }
}
