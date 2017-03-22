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
        private List<Volunteer> sourceList=new List<Volunteer>();
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
            if(studentnum==null)
            {

            }
            else
            {
                
                var vh = VolunteerHelper.GetInstance();
                if(studentnum.Text!="")
                {
                     try
                    {
                        int num = int.Parse(studentnum.Text);
                        sourceList.Add(vh.FindVolunteer(num));
                    }
                    catch(Exception)
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
                var Blacklist = new BlackList(vol);
                Blacklist.Show();
            }
        }
    }
}
