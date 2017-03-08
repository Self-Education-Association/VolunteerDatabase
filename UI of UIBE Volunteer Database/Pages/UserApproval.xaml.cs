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

        List<AppUser> approvallist;
        private AppUserIdentityClaims Claims { get; set; }
        public UserApproval()
        {
            InitializeComponent();
            Claims = identitypage.Claims;
            approvallist = helper.ShowNotApprovedMembers(Claims.User.Organization);
            //approval_list.Items.Clear();
            approval_list.ItemsSource = approvallist;
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
                if (result == IdentityResult.Success())
                {
                    MessageBox.Show("通过用户审批成功!");
                    approvallist.Remove(user);
                }

                else
                    MessageBox.Show(string.Join(",", result.Errors));
            }
            else
            {
                result = IdentityResult.Error("待审批的用户不存在.");
                MessageBox.Show("错误:待审批的用户不存在.");
            }
            

        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            IdentityResult result;
            AppUser user;
            if (senderButton.DataContext is AppUser)
            {
                user = (AppUser)senderButton.DataContext;
                result = helper.RejectRegisterRequest(user);
                if (result == IdentityResult.Success())
                {
                    MessageBox.Show("拒绝用户审批成功!");
                    approvallist.Remove(user);
                }
                else
                    MessageBox.Show(string.Join(",", result.Errors));
            }
            else
            {
                result = IdentityResult.Error("待审批的用户不存在.");
                MessageBox.Show("错误:待审批的用户不存在.");
            }


        }

        private void addorganization_btn_Click(object sender, RoutedEventArgs e)
        {
            var Register = new Register();
            Register.ShowDialog();
        }
    }
}
