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
            if(Claims!=null)
            {
                Claims = identitypage.Claims;
            }
            managehelper = ProjectManageHelper.GetInstance();
            progresshelper = ProjectProgressHelper.GetInstance();
            allprojectlist = managehelper.ShowProjectList(Claims.User.Organization,true);
            allprojectlist.AddRange(managehelper.ShowProjectList(Claims.User.Organization, false));
            ShowList(StatusSwitch.SelectedIndex);
            if (Claims.Roles.Count() == 1 && Claims.IsInRole(AppRoleEnum.OrgnizationMember))
            {
                project_list.ItemsSource = progresshelper.FindAuthorizedProjectsByUser(Claims.User);
                StatusSwitch.IsEnabled = false;
            }
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