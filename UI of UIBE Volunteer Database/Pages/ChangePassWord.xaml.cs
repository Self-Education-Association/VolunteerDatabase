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
using FirstFloor.ModernUI.Windows.Controls;
using VolunteerDatabase.Helper;

namespace VolunteerDatabase.Desktop.Pages
{
    /// <summary>
    /// ChangePassWord.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePassWord : UserControl
    {
        public ChangePassWord()
        {
            InitializeComponent();
        }


        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            IdentityHelper ih = IdentityHelper.GetInstance();
            if (originPasswordBox.Password == "" || newPasswordBox.Password=="")
            {
                ModernDialog.ShowMessage("原始密码或新密码不能为空.","提示",MessageBoxButton.OK);
            }
            if(originPasswordBox.Password!=newPasswordBox.Password)
            {
                ModernDialog.ShowMessage("两次输入的密码不一致.", "提示", MessageBoxButton.OK);
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
