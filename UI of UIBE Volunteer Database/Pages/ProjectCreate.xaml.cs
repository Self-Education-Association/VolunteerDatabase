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

namespace Desktop.Pages
{
    /// <summary>
    /// Interaction logic for ProjectCreate.xaml
    /// </summary>
    public partial class ProjectCreate : UserControl
    {
        private IdentityPage basepage = IdentityPage.GetInstance();
        private ProjectManageHelper helper;
        private AppUserIdentityClaims Claims { get; set; }
        public ProjectCreate()
        {
            Claims = basepage.Claims;
            helper = ProjectManageHelper.GetInstance();
            //Login.GetClaims(sendClaimsEventHandler);
            InitializeComponent();
        }

        private void create_project_button_Click(object sender, RoutedEventArgs e)
        {
                TextRange textRange = new TextRange(project_details.Document.ContentStart, project_details.Document.ContentEnd);
                if (project_name.Text == "" || project_place.Text == "" || project_time.DisplayDate == null || project_time.DisplayDate < DateTime.Now.AddYears(-20) || project_place.Text == "" || project_maximum.Text == "" || textRange.Text == "")
                {
                    MessageBox.Show("请完整输入所有项目.");
                }
                else
                {
                //可以在这里对RichTextBox做美化               
                ProgressResult result = helper.CreatNewProject(Claims.Holder.Organization, project_time.DisplayDate, project_name.Text, project_place.Text, textRange.Text, int.Parse(project_maximum.Text));
                if (result.Succeeded)
                {
                    MessageBox.Show("项目创建成功!");
                    project_name.Clear();
                    project_place.Clear();
                    project_maximum.Clear();
                    project_details.Document.Blocks.Clear();

                }
                else
                {
                    MessageBox.Show("项目创建失败!错误信息" + string.Join(",", result.Errors));
                }
            }
        }

        // 
    }
}
