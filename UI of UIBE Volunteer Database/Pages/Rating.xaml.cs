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
    /// Rating.xaml 的交互逻辑
    /// </summary>
    public partial class Rating : Window
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
            temp= (int.Parse(Time.Text) + int.Parse(Attitude.Text) + int.Parse(Connection.Text)) / 3.0;
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
                    this.Close();
                }
                else
                {
                    MessageBox.Show("评分失败，请检查分数输入是否合法");
                }
            }
            else
            {
                var pph = ProjectProgressHelper.GetInstance();
                var result=pph.EditScore(vol,pro,temp);
                if(result.Succeeded)
                {
                    MessageBox.Show("分数已经更改");
                    this.Close();
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
