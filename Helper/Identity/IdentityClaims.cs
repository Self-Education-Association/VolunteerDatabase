using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;

namespace VolunteerDatabase.Helper
{
    public class IdentityClaims
    {
        private AppUser _user;

        private List<AppRoleEnum> _roles;

        private bool _isAuthenticated;

        public AppUser User
        {
            get
            {
                if (!_isAuthenticated) return null;
                return _user;
            }
        }

        public List<AppRoleEnum> Roles
        {
            get
            {
                if (!_isAuthenticated) return new List<AppRoleEnum>();
                return _roles;
            }
        }

        public bool IsAuthenticated { get { return _isAuthenticated; } }

        internal static IdentityClaims Create(AppUser user)
        {
            IdentityClaims claims;
            if (user == null || user.Roles == null)
            {
                claims = new IdentityClaims
                {
                    _isAuthenticated = false
                };
            }
            else
            {
                claims = new IdentityClaims
                {
                    _user = user,
                    _roles = (from r in user.Roles select r.RoleEnum).ToList(),
                    _isAuthenticated = true
                };
            }

            return claims;
        }

        private IdentityClaims() { }
    }
}
