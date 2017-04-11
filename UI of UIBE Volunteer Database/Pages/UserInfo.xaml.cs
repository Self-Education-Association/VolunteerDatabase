using System.Windows;
using System.Windows.Controls;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;

namespace VolunteerDatabase.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : UserControl
    {
        private IdentityPage identitypage;
        private AppUserIdentityClaims Claims { get; set; }
        public UserInfo()
        {
            identitypage = IdentityPage.GetInstance();
            if (identitypage!=null&&identitypage?.Claims != null)
            {
                Claims = identitypage.Claims;
                if(identitypage.flag)
                {
                    InitializeComponent();
                    ShowUserMessage();
                }
                else
                {
                    IsEnabled = false;
                }
            }
        }
            

        private void sendClaimsEventHandler(AppUserIdentityClaims claims)
        {
            this.Claims = claims;
        }

        private void ChangePassword_btn_Click(object sender, RoutedEventArgs e)
        {
            string a = Password.Password;
            string b = PasswordChange.Password;
            string c = PasswordChangeConfirm.Password;
            if (b != c)
            {
                MessageBox.Show("两次输入密码不一致，请检查");
            }
            else
            {
                var ih = IdentityHelper.GetInstance();
                var re = ih.ChangePassword(Claims.User.AccountName, a, b);
                if (re.Succeeded)
                {
                    MessageBox.Show("修改成功");
                }
                else
                {
                    MessageBox.Show(re.Errors.ToString());
                }
            }
        }

        private void ShowUserMessage()
        {
            string userroles = "";
            foreach (AppRole role in Claims.User.Roles)
            {
                userroles = userroles + role.Name +" ";
            }
            account_name.Text = Claims.User.AccountName;
            org.Text = Claims.User.Organization.Name;
            tel.Text = Claims.User.Mobile;
            roles.Text = userroles;
            userid.Text = Claims.User.StudentNum.ToString();
            email.Text = Claims.User.Email;
            dormitary.Text = Claims.User.Room;
        }

    }

}
