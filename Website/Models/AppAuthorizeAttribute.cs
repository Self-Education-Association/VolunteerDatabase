using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Interface;

namespace Website.Models
{
    public class WebsiteAuthorizeAttribute : AuthorizeAttribute
    {
        public WebsiteAuthorizeAttribute(params AppRoleEnum[] roles)
        {
            Roles = roles;
        }

        public new AppRoleEnum[] Roles { get; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                var user = (AppUserIdentityClaims)httpContext.Session["Claim"];
                if (user?.IsAuthenticated != true)
                    return false;

                if (Roles.Length == 0)
                    return true;

                var userRoles = user.Roles;
                if (userRoles == null)
                    return false;
                foreach (var role in Roles)
                {
                    if (userRoles.Contains(role))
                        return true;
                }
                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }
    }
}