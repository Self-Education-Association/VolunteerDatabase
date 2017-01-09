using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AppAuthorizeAttribute : Attribute
    {
        public AppRoleEnum Role { get; set; }
    }
}
