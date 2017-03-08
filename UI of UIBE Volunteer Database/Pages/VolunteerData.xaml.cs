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
            //InitialData();


        //    this.fivePointStarGroup1.SelectCountChangeEvent +=
        //    new RoutedEventHandler(fivePointStarGroup1_SelectCountChangeEvent);
        //}


        //private void InitialData()
        //{
        //    this.textBox1.Text = this.fivePointStarGroup1.SelectCount.ToString();


        //    this.textBox2.Text = this.fivePointStarGroup1.ItemsCount.ToString();
        //}


        //void fivePointStarGroup1_SelectCountChangeEvent(object sender, RoutedEventArgs e)
        //{
        //    InitialData();
        //}


        //private void button1_Click(object sender, RoutedEventArgs e)
        //{
        //    int selectCount = Convert.ToInt32(this.textBox1.Text);


        //    int allCount = Convert.ToInt32(this.textBox2.Text);


        //    if (allCount < selectCount)
        //    {
        //        MessageBox.Show("参数设置错误!");


        //        return;
        //    }


        //    this.fivePointStarGroup1.ItemsCount = allCount;


        //    this.fivePointStarGroup1.SelectCount = selectCount;
        }
        private void sendClaimsEventHandler(AppUserIdentityClaims claims)
        {
            this.Claims = claims;
#warning "把这些MessageBox.Show()改成友好的窗口或者Tips"
            MessageBox.Show("志愿者库模块收到令牌.");
        }

        private void searchbyid_btn_Click(object sender, RoutedEventArgs e)
        {
            if(studentnum==null)
            {

            }
            else
            {
                
                var vh = VolunteerHelper.GetInstance();
                sourceList.Add(vh.FindVolunteer(int.Parse(studentnum.Text)));
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
