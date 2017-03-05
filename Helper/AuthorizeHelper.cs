using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper
{
    public class AuthorizeHelper<TData, TOut>
    {
        public delegate TOut AuthorizeFunction(TData data);

        private AuthorizeHelper() { }

        public static TOut Execute(IAuthorizeInput<TData, AppUser> input, AuthorizeFunction function)
        {
            if (!Authorize(input, function))
            {
                var attributes = Attribute.GetCustomAttributes(function.GetMethodInfo(), typeof(AppAuthorizeAttribute));
                IEnumerable<AppRoleEnum> requiredRoles = from AppAuthorizeAttribute a in attributes select a.Role;
                throw new AppUserNotAuthorizedException(input.Claims, requiredRoles);
            }
            var output = function(input.Data);
            return output;
        }

        public static bool Authorize(IAuthorizeInput<TData, AppUser> input, AuthorizeFunction function)
        {
            var attributes = Attribute.GetCustomAttributes(function.GetMethodInfo(), typeof(AppAuthorizeAttribute));
            IEnumerable<AppRoleEnum> requiredRoles;
            if (attributes == null || attributes.Count() == 0)
            {
                requiredRoles = new List<AppRoleEnum> { AppRoleEnum.Administrator };
            }
            else
            {
                requiredRoles = from AppAuthorizeAttribute a in attributes select a.Role;
            }
            if (input == null || input.Claims == null)
            {
                return false;
            }
            if (!input.Claims.IsAuthenticated)
            {
                return false;
            }
            if (input.Claims.IsInRole(AppRoleEnum.Administrator))
            {
                return true;
            }
            foreach(var role in requiredRoles)
            {
                if (input.Claims.IsInRole(role))
                {
                    return true;
                }
            }
            if (typeof(Project).IsAssignableFrom(input.Data.GetType()))
            {
                var project = input.Data as Project;
                bool flag = false;
                List<AppUser> managerlist = project.Managers;
                foreach (AppUser manager in managerlist)
                {
                    if (manager.AccountName == input.Claims.User.AccountName)
                        flag = true;
                }
                return flag;
            }
            return false;
        }
    }
}
