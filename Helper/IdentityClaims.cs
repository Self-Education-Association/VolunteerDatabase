using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper
{
    public class IdentityClaims : IClaims
    {
        private IUser _user;

        private IUser _holder;

        private IEnumerable<AppRoleEnum> _roles;

        private bool _isAuthenticated;

        public IUser User
        {
            get
            {
                if (!_isAuthenticated) return null;
                return _user;
            }
        }

        public IUser Holder
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

        internal static IdentityClaims Create(IUser user, IUser holder)
        {
            IdentityClaims claims;
            if (user == null || user.Roles == null)
            {
                claims = new IdentityClaims
                {
                    _holder = holder,
                    _isAuthenticated = false
                };
            }
            else
            {
                claims = new IdentityClaims
                {
                    _user = user,
                    _holder = holder,
                    _roles = from r in user.Roles select r.RoleEnum,
                    _isAuthenticated = true
                };
            }

            return claims;
        }

        private IdentityClaims() { }
    }
}
