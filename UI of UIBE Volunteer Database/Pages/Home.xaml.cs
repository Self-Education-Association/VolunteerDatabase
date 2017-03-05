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

namespace Desktop.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
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
            IdentityHelper ih = IdentityHelper.GetInstance();
            if (userid.Text == "" || password.Password.ToString() == "")
            {

            }
            else
            {
                var claims = await ih.CreateClaimsAsync(userid.Text, password.Password.ToString());
                if (claims.IsAuthenticated)
                {
                    //MessageBox.Show("登陆成功！");
                }
                else
                {
                    MessageBox.Show("登录失败，请检查用户名和密码！");
                }
            }
            
        }
    }
}
