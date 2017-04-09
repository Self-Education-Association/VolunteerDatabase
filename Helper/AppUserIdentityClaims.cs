using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper
{
    public class AppUserIdentityClaims : IClaims<AppUser>
    {
        private AppUser _user;

        private IEnumerable<AppRoleEnum> _roles;

        public AppUser User => !IsAuthenticated ? null : _user;

        public AppUser Holder { get; private set; }

        public IEnumerable<AppRoleEnum> Roles
        {
            get
            {
                if (!IsAuthenticated) return new List<AppRoleEnum>();
                return _roles;
            }
        }

        public bool IsAuthenticated { get; private set; }

        public bool IsInRole(AppRoleEnum roleEnum)
        {
            return Roles.Contains(roleEnum);
        }

        internal static AppUserIdentityClaims Create(AppUser user, AppUser holder)
        {
            AppUserIdentityClaims claims;
            if (user == null)//user应该是owner的意思
            {
                claims = new AppUserIdentityClaims
                {
                    Holder = holder,
                    IsAuthenticated = false
                };
            }
            else
            {
                claims = new AppUserIdentityClaims
                {
                    _user = user,
                    Holder = holder,
                    _roles = from r in user.Roles select r.RoleEnum,
                    IsAuthenticated = true
                };
            }

            return claims;
        }

        private AppUserIdentityClaims() { }
    }
}
