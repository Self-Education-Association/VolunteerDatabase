using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper
{
    public class AppUserAuthorizeInput<T> : IAppUserAuthorizeInput<T>
    {
        public IClaims<AppUser> Claims { get; private set; }

        public T Data { get; set; }

        public static AppUserAuthorizeInput<T> Create(IClaims<AppUser> claims, T data)
        {
            return new AppUserAuthorizeInput<T> { Claims = claims, Data = data };
        }
    }
}
