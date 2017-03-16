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
using FirstFloor.ModernUI.Windows;
using Microsoft.Win32;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;

namespace Desktop.Pages
{
    /// <summary>
    /// AddManager.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectInformation : Window
    {
        private Project Pro;
        private AppUserIdentityClaims Claims;
        public ProjectInformation(AppUserIdentityClaims Claim, Project pro)
        {
            if (Claim == null)
            {
                Login.GetClaims(sendClaimsEventHandler);
                IsEnabled = false;
            }
            else
            {
                this.Claims = Claim;
                Pro = pro;
            }
        }
        public void sendClaimsEventHandler(AppUserIdentityClaims claim)
        {
            IsEnabled = true;
            this.Claims = claim;
            IdentityPage identitypage = IdentityPage.GetInstance(claim);
        }

        public ProjectInformation(Project pro) :this(null,pro)
        {
            InitializeComponent();        
            if (Pro.ScoreCondition != ProjectScoreCondition.Scored)
            {
                endproject.IsEnabled = false;
            }
            ProInfoShow();
            List<AppUser> users=Pro.Managers.ToList();
            List<Volunteer> vols=Pro.Volunteers.ToList();
            project_manager_list.ItemsSource = users;
            volunteer_list.ItemsSource = vols;
            if (Claims.Roles.Count() == 0)
            {
                AddManager_btn.IsEnabled = false;
                deleteproject_btn.IsEnabled = false;
                endproject.IsEnabled = false;
                yijianpingfen.IsEnabled = false;
                piliang.IsEnabled = false;
                AddVolunteer_btn.IsEnabled = false;
            }
            if (Claims.Roles.Count() == 1&&Claims.IsInRole(AppRoleEnum.OrgnizationMember))
            {
                AddManager_btn.IsEnabled = false;
                deleteproject_btn.IsEnabled = false;
            }
            if (Claims.Roles.Count() == 1 && Claims.IsInRole(AppRoleEnum.OrgnizationAdministrator))
            {
                endproject.IsEnabled = false;
                yijianpingfen.IsEnabled = false;
                piliang.IsEnabled = false;
                AddVolunteer_btn.IsEnabled = false;
            }
            //此处权限控制不完善，无法操作DataGrid中两个删除按钮的IsEnabled属性，删除事件也还没有建立，存在传值问题    
        }
        private void ProInfoShow()
        {
            org.Text = Pro.Organization.Name;
            project_name.Text = Pro.Name;
            project_id.Text = Pro.Id.ToString();
            project_place.Text = Pro.Place;
            project_status.Text = Pro.Condition.ToString();
            project_time.Text = Pro.Time.ToString();
            project_accomodation.Text = Pro.Volunteers.Count() + "/" + Pro.Maximum.ToString();
        }

        private void piliang_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Csv文件|*.csv";
            if (op.ShowDialog() == true)
            {
                var ch = CsvHelper.GetInstance();
                ch.MassiveVolunteersInput(op, Pro);
                if(ch.informingMessage.Count()!=0&&ch.informingMessage.Count()!=1)
                {
                    //此处应提示informingMessage,即有改动的信息，然后传多个学号，调用ch中方法确定新信息
                }
                else
                {
                    //提示导入失败
                }
            }
        }

        private void endproject_Click(object sender, RoutedEventArgs e)
        {
            {
                var pph = ProjectProgressHelper.GetInstance();
                var result = pph.FinishProject(Pro);
                if(!result.Succeeded)
                {
                    //此处应提示结项失败
                }
            }

        }

        private void yijianpingfen_Click(object sender, RoutedEventArgs e)
        {
            var pph = ProjectProgressHelper.GetInstance();
            var result = pph.ScoringDefaultForVolunteers(Pro, 4);
        }

        private void deleteproject_btn_Click(object sender, RoutedEventArgs e)
        {
            var pmh = ProjectManageHelper.GetInstance();
            MessageBoxResult result=MessageBox.Show("确定要删除该项目?", "删除提醒", MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch(result)
            {
                case MessageBoxResult.Yes:
                    pmh.ProjectDelete(Pro);
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }

        }

        private void AddManager_btn_Click(object sender, RoutedEventArgs e)
        {
            if(AddManager.Text=="")
            {

            }
            else
            { 
                var pmh = ProjectManageHelper.GetInstance();
                var result = pmh.AddManager(int.Parse(AddManager.Text), Pro);
                if(!result.Succeeded)
                {
                    //此处应提示添加失败
                    AddManager.Clear();
                }
            }
        }

        private void AddVolunteer_btn_Click(object sender, RoutedEventArgs e)
        {
            if(AddVolunteer.Text=="")
            {

            }
            else
            {
                var pph = ProjectProgressHelper.GetInstance();
                var result = pph.SingleVolunteerInputById(int.Parse(AddVolunteer.Text), Pro);
                if (!result.Succeeded)
                {
                    //此处应提示添加失败
                    AddVolunteer.Clear();
                }
            }          
        }

        private void AddManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }

        private void AddVolunteer_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }

        private void AddManager_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        //这个方法没什么用吧？不知道为什么会出现在这里
        private void rate_btn_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            if (senderButton.DataContext is Volunteer)
            {
                Volunteer Vol = (Volunteer)senderButton.DataContext;
            }
            //此处传值问题未解决，如何导出三个分数框中的值并且和对应行vol进行匹配？
        }

        private void shoushiqingkuang_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
        }

        private void fuwutaidu_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb2 = sender as TextBox;
        }

        private void tongxinqingkuang_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb3 = sender as TextBox;
        }
    }
}
