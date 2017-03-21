using System.Windows.Controls;
using VolunteerDatabase.Helper;

namespace VolunteerDatabase.Desktop.Pages
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
                Login.GetClaims(sendClaimsEventHandler, logOutEventHandler);
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

        public void logOutEventHandler()
        {
            claims = null;
        }
    }
}
