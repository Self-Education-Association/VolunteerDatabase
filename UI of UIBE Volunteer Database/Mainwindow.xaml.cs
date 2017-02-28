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
using VolunteerDatabase.Entity;

namespace WpfApplication1
{
    /// <summary>
    /// Mainwindow.xaml 的交互逻辑
    /// </summary>
    public partial class Mainwindow : Window
    {
        private AppUserIdentityClaims claim;
        
        public Mainwindow(AppUserIdentityClaims claims)
        {
            claim = claims;
            if (claims.Roles.Contains(AppRoleEnum.OrgnizationAdministrator))
            {
                NewProject.IsEnabled = true;
                UserApproval.IsEnabled = true;
                ProjectManage.IsEnabled = true;
                UserInfo.IsEnabled = true;
            }
            if (claims.Roles.Contains(AppRoleEnum.OrgnizationMember))
            {
                ProjectManage.IsEnabled = true;
                UserInfo.IsEnabled = true;
            }
            if(claim.Roles.Contains(AppRoleEnum.Administrator))
            {
                NewProject.IsEnabled = true;
                UserApproval.IsEnabled = true;
                ProjectManage.IsEnabled = true;
            }
            InitializeComponent();
        }    

        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void create_project_button_Click(object sender, RoutedEventArgs e)
        {
            ProjectManageHelper pmh = ProjectManageHelper.GetInstance();
            TextRange textRange = new TextRange(project_detail.Document.ContentStart,project_detail.Document.ContentEnd);
            pmh.CreatNewProject(claim.User.Organization, project_time.Text, project_name.Text, project_place.Text, textRange.Text, int.Parse(max.Text));
        }
    }
}
