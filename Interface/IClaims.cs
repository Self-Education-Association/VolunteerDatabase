using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Interface
{
    public interface IClaims
    {
        IUser User { get; }

        IUser Holder { get; }

        bool IsAuthenticated { get; }

        bool IsInRole(AppRoleEnum roleEnum);
    }
}
