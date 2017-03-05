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
    public class BasePage
    {
        private static BasePage basepage;
        
        public AppUserIdentityClaims Claims { get; private set; }

        public static BasePage GetInstance(AppUserIdentityClaims claims)
        {
            if(basepage == null)
            {
                basepage = new BasePage(claims);
            }
            return basepage;
        }

        public static BasePage GetInstance()
        {
            if(basepage == null)
            {
                IdentityError();
            }
            return basepage;
        }

        public static void IdentityError()
        {
            MessageBox.Show("用户非法，请重新登录");
        }

        public BasePage(AppUserIdentityClaims claims)
        {
            Claims = claims;
        }
    }
}
