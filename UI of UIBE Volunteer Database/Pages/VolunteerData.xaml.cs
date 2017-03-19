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

namespace Desktop.Pages
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
                    sourceList.Add(vh.FindVolunteer(int.Parse(studentnum.Text)));
                    App.DoEvents();
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
