using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VolunteerDatabase.Entity
{
    public class BlackListRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UID { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime EndTime { get; set; }
        public BlackListRecordStatus Status { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual AppUser Adder { get; set; }
        public virtual Volunteer Volunteer { get; set; }
        public virtual Project Project { get; set; }

    }
    public enum BlackListRecordStatus
    {
        Disabled,
        Enabled
    }
}
