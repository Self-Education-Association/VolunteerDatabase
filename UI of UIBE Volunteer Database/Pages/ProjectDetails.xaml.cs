using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using VolunteerDatabase.Desktop.Pages.InPutVolunteerInBatch;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Interface;
using FirstFloor.ModernUI.Windows.Controls;

namespace VolunteerDatabase.Desktop.Pages
{
    /// <summary>
    /// AddManager.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectInformation : Window
    {
        private Project Pro;
        private AppUserIdentityClaims Claims;

        private int VltCount { get { return Pro.Volunteers.Count; } }
        private int VltPgeIndex { get; set; }
        private int MaxVltItems = 5;

        private int MngCount { get { return Pro.Managers.Count; } }
        private int MngPgeIndex { get; set; }
        private int MaxMngItems = 3;

        public void finishScoringEventHandler()
        {
            ShowVltGrid();
        }

        public void sendClaimsEventHandler(AppUserIdentityClaims claim)
        {
            IsEnabled = true;
            this.Claims = claim;
            IdentityPage identitypage = IdentityPage.GetInstance(claim);
        }

        public void logOutEventHandler()
        {
            Claims = null;
            Close();
        }

        public ProjectInformation(AppUserIdentityClaims Claim, Project pro)
        {
            if (Claim == null)
            {
                Login.GetClaims(sendClaimsEventHandler, logOutEventHandler);
                IsEnabled = false;
            }
            else
            {
                this.Claims = Claim;
                Pro = pro;
            }
            //VltConut = Pro.Volunteers.Count;
            InitializeComponent();
            Auth();
            ProInfoShow();
            ShowVltGrid();
            ShowMngGrid();

        }

        private void ShowVltGrid()
        {
            List<Volunteer> vols = new List<Volunteer>();
            for (int i = VltPgeIndex; i <= VltPgeIndex + MaxVltItems; i++)
            {
                if (i > VltCount - 1) break;
                vols.Add(Pro.Volunteers[i]);
            }
            VltPge.Content = string.Format("{0}/{1}",VltPgeIndex/MaxVltItems+1,VltCount/MaxVltItems+1);
            LblVltListEpt.Visibility = Pro.Volunteers.Count == 0 ? Visibility.Visible : Visibility.Hidden;
            volunteer_list.ItemsSource = vols;
            //this.
        }

        private void ShowMngGrid()
        {
            List<AppUser> mngs = new List<AppUser>();
            for (int i = MngPgeIndex; i <= MngPgeIndex + MaxMngItems; i++)
            {
                if (i > MngCount - 1) break;
                mngs.Add(Pro.Managers[i]);
            }
            MngPge.Content = string.Format("{0}/{1}", MngPgeIndex / MaxMngItems + 1, MngCount / MaxMngItems + 1);
            LblMngListEpt.Visibility =Pro.Managers.Count == 0?Visibility.Visible:Visibility.Hidden;
            project_manager_list.ItemsSource = mngs;
        }

        private void Auth()
        {
            if (Pro.ScoreCondition != ProjectScoreCondition.Scored)
            {
                endproject.IsEnabled = false;
            }
            if (Claims.Roles.Count() == 0 || Pro.Condition == ProjectCondition.Finished)
            {
                AddManager_btn.IsEnabled = false;
                deleteproject_btn.IsEnabled = false;
                endproject.IsEnabled = false;
                piliang.IsEnabled = false;
                AddVolunteer_btn.IsEnabled = false;
            }
            if (Claims.Roles.Count() == 1 && Claims.IsInRole(AppRoleEnum.OrgnizationMember))
            {
                AddManager_btn.IsEnabled = false;
                deleteproject_btn.IsEnabled = false;
                project_manager_list.IsEnabled = false;
                AddManager.IsEnabled = false;
                AddManager_btn.IsEnabled = false;
            }
            if (Claims.Roles.Count() == 1 && Claims.IsInRole(AppRoleEnum.OrgnizationAdministrator))
            {
                endproject.IsEnabled = false;
                piliang.IsEnabled = false;
                AddVolunteer_btn.IsEnabled = false;
            }
        }

        private void ProInfoShow()
        {
            if (Pro != null)
            {
                //org.Text = Pro.Organization.Name;
                Title = Pro.Name;
                project_id.Text = Pro.Id.ToString();
                project_place.Text = Pro.Place;
                project_status.Text = Pro.Condition.ToString();
                project_time.Text = Pro.Time.ToString("yyyy-M-d");
                project_accomodation.Text = Pro.Volunteers.Count() + "/" + Pro.Maximum.ToString();
            }
        }

        private void piliang_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Csv文件|*.csv";
            List<Volunteer> list = new List<Volunteer>();
            if (op.ShowDialog() == true)
            {
                var ch = CsvHelper.GetInstance();
                list = ch.PrepareAddInBatch(op, Pro);
                if (ch.informingMessage.Count() != 0)
                {
                    foreach (string item in ch.informingMessage)
                    {
                        MessageBox.Show(item);
                    }//显示非致命提示信息 此处应建立窗口提示informingMessage,即有改动的信息，然后传多个学号，再调用ch中方法确定新信息
                }
                if (ch.errorList.Count() != 0)
                {
                    foreach (string item in ch.errorList)
                    {
                        MessageBox.Show(item);
                    }
                }//显示致命错误信息
                else
                {
                    InputWindow window = new InputWindow();
                    window.Height = 650;
                    window.Width = 470;
                    DealWithConflict dealer = new DealWithConflict(Pro, list, window);
                    window.FinishInPutEvent += FinishInPutEventHandler;
                    window.Content = dealer;
                    window.Owner = this;
                    window.Show();
                }
                //MessageBox.Show("导入的信息与志愿者库中不一致的条目已被红色高亮标记,请确认保留项目.");
            }
        }

        private void FinishInPutEventHandler()
        {
            ShowVltGrid();
        }

        private void endproject_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mr = MessageBox.Show("确认要结项吗？\n没有被评分的志愿者将被自动评分为4.","确认结项",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if(mr==MessageBoxResult.Yes)
            {
                var pph = ProjectProgressHelper.GetInstance();
                //MessageBox.Show("未单独评分的志愿者所有项全部评分为：4");
                if (Pro != null && Pro.Condition == ProjectCondition.Ongoing)
                {
                    try
                    {
                        var result1 = pph.ScoringDefaultForVolunteers(Pro, new CreditRecord.CreditScore
                        {
                            CmmScore = 4,
                            PncScore = 4,
                            SrvScore = 4
                        }
                        );
                        if (!result1.Succeeded)
                        {
                            MessageBox.Show("评分失败,未能结项\n"+string.Join(",",result1.Errors));
                            return;
                        }
                        if (Pro.ScoreCondition == ProjectScoreCondition.Scored)
                        {
                            var result2 = pph.FinishProject(Pro);
                            if (!result2.Succeeded)
                            {
                                MessageBox.Show("结项失败");
                            }
                        }
                        else
                        {
                            MessageBox.Show("没有评分,未能结项");
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        private void deleteproject_btn_Click(object sender, RoutedEventArgs e)
        {
            var pmh = ProjectManageHelper.GetInstance();
            MessageBoxResult result = MessageBox.Show("确定要删除该项目?", "删除提醒", MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch (result)
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
            if (AddManager.Text == "")
            {
                MessageBox.Show("请输入管理者学号!");
            }
            else
            {
                var pmh = ProjectManageHelper.GetInstance();
                try
                {
                    var result = pmh.AddManager(int.Parse(AddManager.Text), Pro);
                    if (result.Succeeded)
                    {
                        MessageBox.Show("学号为[" + AddManager.Text + "]的用户已经被添加为项目[" + Pro.Name + "]的项目管理者.");
                        ShowMngGrid();
                    }
                    if (!result.Succeeded)
                    {
                        MessageBox.Show("导入失败,错误信息:" + string.Join(",", result.Errors));
                        AddManager.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AddVolunteer_btn_Click(object sender, RoutedEventArgs e)
        {
            if (AddVolunteer.Text == "")
            {
                MessageBox.Show("请输入待添加的志愿者学号。");
            }
            else
            {
                var pph = ProjectProgressHelper.GetInstance();
                var result = pph.SingleVolunteerInputById(int.Parse(AddVolunteer.Text), Pro);
                if (result.Succeeded)
                {
                    MessageBox.Show("学号为[" + AddVolunteer.Text + "]的志愿者已经被添加入项目[" + Pro.Name + "]的志愿者列表.");
                    ShowVltGrid();
                }
                if (!result.Succeeded)
                {
                    MessageBox.Show("导入失败，"+string.Join(",",result.Errors));
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

        /*private void rate_btn_Click(object sender, RoutedEventArgs e)
        {
        }*/

        private void deleteprojectmanager_btn_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            if (senderButton.DataContext is AppUser)
            {
                AppUser Man = (AppUser)senderButton.DataContext;
                if (Man != null)
                {
                    var pmh = ProjectManageHelper.GetInstance();
                    pmh.DeletManager(Man.StudentNum, Pro);
                    ShowMngGrid();
                }
            }
        }

        private void deletevolunteer_btn_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            if (senderButton.DataContext is Volunteer)
            {
                Volunteer Vol = (Volunteer)senderButton.DataContext;
                if (Vol != null)
                {
                    MessageBoxResult mr = MessageBox.Show(string.Format("确认从项目中移除姓名为[{0}],学号为[{1}]的志愿者？", Vol.Name, Vol.StudentNum), "确认移除", MessageBoxButton.YesNo);
                    if (mr == MessageBoxResult.Yes)
                    {
                        var pph = ProjectProgressHelper.GetInstance();
                        pph.DeleteVolunteerFromProject(Vol, Pro);
                        ShowVltGrid();
                    }
                }
            }
        }

        private void saveFile_Click(object sender, RoutedEventArgs e)
        {
            var templateName = "导入模板.csv";
            var sfd = new SaveFileDialog
            {
                Filter = "逗号分隔的列表文件（*.csv）|*.csv",
                DefaultExt = "csv",
                FileName = "导入模板",
                AddExtension = true
            };
            if (sfd.ShowDialog() == true)
            {
                var pathName = sfd.FileName;
                var templatePathName = Environment.CurrentDirectory + @"\" + templateName;
                try
                {
                    File.Copy(templatePathName, pathName, true);
                }
                catch (IOException exception)
                {
                    //TODO: LOG
                }

                MessageBox.Show("导出模板文件成功。");
            }
            else
            {
                MessageBox.Show("用户取消操作。");
            }
        }

        private void BlacklistDetails_btn_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            if (senderButton.DataContext is Volunteer)
            {
                Volunteer Vol = (Volunteer)senderButton.DataContext;
                if (Vol != null)
                {
                    var bl = new BlackList(Vol);
                    bl.Show();
                    bl.Owner = this;
                }
            }
        }

        private void VltPgeNext_Click(object sender, RoutedEventArgs e)
        {
            VltPgeIndex = (VltPgeIndex + MaxVltItems > VltCount) ? VltPgeIndex : VltPgeIndex + MaxVltItems;
            if (VltPgeIndex + MaxVltItems > VltCount) VltPgeNext.IsEnabled = false;
            VltPgePrevious.IsEnabled = true;
            ShowVltGrid();
        }

        private void VltPgePrevious_Click(object sender, RoutedEventArgs e)
        {
            VltPgeIndex = (VltPgeIndex - MaxVltItems < 0) ? VltPgeIndex : VltPgeIndex - MaxVltItems;
            if (VltPgeIndex - MaxVltItems < 0) VltPgePrevious.IsEnabled = false;
            VltPgeNext.IsEnabled = true;
            ShowVltGrid();
        }

        private void MngPgeNext_Click(object sender, RoutedEventArgs e)
        {
            MngPgeIndex = (MngPgeIndex + MaxMngItems > MngCount) ? MngPgeIndex : MngPgeIndex + MaxMngItems;
            if (MngPgeIndex + MaxMngItems > MngCount) MngPgeNext.IsEnabled = false;
            MngPgePrevious.IsEnabled = true;
            ShowMngGrid();
        }

        private void MngPgePrevious_Click(object sender, RoutedEventArgs e)
        {
            MngPgeIndex = (MngPgeIndex - MaxMngItems < 0) ? MngPgeIndex : MngPgeIndex - MaxMngItems;
            if (MngPgeIndex - MaxMngItems < 0) MngPgePrevious.IsEnabled = false;
            MngPgeNext.IsEnabled = true;
            ShowMngGrid();
        }



        private void BtnRate_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            if (senderButton.DataContext is Volunteer)
            {
                Volunteer Vol = (Volunteer)senderButton.DataContext;
                if (Vol != null && Pro.ScoreCondition == ProjectScoreCondition.UnScored)
                {
                    var rp = new Rating(Vol, Pro,finishScoringEventHandler);
                    rp.Owner = this;
                    rp.Show();
                }
            }
            else
            {
                MessageBox.Show("数据上下文非志愿者，请联系管理员。");
            }
        }
        
        //public void Query(int size, int pageIndex)
        //{
        //    Result.Total = Student.Students.Count;
        //    Result.Students = Student.Students.Skip((pageIndex - 1) * size).Take(size).ToList();

        //}
        //private void dataPager_PageChanged(object sender, Resource.PageChangedEventArgs args)
        //{
        //    Query(args.PageSize, args.PageIndex);
        //}
    }
}