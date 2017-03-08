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
        public ProjectInformation(Project pro)
        {
            InitializeComponent();
            Pro = pro;
            if (Pro.ScoreCondition != ProjectScoreCondition.Scored)
            {
                endproject.IsEnabled = false;
            }
            ProInfoShow();
            List<AppUser> users=Pro.Managers.ToList();
            List<Volunteer> vols=Pro.Volunteers.ToList();
            project_manager_list.ItemsSource = users;
            volunteer_list.ItemsSource = vols;
        }
        private void ProInfoShow()
        {
            org.Text = Pro.Organization.ToString();
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

        private void rate_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
