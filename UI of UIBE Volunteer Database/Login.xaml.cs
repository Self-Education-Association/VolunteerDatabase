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
using FirstFloor.ModernUI.Windows.Controls;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private static AppUserIdentityClaims claims;

        public Login()
        {
            InitializeComponent();
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            var Register = new Register();
            Register.Show();
        }

        private async void login_btn_Click(object sender, RoutedEventArgs e)
        {
            if (userid.Text == "" ||password.Password == "")
            {
                Tips_block.Visibility = Visibility.Visible;
            }
            else
            { 
                if (claims != null && claims.IsAuthenticated == true)
                {
                    SendClaimsEvent(claims);
                    return;
                }
                IdentityHelper ih = IdentityHelper.GetInstance();
                if (userid.Text == "" || password.Password.ToString() == "")
                {

                }
                else
                {
                    var claims = await ih.CreateClaimsAsync(userid.Text, password.Password.ToString());//输入合法性验证
                    if (claims.IsAuthenticated)
                    {
                        MessageBox.Show("登陆成功！");
                        SendClaimsEvent(claims);
                        Close();

                    }
                    else
                    {
                        MessageBox.Show("登录失败，请检查用户名和密码！");
                    }
                }
            }

        }

        public static void GetClaims(SendClaimsDelegate sendClaims)
        {
            if (claims?.IsAuthenticated == true)
            {
                SendClaimsEvent(claims);
                return;
            }

            Login loginWindow = new Login();
            loginWindow.Show();
            SendClaimsEvent += sendClaims;
            return;
        }

        public delegate void SendClaimsDelegate(AppUserIdentityClaims claims);

        public static event SendClaimsDelegate SendClaimsEvent;
    }
}
