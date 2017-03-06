using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;
using System.Windows;
using System.Windows.Controls;

namespace Desktop.Pages
{
    public class IdentityPage
    {
        private static IdentityPage identitypage;
        
        public AppUserIdentityClaims Claims { get; private set; }

        public static IdentityPage GetInstance(AppUserIdentityClaims claims)
        {
            if(identitypage == null)
            {
                identitypage = new IdentityPage(claims);
            }
            return identitypage;
        }

        public static IdentityPage GetInstance()
        {
            return identitypage;
        }

        public static void IdentityError()
        {
            MessageBox.Show("用户非法，请重新登录");
        }

        public IdentityPage(AppUserIdentityClaims claims)
        {
            Claims = claims;
        }
    }
}
