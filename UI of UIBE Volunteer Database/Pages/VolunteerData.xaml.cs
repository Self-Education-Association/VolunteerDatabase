using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;

namespace VolunteerDatabase.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for VolunteerData.xaml
    /// </summary>
    public partial class VolunteerData : UserControl
    {
        private List<Volunteer> sourceList = new List<Volunteer>();
        private IdentityPage identitypage = IdentityPage.GetInstance();
        private AppUserIdentityClaims Claims { get; set; }

        public VolunteerData()
        {
            Claims = identitypage.Claims;
            InitializeComponent();
        }

        private void sendClaimsEventHandler(AppUserIdentityClaims claims)
        {
            this.Claims = claims;
        }

        private void searchbyid_btn_Click(object sender, RoutedEventArgs e)
        {
            if (studentnum.Text == ""||studentnum.Text=="输入学号")
            {
                ModernDialog.ShowMessage("请输入学号.", "", MessageBoxButton.OK);
            }
            else
            {
                var vh = VolunteerHelper.GetInstance();
                if (studentnum.Text != "")
                {
                    try
                    {
                        string num = studentnum.Text;
                        Volunteer volunteer = vh.FindVolunteer(num);
                        if (volunteer == null)
                        {
                            ModernDialog.ShowMessage(string.Format("没有在数据库中找到学号为[{0}]的志愿者.",num.ToString()), "警告", MessageBoxButton.OK);
                        }
                        else
                        {
                            sourceList.Add(vh.FindVolunteer(num));
                            Volunteerdata.ItemsSource = sourceList;
                        }
                    }
                    catch (Exception)
                    {
                        ModernDialog.ShowMessage("学号输入非法,仅能输入数字.", "警告", MessageBoxButton.OK);
                    }

                }
            }
        }

        private void BlacklistDetails_btn_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            if (senderButton.DataContext is Volunteer)
            {
                Volunteer vol = (Volunteer)senderButton.DataContext;
                var Blacklist = new BlackList(vol,null);
                Blacklist.Show();
            }
        }

        private void studentnum_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(studentnum.Text=="输入学号")
                studentnum.Text = "";
        }
    }
}