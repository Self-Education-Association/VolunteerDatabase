using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Entity
{
    public class AppRole : IRole
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public AppRoleEnum RoleEnum { get; set; }

        public virtual List<AppUser> Users { get; set; }
    }
}
