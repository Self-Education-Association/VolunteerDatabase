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
            {//检索应用于类型的成员的自定义特性的数组
                var attributes = Attribute.GetCustomAttributes(function.GetMethodInfo(), typeof(AppAuthorizeAttribute));
                IEnumerable<AppRoleEnum> requiredRoles = from AppAuthorizeAttribute a in attributes select a.Role;//在attributes里用a将a.role投影到新枚举类
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
                requiredRoles = from AppAuthorizeAttribute a in attributes select a.Role;//在attributes里用a将a.role投影到新表单
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
            if (typeof(Project).IsAssignableFrom(input.Data.GetType()))//考察input.Data.GetType()是否可以分配给project的实例
            {
                var project = input.Data as Project;
                bool flag = false;
                List<AppUser> managerlist = project.Managers;
                foreach (AppUser manager in managerlist)
                {
                    if (manager.AccountName == input.Claims.User.AccountName)//如果管理者项目姓名等于输入的用户姓名，返回真
                        flag = true;
                }
                return flag;
            }
            return false;
        }
    }
}
