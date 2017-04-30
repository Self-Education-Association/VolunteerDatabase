using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;
using System;
using FirstFloor.ModernUI.Windows;

namespace VolunteerDatabase.Desktop.Pages
{
    /// <summary>
    /// Rating.xaml 的交互逻辑
    /// </summary>
    public partial class Rating : UserControl
    {
        private Project pro;
        private Volunteer vol;
        private double temp;
        public Rating(Volunteer Vol,Project Pro)
        { 
            InitializeComponent();
            pro = Pro;
            vol = Vol;
            ProName.Text = Pro.Name;
            VolName.Text = Vol.Name;
            try
            {
                int num1 = int.Parse(Time.Text);
                int num2 = int.Parse(Attitude.Text);
                int num3 = int.Parse(Connection.Text);
                temp = (num1+ num2 + num3) / 3.0;
            }
            catch (Exception)
            {
                ModernDialog.ShowMessage("分数输入非法,仅能输入数字.", "警告", MessageBoxButton.OK);
            }
           
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (!vol.CreditRecords.Exists(o => o.Project.Id == pro.Id))
            {
                var pph = ProjectProgressHelper.GetInstance();
                var result =pph.AddScore(vol, pro, temp);
                if (result.Succeeded)
                {
                    MessageBox.Show("评分成功");
                    //this.Close();
                }
                else
                {
                    ModernDialog.ShowMessage("请检查分数输入是否合法","评分失败",MessageBoxButton.OK);
                }
            }
            else
            {
                var pph = ProjectProgressHelper.GetInstance();
                var result=pph.EditScore(vol,pro,temp);
                if(result.Succeeded)
                {
                    ModernDialog.ShowMessage("分数已经更改","评分成功",MessageBoxButton.OK);
                    //this.Close();
                }
            }          
        }

        private void Time_KeyDown(object sender, KeyEventArgs e)
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
        }
    }
}
