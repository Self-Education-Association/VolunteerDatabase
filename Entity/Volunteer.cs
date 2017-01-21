using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Entity
{
    public class Volunteer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Mobile { get; set; }
        public string Room { get; set; }
        public string Email { get; set; }
        public int Score { get; set; }
        public virtual List<CreditRecord> Records { get; set; } 
        public virtual List<BlackListRecord> BlackRecords { get; set; }

    }


}
