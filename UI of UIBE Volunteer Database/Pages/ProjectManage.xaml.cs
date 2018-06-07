using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for ProjectManage.xaml
    /// </summary>
    public partial class ProjectManage : UserControl
    {
        private IdentityPage identitypage = IdentityPage.GetInstance();
        private List<Project> allprojectlist;
        private ProjectManageHelper managehelper;
        private ProjectProgressHelper progresshelper;
        private AppUserIdentityClaims Claims { get; set; }
        private List<Project> current;

        private int ProPgeIndex { get; set; }
        private int MaxProItems = 5;
        private int ProCount
        {
            get
            {
                return current.Count;
             }//?如何count,如果按编号查，只有一个
        }
        public ProjectManage()
        {
            InitializeComponent();
            if (identitypage.Claims != null)
            {
                Claims = identitypage.Claims;
            }
            managehelper = ProjectManageHelper.GetInstance();
            progresshelper = ProjectProgressHelper.GetInstance();
            if (managehelper.ShowProjectList(Claims.User.Organization, true) != null)
                allprojectlist = managehelper.ShowProjectList(Claims.User.Organization, true);
            allprojectlist.AddRange(managehelper.ShowProjectList(Claims.User.Organization, false));
            GenList();
            if (Claims.Roles.Count() == 1 && Claims.IsInRole(AppRoleEnum.OrgnizationMember))
            {
                project_list.ItemsSource = progresshelper.FindAuthorizedProjectsByUser(Claims.User);
                StatusSwitch.IsEnabled = false;
            }
        }

        private void GenList()
        {
            int status = StatusSwitch.SelectedIndex;
            if (managehelper != null && progresshelper != null)
            {
                allprojectlist = managehelper.ShowProjectList(Claims.User.Organization, true);
                allprojectlist.AddRange(managehelper.ShowProjectList(Claims.User.Organization, false));
            }
            if (project_list != null)
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
                current = list;
                ShowProGrid();
                //project_list.ItemsSource = list;
            }
        }

        private void StatusSwitch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GenList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            if (senderButton.DataContext is Project)
            {
                Project project = (Project)senderButton.DataContext;
                var ProjectInformation = new ProjectInformation(this.Claims, project);
                ProjectInformation.Show();
            }
        }

        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {
            if (search_project.Text == "")
            {
            }
            else
            {
                List<Project> Pros = new List<Project>();
                try
                {
                    int num = int.Parse(search_project.Text);
                    Pros.Add(progresshelper.FindProjectByProjectId(num));
                    project_list.ItemsSource = Pros;
                }
                catch (Exception)
                {
                    ModernDialog.ShowMessage("输入非法,仅能输入数字.", "警告", MessageBoxButton.OK);
                }
            }
        }

        private void ShowProGrid()
        {
            List<Project> records = new List<Project>();
            for (int i = ProPgeIndex; i < ProPgeIndex + MaxProItems; i++)
            {
                if (i > ProCount - 1) break;
                records.Add(current[i]);
            }
            project_list.ItemsSource = records;
            ProPge.Content = string.Format("{0}/{1}", ProPgeIndex / MaxProItems + 1, ProCount / MaxProItems + 1);
        }

        private void ProPgePrevious_Click(object sender, RoutedEventArgs e)
        {
            ProPgeIndex = (ProPgeIndex - MaxProItems < 0) ? ProPgeIndex : ProPgeIndex - MaxProItems;
            if (ProPgeIndex - MaxProItems < 0) ProPgePrevious.IsEnabled = false;
            else ProPgeNext.IsEnabled = true;
            ShowProGrid();
        }
        private void ProPgeNext_Click(object sender, RoutedEventArgs e)
        {
            ProPgeIndex = (ProPgeIndex + MaxProItems > ProCount) ? ProPgeIndex : ProPgeIndex + MaxProItems;
            if (ProPgeIndex + MaxProItems > ProCount) ProPgeNext.IsEnabled = false;
            else ProPgePrevious.IsEnabled = true;
            ShowProGrid();
        }

    }
}
