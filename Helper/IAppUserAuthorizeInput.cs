using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper
{
    public interface IAppUserAuthorizeInput<TData> : IAuthorizeInput<TData, AppUser> { }
}
