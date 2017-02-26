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

        private AppUser _holder;

        private IEnumerable<AppRoleEnum> _roles;

        private bool _isAuthenticated;

        public AppUser User
        {
            get
            {
                if (!_isAuthenticated) return null;
                return _user;
            }
        }

        public AppUser Holder
        {
            get
            {
                return _holder;
            }
        }

        public IEnumerable<AppRoleEnum> Roles
        {
            get
            {
                if (!_isAuthenticated) return new List<AppRoleEnum>();
                return _roles;
            }
        }

        public bool IsAuthenticated { get { return _isAuthenticated; } }

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
                    _holder = holder,
                    _isAuthenticated = false
                };
            }
            else
            {
                claims = new AppUserIdentityClaims
                {
                    _user = user,
                    _holder = holder,
                    _roles = from r in user.Roles select r.RoleEnum,
                    _isAuthenticated = true
                };
            }

            return claims;
        }

        private AppUserIdentityClaims() { }
    }
}
