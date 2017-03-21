using System.Windows.Controls;
using VolunteerDatabase.Helper;

namespace VolunteerDatabase.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        IdentityPage identitypage = IdentityPage.GetInstance();
        AppUserIdentityClaims Claims { get; set; }
        
        public SettingsPage()
        {
            Claims = identitypage.Claims;
            InitializeComponent();
        }
    }
}
