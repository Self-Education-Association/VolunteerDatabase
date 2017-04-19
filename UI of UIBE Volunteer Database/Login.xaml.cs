using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows.Controls;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Desktop
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private static AppUserIdentityClaims _claimsStored;

        private static readonly object LoginWindowLocker = new object();

        private static Login _loginWindow;

        public static bool IsLogin => _claimsStored?.IsAuthenticated == true;

        protected Login()
        {
            InitializeComponent();
            this.Topmost = true;
        }

        protected static Login GetWindow()
        {
            if (_loginWindow == null)
            {
                lock (LoginWindowLocker)
                {
                    if (_loginWindow == null)
                        _loginWindow = new Login();
                }
            }
            return _loginWindow;
        }

        public static bool LogOut()
        {
            _claimsStored = null;
            SendClaimsEvent = null;
            LogOutEvent?.Invoke();
            LogOutEvent = null;
            return true;
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            var register = new Register();
            register.Show();
            Hide();
        }

        private async void login_btn_Click(object sender, RoutedEventArgs e)
        {
            if (userid.Text == "" || password.Password == "")
            {
                Tips_block.Visibility = Visibility.Visible;
            }
            else
            {
                if (_claimsStored != null && _claimsStored.IsAuthenticated == true)
                {
                    SendClaimsEvent?.Invoke(_claimsStored);
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
                        _claimsStored = claims;
                        SendClaimsEvent?.Invoke(claims);
                        Close();
                        ModernDialog.ShowMessage("登录成功", "信息", MessageBoxButton.OK);                                   
                    }
                    else if (claims.User != null && claims.User.Status == AppUserStatus.NotApproved)
                    {
                        ModernDialog.ShowMessage("已发送用户注册审批请求,请等待机构管理员审批", "注册成功", MessageBoxButton.OK);
                    }
                    else
                    {
                        Hide();
                        ModernDialog.ShowMessage("用户名或密码出错，或未通过管理员审批！", "登录失败", MessageBoxButton.OK);
                        Show();
                    }
                }
            }
        }

        public static void GetClaims(SendClaimsDelegate sendClaims, LogOutDelegate logout)
        {
            if (_claimsStored?.IsAuthenticated == true)
            {
                sendClaims(_claimsStored);
                return;
            }

            Login loginWindow = GetWindow();
            loginWindow.Show();
            SendClaimsEvent += sendClaims;
            return;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_claimsStored == null)
                Application.Current.Shutdown();
            e.Cancel = true;
            Hide();
        }

        public delegate void SendClaimsDelegate(AppUserIdentityClaims claims);

        public delegate void LogOutDelegate();

        public static event SendClaimsDelegate SendClaimsEvent;

        public static event LogOutDelegate LogOutEvent;

    }
}
