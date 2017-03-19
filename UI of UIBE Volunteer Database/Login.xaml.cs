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
using Desktop.TipsPages;
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

        public static bool IsLogin
        {
            get
            {
                return claimsStored?.IsAuthenticated == true;
            }
        }

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

        public static bool LogOut()
        {
            claimsStored = null;
            SendClaimsEvent = null;
            LogOutEvent?.Invoke();
            LogOutEvent = null;
            return true;
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
                        LoginDialog1.ShowMessage("登陆成功", " ", MessageBoxButton.OK);
                        claimsStored = claims;
                        SendClaimsEvent(claims);
                        Close();

                    }
                    else if (claims.User != null && claims.User.Status == AppUserStatus.NotApproved)
                    {
                        ModernDialog.ShowMessage("已发送用户注册审批请求,请等待机构管理员审批", "注册成功", MessageBoxButton.OK);
                    }
                    else
                    {
                        ModernDialog.ShowMessage("用户名或密码出错，或未通过管理员审批！", "登录失败", MessageBoxButton.OK);
                    }
                }
            }
        }

        public static void GetClaims(SendClaimsDelegate sendClaims, LogOutDelegate logout)
        {
            if (claimsStored?.IsAuthenticated == true)
            {
                sendClaims(claimsStored);
                return;
            }

            Login loginWindow = GetWindow();
            loginWindow.Show();
            SendClaimsEvent += sendClaims;
            return;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (claimsStored == null)
                Application.Current.Shutdown();
            e.Cancel = true;
            Hide();
        }

        public delegate void SendClaimsDelegate(AppUserIdentityClaims claims);

        public delegate void LogOutDelegate();

        public static event SendClaimsDelegate SendClaimsEvent;

        public static event LogOutDelegate LogOutEvent;

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }
    }
}
