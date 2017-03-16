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
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;

namespace Desktop.Pages
{
    /// <summary>
    /// Interaction logic for ProjectManage.xaml
    /// </summary>
    public partial class ProjectManage : UserControl
    {
        IdentityPage identitypage = IdentityPage.GetInstance();
        List<Project> allprojectlist;
        ProjectManageHelper managehelper;
        ProjectProgressHelper progresshelper;
        private AppUserIdentityClaims Claims { get; set; }
        public ProjectManage()
        {
            InitializeComponent();
            Claims = identitypage.Claims;
            managehelper = ProjectManageHelper.GetInstance();
            progresshelper = ProjectProgressHelper.GetInstance();
            allprojectlist = managehelper.ShowProjectList(Claims.User.Organization,true);
            allprojectlist.AddRange(managehelper.ShowProjectList(Claims.User.Organization, false));
            ShowList(StatusSwitch.SelectedIndex);
            //最后通过绑定后台资源实现列表内容更新
            //project_list.ItemsSource = helper.FindAuthorizedProjectsByUser(Claims.User);
            //此处逻辑未完成，仅为OrgMember时应该禁用选择项目状态按钮；此外应该配置切换到该页面的刷新
        }

        private void ShowList(int status)
        {
            List<Project> list = new List<Project>();
            switch (status)
            {
                case 0:
                    {
                        list = allprojectlist;
                    }
                    break;
                case 1:
                    foreach (Project project in allprojectlist)
                    {
                        if (project.Condition == ProjectCondition.Ongoing)
                            list.Add(project);
                    }
                    break;
                case 2:
                    foreach (Project project in allprojectlist)
                    {
                        if (project.Condition == ProjectCondition.Finished)
                            list.Add(project);
                    }
                    break;
                default:
                    break;
            }
            project_list.ItemsSource = list;
        }


        private void StatusSwitch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(managehelper !=null && progresshelper != null)
            {
                allprojectlist = managehelper.ShowProjectList(Claims.User.Organization, true);
                allprojectlist.AddRange(managehelper.ShowProjectList(Claims.User.Organization, false));
            }
            if (project_list != null)
                ShowList(StatusSwitch.SelectedIndex);
        }


        private List<Project> testCreateProjectList()
        {
            //ProjectManageHelper helper1 = ProjectManageHelper.GetInstance();
            // ProjectProgressHelper helper2 = ProjectProgressHelper.GetInstance();

            Volunteer v1 = new Volunteer
            {
                StudentNum = 5551,
                Mobile = "13812345678",
                Name = "AddTest",
                Email = "AddTest@test.com",
                Class = "AddTestClass",
                Room = "AddTestRoom"
            };

            Volunteer v2 = new Volunteer
            {
                StudentNum = 5552,
                Mobile = "13812345671",
                Name = "AddTest2",
                Email = "AddTest2@test.com",
                Class = "AddTestClass2",
                Room = "AddTestRoom2"
            };
            List<Volunteer> list = new List<Volunteer>();
            list.Add(v1);
            list.Add(v2);

            Project p = new Project
            {
                Id = 999,
                CreatTime = DateTime.MinValue,
                Name = "A Project",
                Condition = ProjectCondition.Ongoing,
                Organization = Claims.User.Organization,
                Time = DateTime.MinValue,
                Details = "A Test Project.",
                Maximum = 70,
                Place = "UIBE",
                Volunteers = list,
                Managers = new List<AppUser>()


            };

            Project p2 = new Project
            {
                Id = 998,
                CreatTime = DateTime.MinValue,
                Name = "A Finished Project",
                Condition = ProjectCondition.Finished,
                Organization = Claims.User.Organization,
                Time = DateTime.MinValue,
                Details = "A Test Project.",
                Maximum = 70,
                Place = "UIBE",
                Volunteers = list,
                Managers = new List<AppUser>()


            };
            p.Managers.Add(Claims.User);

            List<Project> projects = new List<Project>();
            projects.Add(p);
            projects.Add(p2);
            return projects;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            if(senderButton.DataContext is Project)
            {
                Project project = (Project)senderButton.DataContext;
                var ProjectInformation = new ProjectInformation(this.Claims,project);
                ProjectInformation.Show();
            }          
        }
        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {
            if(search_project.Text=="")
            {

            }
            else
            {
                List<Project> Pros = new List<Project>();
                Pros.Add(progresshelper.FindProjectByProjectId(int.Parse(search_project.Text)));
                project_list.ItemsSource = Pros;
            }
        }
    }
}