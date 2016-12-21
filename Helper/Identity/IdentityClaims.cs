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

        private AppRoleEnum _role;

        private bool _isAuthenticated;

        public AppUser User
        {
            get
            {
                if (!_isAuthenticated) return null;
                return _user;
            }
        }

        public AppRoleEnum? Role
        {
            get
            {
                if (!_isAuthenticated) return null;
                return _role;
            }
        }

        public bool IsAuthenticated { get { return _isAuthenticated; } }

        protected IdentityClaims Success(AppUser user)
        {
            IdentityClaims claims;
            if (user == null || user.Role == null)
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
                    _role = user.Role.RoleEnum,
                    _isAuthenticated = true
                };
            }

            return claims;
        }

        private IdentityClaims() { }
    }
}
