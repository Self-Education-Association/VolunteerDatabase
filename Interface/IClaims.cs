using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Interface
{
    public interface IClaims<T> where T : IUser
    {
        T User { get; }

        T Holder { get; }

        bool IsAuthenticated { get; }

        bool IsInRole(AppRoleEnum roleEnum);
    }
}
