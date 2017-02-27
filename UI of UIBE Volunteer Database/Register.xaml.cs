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
using System.Windows.Shapes;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Interface;

namespace WpfApplication1
{
    /// <summary>
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        private void register_button_Click(object sender, RoutedEventArgs e)
        {
            IdentityHelper ih = IdentityHelper.GetInstance();
            if (passwordBox.Password == passwordBox1.Password)
            {
                string passWord = passwordBox.Password;
            }
            else
            {
                MessageBox.Show("两次密码输入不一致，请进行检查");
            }
            OrganizationEnum org = ih.Matching(comboBox.Text);
            AppUser au = new AppUser()
            {
                StudentNum = textBox1.Text,
                AccountName = textBox2.Text,
                Name= textBox.Text,
                Mobile= textBox3.Text,
                Email= textBox4.Text
            };                
        }
    }
}
