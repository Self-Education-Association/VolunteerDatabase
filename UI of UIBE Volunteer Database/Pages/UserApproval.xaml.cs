using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Desktop.Pages
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
            if(Claims.IsInRole(AppRoleEnum.Administrator))
            {
                addorganization_btn.Visibility = Visibility.Visible;
                approvallist = helper.ShowNotApprovedMembers(Claims.User.Organization);
            }
            if(Claims.IsInRole(AppRoleEnum.Administrator)|| Claims.IsInRole(AppRoleEnum.OrgnizationAdministrator))
            {
                this.IsEnabled = true;
                approvallist = helper.ShowNotApprovedMembers(Claims.User.Organization);
            }
            else
            {
                this.IsEnabled = false;
            }
            
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
                if (result.AreSameWith(IdentityResult.Success()))
                {
                    MessageBox.Show("通过用户审批成功!");
                    approvallist.Remove(user);
                    approval_list.ItemsSource = null;
                    approval_list.ItemsSource = approvallist;
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
                if (result.AreSameWith(IdentityResult.Success()))
                {
                    MessageBox.Show("拒绝用户审批成功!");
                    approvallist.Remove(user);
                    approval_list.ItemsSource = null;
                    approval_list.ItemsSource = approvallist;
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
            var Register = new Register(Claims);
            Register.ShowDialog();
        }
    }
}
