using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Entity
{
    public class Organization
    {
        public string Name { get; set; }

        public virtual AppUser Administrator { get; set; }

        public virtual List<AppUser> Members { get; set; }
    }

    public enum OrganizationStatus
    {
        Enabled,
        Disabled
    }
}
