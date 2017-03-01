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
                OrganizationEnum org = ih.Matching(comboBox.Text);
                AppUser au = new AppUser()
                {
                    StudentNum = textBox1.Text,
                    AccountName = textBox2.Text,
                    Name = textBox.Text,
                    Mobile = textBox3.Text,
                    Email = textBox4.Text
                };
                IdentityResult result = ih.CreateUser(au, passWord, AppRoleEnum.Administrator, org);
                if(result.Succeeded == true)
                {
                    MessageBox.Show("注册成功！");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("注册失败，错误信息：" + string.Join(",", result.Errors));
                }
            }

            else
            {
                MessageBox.Show("两次密码输入不一致，请进行检查");
                //前端需要检验：  1所有字段不为空 2字段符合要求（没有非法字符，学号为Numeric） 3用户名是否重复
                //用户名是否重复已经由createuser函数完成了，非空需要检查
            }
            
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }

        }
    }
}
