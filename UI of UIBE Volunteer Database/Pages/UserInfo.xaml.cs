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
using VolunteerDatabase.Interface;
using VolunteerDatabase.Helper;

namespace Desktop.Pages
{
    /// <summary>
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : UserControl
    {
        private IdentityPage identitypage = IdentityPage.GetInstance();
        private AppUserIdentityClaims Claims { get; set; }
        public UserInfo()
        {
            Claims = identitypage.Claims;
            InitializeComponent();
        }
        private void sendClaimsEventHandler(AppUserIdentityClaims claims)
        {
            this.Claims = claims;
#warning "把这些MessageBox.Show()改成友好的窗口或者Tips"
            MessageBox.Show("用户信息模块收到令牌.");
        }

        private void ChangePassword_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void ShowUserMessage()
        //{
        //    string[] orgstring = Claims.User.Organization.OrganizationEnum.ToString().Split('.');
        //    string orgdetail = orgstring.Last();
        //    string[] rolestring = Claims.Roles.ToString().Split('.');
        //    string roledetail = rolestring.Last();
        //    account_name.Text = Claims.User.AccountName;
        //    org.Text = orgdetail;
        //    tel.Text = Claims.User.Mobile;
        //    roles.Text = roledetail;
        //    userid.Text = Claims.User.StudentNum.ToString();
        //    email.Text = Claims.User.Email;
        //    dormitary.Text = Claims.User.Room;
        //}

    }

}
