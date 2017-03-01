using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Interface
{
    public interface IUser
    {
        string AccountName { get; set; }

        AppUserStatus Status { get; set; }
    }

    public enum AppUserStatus
    {
        Enabled,
        NotApproved,
        Rejected,
        Disabled,
        Banned
    }
}
