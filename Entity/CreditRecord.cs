using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Entity
{
    public class CreditRecord
    {
        public Guid UID { get; set; }
        public Volunteer Participant { get; set; }
        public Project Project { get; set; }
        public Organization Organization { get; set; }
        public int Score { get; set; }
    }
}
