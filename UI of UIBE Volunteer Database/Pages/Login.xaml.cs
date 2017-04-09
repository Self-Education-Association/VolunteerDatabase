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
using VolunteerDatabase.Interface;
using FirstFloor.ModernUI.Windows.Controls;

namespace VolunteerDatabase.Desktop.Pages
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : UserControl
    {
        private static AppUserIdentityClaims _claimsStored;

        private static readonly object LoginWindowLocker = new object();

        private static WelcomePage _loginWindow;

        public static bool IsLogin => _claimsStored?.IsAuthenticated == true;

        protected Login()
        {
            InitializeComponent();
        }

        protected static WelcomePage GetWindow()
        {
            if (_loginWindow == null)
            {
                lock (LoginWindowLocker)
                {
                    if (_loginWindow == null)
                        _loginWindow = new WelcomePage();
                }
            }
            return _loginWindow;
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
                        ModernDialog.ShowMessage("登陆成功", " ", MessageBoxButton.OK);
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
    }
}
