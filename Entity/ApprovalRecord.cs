using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Entity
{
    public class ApprovalRecord
    {
        public int Id { get; set; }
        public bool IsApproved { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public string Note { get; set; }
        public AppUser User { get; set; }//这个需要打开CascedeDelete
    }
}
