using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;

namespace VolunteerDatabase.Desktop
{
    class AppPrincipal : IPrincipal
    {
        private readonly Database _db = DatabaseContext.GetInstance();

        public IIdentity Identity { get; }

        public bool IsInRole(string role)
        {
            var user = _db.Users.FirstOrDefault(u => u.Name == Identity.Name);
            var flag = false;
            if (user?.Roles == null)
                return false;
            foreach (var r in user.Roles)
            {
                if (r.Name == role)
                    flag = true;
            }
            return flag;
        }

        public AppPrincipal(IIdentity identity)
        {
            Identity = identity;
        }
    }
}
