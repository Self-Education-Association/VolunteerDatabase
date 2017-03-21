using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using VolunteerDatabase.Helper;

namespace VolunteerDatabase.Desktop.Pages
{
    public class IdentityPage
    {
        private static IdentityPage identitypage;
        public bool flag = true;
        
        public AppUserIdentityClaims Claims { get; private set; }

        public static IdentityPage GetInstance(AppUserIdentityClaims claims)
        {
            if(identitypage == null || identitypage.Claims == null)
            {
                identitypage = new IdentityPage(claims);
            }
            return identitypage;
        }

        public static IdentityPage GetInstance()
        {
            if(identitypage == null || identitypage.Claims == null)
            {
                IdentityError();
            }
            return identitypage;
        }

        public static void IdentityError()
        {
            if(identitypage!=null)
            identitypage.flag = false;
            ModernDialog.ShowMessage("用户非法，请重新登录","",MessageBoxButton.OK);
        }

        public IdentityPage(AppUserIdentityClaims claims)
        {
            Claims = claims;
        }
    }
}
