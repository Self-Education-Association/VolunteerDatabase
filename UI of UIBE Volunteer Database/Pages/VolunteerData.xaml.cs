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

namespace Desktop.Pages
{
    /// <summary>
    /// Interaction logic for VolunteerData.xaml
    /// </summary>
    public partial class VolunteerData : UserControl
    {
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
            MessageBox.Show("志愿者库模块收到令牌.");
        }
    }
}
