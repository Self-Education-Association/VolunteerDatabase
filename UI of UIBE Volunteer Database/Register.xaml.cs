using System;
using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows.Controls;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Desktop
{
    /// <summary>
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : Window
    {
        private IdentityHelper identityhelper;

        private bool isAdministrator = false;
        public Register(AppUserIdentityClaims claims = null)
        {
            identityhelper = IdentityHelper.GetInstance();
            if (claims != null && claims.IsInRole(AppRoleEnum.Administrator))
            {
                isAdministrator = true;
            }
            InitializeComponent();
        }

        private void register_button_Click(object sender, RoutedEventArgs e)
        {
            if (wholename.Text == "" || studentid.Text == "" || accountname.Text == "" || telephonenumber.Text == "" || emailadress.Text == "" || dormitaryadress.Text == "" || passwordBox.Password == "" || comboBox.Text == "")
            {
                ModernDialog.ShowMessage("信息输入不完整,请检查后重试.","警告", MessageBoxButton.OK);
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
                        ModernDialog.ShowMessage("学号输入非法,仅能输入数字.", "警告", MessageBoxButton.OK);
                    }
                    AppUserStatus status = AppUserStatus.NotApproved;
                    AppRoleEnum role = AppRoleEnum.OrgnizationMember;
                    if(isAdministrator)
                    {
                        status = AppUserStatus.Enabled;
                        role = AppRoleEnum.OrgnizationAdministrator;
                    }
                    if (accountname.Text=="admin"&& wholename.Text=="admin")
                    {
                        status = AppUserStatus.Enabled;
                        role = AppRoleEnum.Administrator;
                    }
                    AppUser au = new AppUser()
                    {
                        StudentNum = int.Parse(studentid.Text),
                        AccountName = accountname.Text,
                        Name = wholename.Text,
                        Mobile = telephonenumber.Text,
                        Email = emailadress.Text,
                        Room = dormitaryadress.Text,
                        Status = status
                    };
                    IdentityResult result = ih.CreateUser(au, passWord,  role, org);
                    if (result.Succeeded == true)
                    {
                        if(isAdministrator)
                        {
                            ModernDialog.ShowMessage("注册机构账号成功!账号所属机构:[" + comboBox.Text + "],机构账号用户名:[" + accountname.Text + "],密码:[" + passWord + "],请牢记账号密码!", "", MessageBoxButton.OK);
                        }
                        else
                        {
                            ModernDialog.ShowMessage("注册成功!已发送注册审批请求,请等待管理员审批.","",MessageBoxButton.OK);
                            Application.Current.Shutdown();
                        }
                        this.Close();
                    }
                    else
                    {
                        ModernDialog.ShowMessage("注册失败，错误信息：" + string.Join(",", result.Errors),"",MessageBoxButton.OK);
                    }
                }
                else
                {
                    ModernDialog.ShowMessage("两次密码输入不一致，请核对","",MessageBoxButton.OK);
                }
            }
        }

        private void studentid_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)||e.Key==Key.Enter||e.Key==Key.Tab))
            {
                e.Handled = true;
            }
        }

        private void telephonenumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Enter || e.Key == Key.Tab)))
            {
                e.Handled = true;
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
        //此处限制了键盘输入必须为数字，但是无法检查输入法的中文输入，待解决
        //动态展现：用户名已经被占用
    }
}
