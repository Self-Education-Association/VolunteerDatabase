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
        private AppUserIdentityClaims Claims;
        private Window Owner;

        public void sendClaimsEventHandler(AppUserIdentityClaims claim)
        {
            IsEnabled = true;
            this.Claims = claim;
            IdentityPage identitypage = IdentityPage.GetInstance(claim);
        }

        public void logOutEventHandler()
        {
            Claims = null;
            Owner.Close();
        }

        public ChangePassWord(AppUserIdentityClaims Claim,Window owner)
        {
            if (Claim == null)
            {
                Login.GetClaims(sendClaimsEventHandler, logOutEventHandler);
                IsEnabled = false;
            }
            else
            {
                this.Claims = Claim;
                this.Owner = owner;
            }
            InitializeComponent();
        }


        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            IdentityHelper ih = IdentityHelper.GetInstance();
            if (originPasswordBox.Password == "" || newPasswordBox.Password == "")
            {
                informingMessage.Content = "原始密码或新密码不能为空.";
            }
            else if (newPasswordBox.Password != repeatPasswordBox.Password)
            {
                informingMessage.Content = "两次输入的新密码不一致.";
            }
            else if (SecurityHelper.Hash(password: originPasswordBox.Password, salt: Claims.User.Salt) != Claims.User.HashedPassword)
            {
                ModernDialog.ShowMessage("原密码错误.", "提示", MessageBoxButton.OK);
            }
            else
            {
                var ihh = IdentityHelper.GetInstance();
                var re = ihh.ChangePassword(Claims.User.AccountName, originPasswordBox.Password, newPasswordBox.Password);
                if (re.Succeeded)
                {
                    ModernDialog.ShowMessage("修改成功", "成功", MessageBoxButton.OK);
                    Owner.Close();
                }
                else
                {
                    ModernDialog.ShowMessage(re.Errors.ToString(), "提示", MessageBoxButton.OK);
                }
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Owner.Close();
        }

        private void originPasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            informingMessage.Content = "";
        }

        private void newPasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            informingMessage.Content = "";
        }

        private void repeatPasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            informingMessage.Content = "";
        }
    }
}
