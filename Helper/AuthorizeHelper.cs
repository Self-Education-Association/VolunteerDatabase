using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper
{
    public class AuthorizeHelper<TData, TOut> where TData : class where TOut : class
    {
        public delegate TOut AuthorizeFunction(TData data);

        private AuthorizeHelper() { }

        public static TOut Execute(IAuthorizeInput<TData> input, AuthorizeFunction function, AppRoleEnum roleEnum)
        {
            if (!Authorize(input, function, roleEnum))
            {
                throw new NotAuthorizedException(input.Claims, roleEnum);
            }
            var output = function(input.Data);
            return output;
        }

        public static bool Authorize(IAuthorizeInput<TData> input, AuthorizeFunction function, AppRoleEnum roleEnum)
        {
            if (!input.Claims.IsAuthenticated)
            {
                return false;
            }
            if (input.Claims.IsInRole(AppRoleEnum.Administrator))
            {
                return true;
            }
            if (input.Claims.IsInRole(roleEnum))
            {
                return true;
            }
            if (typeof(IProject).IsAssignableFrom(input.Data.GetType()))
            {
                var project = input.Data as IProject;
                if (project.Manager.Name == input.Claims.User.Name)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class NotAuthorizedException : Exception
    {
        public AppRoleEnum RequiredRole { get; }

        public IClaims Claims { get; }

        public override string ToString()
        {
            var roles = from r in Claims.User.Roles select r.Name;
            string roleString = string.Join(",", roles);
            return string.Format("[{0}]使用令牌[{1}]授权失败，要求用户角色为[{2}]，令牌具有用户角色为[{3}]。\n", Claims.Holder.Name, Claims.User.Name, RequiredRole.ToString(), roleString) + base.ToString();
        }

        public NotAuthorizedException(IClaims claims, AppRoleEnum requiredRole)
        {
            Claims = claims;
            RequiredRole = requiredRole;
        }
    }
}
