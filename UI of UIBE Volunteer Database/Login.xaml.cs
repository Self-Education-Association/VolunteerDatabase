using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using VolunteerDatabase.Entity;

namespace WpfApplication1
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            var Register = new Register();
            Register.Show();
        }

        private async void login_Click(object sender, RoutedEventArgs e)
        {
            IdentityHelper ih = IdentityHelper.GetInstance();
            //应验证用户名和密码非空，空则不与数据库交互而是弹出提示
            var claims= await ih.CreateClaimsAsync(userid.Text, password.Password.ToString());
            
            if (claims.IsAuthenticated)
            {
                //MessageBox.Show("登陆成功！");
                Mainwindow mainwindow = new Mainwindow(claims);
                mainwindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("登录失败，请检查用户名和密码！");
            }
        }
    }
}
