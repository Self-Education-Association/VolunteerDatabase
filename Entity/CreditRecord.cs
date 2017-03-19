using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VolunteerDatabase.Entity
{
    public class CreditRecord
    {
        [Key]
        public Guid UID { get; set; }
        public Volunteer Participant { get; set; }
        public Project Project { get; set; }
        public Organization Organization { get; set; }
        public double Score { get; set; }
    }
}
