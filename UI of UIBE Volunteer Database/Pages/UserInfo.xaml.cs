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
using VolunteerDatabase.Interface;

namespace WpfApplication1.Pages
{
    /// <summary>
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : UserControl
    {
        public UserInfo()
        {
            InitializeComponent();
        }

        //private void ShowUserMessage()
        //{
        //    string[] orgstring = Claims.User.Organization.OrganizationEnum.ToString().Split('.');
        //    string orgdetail = orgstring.Last();
        //    string[] rolestring = Claims.Roles.ToString().Split('.');
        //    string roledetail = rolestring.Last();
        //    account_name.Text = Claims.User.AccountName;
        //    org.Text = orgdetail;
        //    tel.Text = Claims.User.Mobile;
        //    roles.Text = roledetail;
        //    userid.Text = Claims.User.StudentNum.ToString();
        //    email.Text = Claims.User.Email;
        //    dormitary.Text = Claims.User.Room;
        //}

    }

}
