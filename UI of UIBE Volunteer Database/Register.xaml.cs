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
using FirstFloor.ModernUI.Windows.Controls;

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
            if (wholename.Text == "" || studentid.Text == "" || accountname.Text == "" || telephonenumber.Text == "" || emailadress.Text == "" || dormitaryadress.Text == "" || passwordBox.Password == "" || comboBox.Text == "")
            {
                //MessageBox.Show("信息输入不完整,请检查后重试.");//存在空白，信息不完整
            }
            else
            {
                IdentityHelper ih = IdentityHelper.GetInstance();
                if (passwordBox.Password == passwordBox1.Password)
                {
                    string passWord = passwordBox.Password;
                    OrganizationEnum org = ih.Matching(comboBox.Text);
                    try
                    {
                        int studentnum = int.Parse(studentid.Text);
                    }
                    catch(Exception)
                    {
#warning "把这些MessageBox.Show()改成友好的窗口或者Tips"
                        MessageBox.Show("学号输入非法,仅能输入数字.");
                    }
                    AppUser au = new AppUser()
                    {
                        StudentNum = int.Parse(studentid.Text),
                        AccountName = accountname.Text,
                        Name = wholename.Text,
                        Mobile = telephonenumber.Text,
                        Email = emailadress.Text,
                        Room = dormitaryadress.Text,
                        Status = AppUserStatus.Enabled
                    };
                    IdentityResult result = ih.CreateUser(au, passWord, AppRoleEnum.OrgnizationAdministrator, org);
                    if (result.Succeeded == true)
                    {
#warning "把这些MessageBox.Show()改成友好的窗口或者Tips"
                        MessageBox.Show("注册成功!已发送注册审批请求,请等待管理员审批.");
                        this.Close();
                    }
                    else
                    {
#warning "把这些MessageBox.Show()改成友好的窗口或者Tips"
                        MessageBox.Show("注册失败，错误信息：" + string.Join(",", result.Errors));
                    }
                }
                else
                {
#warning "把这些MessageBox.Show()改成友好的窗口或者Tips"
                    MessageBox.Show("两次密码输入不一致，请核对");
                    //用MessageBox太丑了，待改
                }
            }
        }

        private void studentid_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }

        private void telephonenumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }

        }       
        //此处限制了键盘输入必须为数字，但是无法检查输入法的中文输入，待解决
        //动态展现：用户名已经被占用
    }
}
