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
        public IEnumerable<AppRoleEnum> RequiredRoles { get; }

        public IClaims<AppUser> Claims { get; }

        public override string ToString()
        {
            string requiredRoleString = string.Join(", ", RequiredRoles);
            string roleString = "";
            if (Claims != null && Claims.User != null)
            {
                var roles = from r in Claims.User.Roles select r.Name;//在claims.user.roles挨个取r将r.name投影到新的枚举类
                roleString = string.Join(", ", roles);
            }
            return string.Format("[{0}]使用令牌[{1}]授权失败，要求用户角色为[{2}]，令牌具有用户角色为[{3}]。\n", Claims?.Holder?.AccountName, Claims?.User?.AccountName, requiredRoleString, roleString) + base.ToString();
        }

        public AppUserNotAuthorizedException(IClaims<AppUser> claims, AppRoleEnum requiredRole)
        {
            Claims = claims;
            RequiredRoles = new List<AppRoleEnum> { requiredRole };
        }

        public AppUserNotAuthorizedException(IClaims<AppUser> claims, IEnumerable<AppRoleEnum> requiredRoles)
        {
            Claims = claims;
            RequiredRoles = requiredRoles;
        }
    }
}
