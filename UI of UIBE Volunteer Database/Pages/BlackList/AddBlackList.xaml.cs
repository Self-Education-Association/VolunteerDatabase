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

namespace VolunteerDatabase.Desktop.Pages
{
    /// <summary>
    /// AddBlackList.xaml 的交互逻辑
    /// </summary>
    public partial class AddBlackList : Window
    {
        IdentityPage id=IdentityPage.GetInstance();
        Project project;
        Volunteer volunteer;
        BlackListHelper bhelper;
        List<string> drselection;

        public AddBlackList(Volunteer v,Project p)
        {
            InitializeComponent();
            project = p;
            volunteer = v;
            bhelper = BlackListHelper.GetInstance();
            cmbTime.Items.Add("一个月");
            cmbTime.Items.Add("三个月");
            cmbTime.Items.Add("半年");
            cmbTime.Items.Add("一年");
            cmbTime.SelectedIndex = 1;
            
        }
        public delegate void FinishAddDelegate();
        public event FinishAddDelegate FinishAddEvent;

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("确认要添加此黑名单记录？\n该操作不可逆。", "确认", MessageBoxButton.OKCancel);
            if(dr==MessageBoxResult.OK)
            {
                DateTime endtime;
                switch (cmbTime.SelectedIndex)
                {
                    case 1:
                        endtime = DateTime.Now.AddMonths(1);
                        break;
                    case 2:
                        endtime = DateTime.Now.AddMonths(3);
                        break;
                    case 3:
                        endtime = DateTime.Now.AddMonths(6);
                        break;
                    case 4:
                        endtime = DateTime.Now.AddYears(1);
                        break;
                    default:
                        endtime = DateTime.Now.AddMonths(3);
                        break;
                }
                var result = bhelper.AddBlackListRecord(txtReason.Text, volunteer, id.Claims.User, endtime, id.Claims.User.Organization, project);
                if (result.Succeeded)
                {
                    MessageBox.Show("黑名单添加成功");
                }
                else
                {
                    MessageBox.Show("添加失败,原因:" + result.ErrorString);
                }
                FinishAddEvent?.Invoke();
            }
            
        }
    }
}
