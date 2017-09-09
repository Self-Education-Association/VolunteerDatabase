using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows.Controls;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;
using System;

namespace VolunteerDatabase.Desktop.Pages
{
    /// <summary>
    /// Rating.xaml 的交互逻辑
    /// </summary>
    public partial class Rating : Window
    {
        private Project pro;
        private Volunteer vol;
        private double Total { get { return (PncScore + SrvScore + CmmScore) / 3.0; } }

        #region 分数部分
        private int PncScore = -1;
        private int SrvScore = -1;
        private int CmmScore = -1;
        #endregion

        public delegate void FinishScoringEventDelegate();
        public event FinishScoringEventDelegate FinishScoringEvent;

        public Rating(Volunteer Vol,Project Pro,FinishScoringEventDelegate handler=null)
        { 
            InitializeComponent();
            pro = Pro;
            vol = Vol;
            FinishScoringEvent += handler;
            ProName.Text = "项目：  " + Pro.Name;
            VolName.Text = "志愿者：" + Vol.Name;
            /*try
            {
                int num1 = int.Parse(Time.Text);
                int num2 = int.Parse(Attitude.Text);
                int num3 = int.Parse(Connection.Text);
                temp = (num1+ num2 + num3) / 3.0;
            }
            catch (Exception)
            {
                ModernDialog.ShowMessage("分数输入非法,仅能输入数字.", "警告", MessageBoxButton.OK);
            }*/
           
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (PncScore == -1 || SrvScore == -1 || CmmScore == -1) 
            {
                MessageBox.Show("仍有未评分的项目.\n所有项评分才能继续.");
            }
            else
            {
                if (!vol.CreditRecords.Exists(o => o.Project.Id == pro.Id))
                {
                    var pph = ProjectProgressHelper.GetInstance();
                    CreditRecord.CreditScore s = new CreditRecord.CreditScore
                    {
                        PncScore = this.PncScore,
                        SrvScore = this.SrvScore,
                        CmmScore = this.CmmScore
                    };
                    var result = pph.AddScore(vol, pro, s);
                    if (result.Succeeded)
                    {
                        MessageBox.Show("评分成功!");
                        FinishScoringEvent?.Invoke();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("请检查分数输入是否合法.", "评分失败", MessageBoxButton.OK);
                        FinishScoringEvent?.Invoke();
                    }
                }
                else
                {
                    var pph = ProjectProgressHelper.GetInstance();
                    CreditRecord.CreditScore s = new CreditRecord.CreditScore
                    {
                        PncScore = this.PncScore,
                        SrvScore = this.SrvScore,
                        CmmScore = this.CmmScore
                    };
                    var result = pph.EditScore(vol, pro, s);
                    if (result.Succeeded)
                    {
                        MessageBox.Show("该志愿者本项目的评分已经更改.", "评分成功", MessageBoxButton.OK);
                        FinishScoringEvent?.Invoke();
                        this.Close();
                        
                    }
                    else
                    {
                        MessageBox.Show("请检查分数输入是否合法."+string.Join(",",result.Errors), "评分失败", MessageBoxButton.OK);
                        FinishScoringEvent?.Invoke();
                        this.Close();
                    }
                }
            }
            
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region 评分部分
        private void PncScore1_Checked(object sender, RoutedEventArgs e)
        {
            PncScore = 1;
        }

        private void PncScore2_Checked(object sender, RoutedEventArgs e)
        {
            PncScore = 2;
        }

        private void PncScore3_Checked(object sender, RoutedEventArgs e)
        {
            PncScore = 3;
        }

        private void PncScore4_Checked(object sender, RoutedEventArgs e)
        {
            PncScore = 4;
        }

        private void PncScore5_Checked(object sender, RoutedEventArgs e)
        {
            PncScore = 5;
        }

        private void SrvScore1_Checked(object sender, RoutedEventArgs e)
        {
            SrvScore = 1;
        }

        private void SrvScore2_Checked(object sender, RoutedEventArgs e)
        {
            SrvScore = 2;
        }

        private void SrvScore3_Checked(object sender, RoutedEventArgs e)
        {
            SrvScore = 3;
        }

        private void SrvScore4_Checked(object sender, RoutedEventArgs e)
        {
            SrvScore = 4;
        }

        private void SrvScore5_Checked(object sender, RoutedEventArgs e)
        {
            SrvScore = 5;
        }

        private void CmmScore1_Checked(object sender, RoutedEventArgs e)
        {
            CmmScore = 1;
        }

        private void CmmScore2_Checked(object sender, RoutedEventArgs e)
        {
            CmmScore = 2;
        }

        private void CmmScore3_Checked(object sender, RoutedEventArgs e)
        {
            CmmScore = 3;
        }

        private void CmmScore4_Checked(object sender, RoutedEventArgs e)
        {
            CmmScore = 4;
        }

        private void CmmScore5_Checked(object sender, RoutedEventArgs e)
        {
            CmmScore = 5;
        }

        /*private void Time_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }

        private void Attitude_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }

        private void Connection_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }*/
        #endregion

        
    }
}
