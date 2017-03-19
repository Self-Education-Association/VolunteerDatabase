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
using Desktop.Pages;
using VolunteerDatabase.Interface;
using System.ComponentModel;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private static AppUserIdentityClaims claimsStored;

        private static Login loginWindow = new Login();

        protected Login()
        {
            InitializeComponent();
        }

        protected static Login GetWindow()
        {
            if (loginWindow == null)
            {
                lock (loginWindow)
                {
                    if (loginWindow == null)
                        loginWindow = new Login();
                }
            }
            return loginWindow;
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            var Register = new Register();
            Register.Show();
        }

        private async void login_btn_Click(object sender, RoutedEventArgs e)
        {
            if (userid.Text == "" || password.Password == "")
            {
                Tips_block.Visibility = Visibility.Visible;
            }
            else
            {
                if (claimsStored != null && claimsStored.IsAuthenticated == true)
                {
                    SendClaimsEvent(claimsStored);
                    return;
                }
                IdentityHelper ih = IdentityHelper.GetInstance();
                if (userid.Text == "" || password.Password.ToString() == "")
                {

                }
                else
                {
                    var claims = await ih.CreateClaimsAsync(userid.Text, password.Password.ToString());//输入合法性验证
                    if (claims.IsAuthenticated && claims.User.Status == AppUserStatus.Enabled)
                    {
#warning "把这些MessageBox.Show()改成友好的窗口或者Tips"
                        MessageBox.Show("登陆成功！");
                        claimsStored = claims;
                        SendClaimsEvent(claims);
                        Close();

                    }
                    else if (claims.User != null&&claims.User.Status == AppUserStatus.NotApproved)
                    {
                        MessageBox.Show("已发送用户注册审批请求,请等待机构管理员审批.");
                    }
                    else
                    {
#warning "把这些MessageBox.Show()改成友好的窗口或者Tips"
                        MessageBox.Show("登录失败，用户名或密码出错，或未通过管理员审批！");
                    }
                }
            }

        }

        public static void GetClaims(SendClaimsDelegate sendClaims)
        {
            if (claimsStored?.IsAuthenticated == true)
            {
                SendClaimsEvent(claimsStored);
                return;
            }

            Login loginWindow = GetWindow();
            loginWindow.Show();
            SendClaimsEvent += sendClaims;
            return;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            base.OnClosing(e);
        }

        public delegate void SendClaimsDelegate(AppUserIdentityClaims claims);

        public static event SendClaimsDelegate SendClaimsEvent;

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (claimsStored == null)
                Environment.Exit(0);
        }
    }
}
