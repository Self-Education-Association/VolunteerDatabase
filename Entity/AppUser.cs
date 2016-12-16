using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Entity
{
    public class AppUser : IdentityUser
    {
        public AppUserStatus Status { get; set; }

        public virtual Organization Organization { get; set; }
    }

    public enum AppUserStatus
    {
        Enabled,
        Disabled,
        Banned
    }
}
