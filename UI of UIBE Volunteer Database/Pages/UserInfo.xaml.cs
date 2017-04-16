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
            Window window = new Window();
            window.Width = 300;
            window.Height = 250;
            window.Content = new ChangePassWord(Claims,window);
            window.Show();
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
