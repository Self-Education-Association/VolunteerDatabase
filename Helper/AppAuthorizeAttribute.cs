using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AppAuthorizeAttribute : Attribute
    {
        public AppAuthorizeAttribute(AppRoleEnum role = AppRoleEnum.Administrator)
        {
            Role = role;
        }//初始的role是administrator

        public AppRoleEnum Role { get; set; }//后面可以改role
    }
}
