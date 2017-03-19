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
    /// Interaction logic for MainTab.xaml
    /// </summary>
    public partial class MainTab : UserControl
    {
        private AppUserIdentityClaims claims;

        public MainTab()
            :this(null)
        {
            InitializeComponent();
        }

        public MainTab(AppUserIdentityClaims claims)
        {
            if (claims == null)
            {
                Login.GetClaims(sendClaimsEventHandler);
                IsEnabled = false;
            }
            else
            {
                this.claims = claims;
            }
        }

        public void sendClaimsEventHandler(AppUserIdentityClaims claims)
        {
            IsEnabled = true;
            this.claims = claims;
            IdentityPage identitypage = IdentityPage.GetInstance(claims);
            //MessageBox.Show("收到令牌啦！");
        }
    }
}
