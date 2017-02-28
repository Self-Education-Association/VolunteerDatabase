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
using VolunteerDatabase.Helper;
using VolunteerDatabase.Interface;

namespace WpfApplication1
{
    /// <summary>
    /// Mainwindow.xaml 的交互逻辑
    /// </summary>
    public partial class Mainwindow : Window
    {
        private AppUserIdentityClaims Claims { get; set; }
        private ProjectManageHelper Projecthelper { get; set; }
        public Mainwindow(AppUserIdentityClaims claims)
        {
            InitializeComponent();
            Claims = claims;
            on_window_Create();

        }

        private void on_window_Create()
        {
            Projecthelper = ProjectManageHelper.GetInstance();
            if (Claims.IsInRole(AppRoleEnum.Administrator))
            {
                Project_Manage.IsEnabled = true;
                Project_Add.IsEnabled = true;
                User_Approve.IsEnabled = true;
                Volunteer_Info.IsEnabled = true;
                User_Info.IsEnabled = true;
            }
            else if(Claims.IsInRole(AppRoleEnum.OrgnizationAdministrator))
            {
                Project_Manage.IsEnabled = true;
                Project_Add.IsEnabled = true;
                User_Approve.IsEnabled = true;
                Volunteer_Info.IsEnabled = true;
                User_Info.IsEnabled = true;
            }
            else if(Claims.IsInRole(AppRoleEnum.OrgnizationMember))
            {
                Project_Manage.IsEnabled = true;
                Project_Add.IsEnabled = false;
                User_Approve.IsEnabled = false;
                Volunteer_Info.IsEnabled = true;
                User_Info.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("用户身份非法,请重新登陆后再试.");
                Main_Tabcontrol.IsEnabled = false;
            }
            var list = Projecthelper.ShowProjectList(Claims.Holder.Organization, true);
            project_list.ItemsSource = list;

            //后台缺少一个FindProjectListByManager方法
        }

        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void create_project_button_Click(object sender, RoutedEventArgs e)
        {
            if(project_name.Text ==""||project_place.Text==""||project_time.DisplayDate==null||project_time.DisplayDate<DateTime.Now.AddYears(-20)||project_place.Text==""||project_maximum.Text==""||project_details.Text=="")
            {
                MessageBox.Show("请完整输入所有项目.");
            }
            else
            {
                ProgressResult result = Projecthelper.CreatNewProject(Claims.Holder.Organization, project_time.DisplayDate, project_name.Text, project_place.Text, project_details.Text, int.Parse(project_maximum.Text));
                if(result.Succeeded==true)
                {
                    MessageBox.Show("项目创建成功!");
                    project_name.Text = "";
                    project_place.Text = "";
                    project_maximum.Text = "";
                    project_details.Text = "";
                }
                else
                {
                    MessageBox.Show("项目创建失败!错误信息" + string.Join(",", result.Errors));
                }
            }

                    //找一下text是哪一个
        }

        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
