using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper
{
    [Serializable]
    public class AppUserNotAuthorizedException : Exception
    {
        public AppRoleEnum RequiredRole { get; }

        public IClaims<AppUser> Claims { get; }

        public override string ToString()
        {
            string roleString = "";
            if (Claims != null && Claims.User != null)
            {
                var roles = from r in Claims.User.Roles select r.Name;
                roleString = string.Join(",", roles);
            }
            return string.Format("[{0}]使用令牌[{1}]授权失败，要求用户角色为[{2}]，令牌具有用户角色为[{3}]。\n", Claims?.Holder?.AccountName, Claims?.User?.AccountName, RequiredRole.ToString(), roleString) + base.ToString();
        }

        public AppUserNotAuthorizedException(IClaims<AppUser> claims, AppRoleEnum requiredRole)
        {
            Claims = claims;
            RequiredRole = requiredRole;
        }
    }
}
