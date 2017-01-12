using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Interface
{
    public interface IRole
    {
        string Name { get; set; }

        AppRoleEnum RoleEnum { get; set; }
    }

    public enum AppRoleEnum
    {
        Administrator,
        OrgnizationAdministrator,
        OrgnizationMember,
        LogViewer,
        TestOnly
    }
}
