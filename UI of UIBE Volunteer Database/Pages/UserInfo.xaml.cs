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
using VolunteerDatabase.Entity;

namespace Desktop.Pages
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
