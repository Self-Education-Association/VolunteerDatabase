using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Helper;

namespace VolunteerDatabase.Desktop
{
    class AppIdentity : IIdentity
    {
        public string Name { get; }

        public string AuthenticationType { get; }

        public bool IsAuthenticated => string.IsNullOrEmpty(Name);

        public AppIdentity(AppUserIdentityClaims claims, string type)
        {
            Name = claims.User.Name;
            AuthenticationType = type;
        }
    }
}
