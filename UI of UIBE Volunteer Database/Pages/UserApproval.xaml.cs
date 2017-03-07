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
        }

        private void search_volunteer_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            IdentityResult result;
            AppUser user;
            if (senderButton.DataContext is AppUser)
            {
                user = (AppUser)senderButton.DataContext;
                result = helper.ApproveRegisterRequest(user);
                MessageBox.Show("通过用户审批成功!");
            }
            else
            {
                result = IdentityResult.Error("待审批的用户为空.");
                MessageBox.Show("错误:待审批的用户为空.");
            }

        }
    }
}
