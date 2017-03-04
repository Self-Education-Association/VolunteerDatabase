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

namespace Desktop
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
            if (textBox.Text == "" || textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || passwordBox.Password == "" || comboBox.Text == "")
            {
                //存在空白，信息不完整
            }
            else
            {
                IdentityHelper ih = IdentityHelper.GetInstance();
                if (passwordBox.Password == passwordBox1.Password)
                {
                    string passWord = passwordBox.Password;
                    OrganizationEnum org = ih.Matching(comboBox.Text);
                    AppUser au = new AppUser()
                    {
                        StudentNum = int.Parse(textBox1.Text),
                        AccountName = textBox2.Text,
                        Name = textBox.Text,
                        Mobile = textBox3.Text,
                        Email = textBox4.Text,
                        Room = textBlock5.Text
                    };
                    IdentityResult result = ih.CreateUser(au, passWord, AppRoleEnum.Administrator, org);
                    if (result.Succeeded == true)
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
                    //用MessageBox太丑了，待改
                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }
        //此处限制了键盘输入必须为数字，但是无法检查输入法的中文输入，待解决
    }
}
