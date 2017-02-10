using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Entity
{
    public class Volunteer:IComparable<Volunteer>
    {
        public int Id { get; set;}

        public string Mobile { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Room { get; set; }

        public string Class { get; set; }

        public virtual List<Project> Project{ get; set; }

        //public virtual List<CreditRecord> Records { get; set; }

        public virtual List<BlackListRecord> BlackListRecords { get; set; }

        public int Score { get; set; }

        public int CompareTo(Volunteer obj)
        {
            int result;
            if((this.Score/this.Project.Count)==(obj.Score/obj.Project.Count))
            {
                result = 0;
            }
            else
            if((this.Score/this.Project.Count)>(obj.Score/obj.Project.Count))
            {
                result = -1;
            }
            else
            {
                result = 1;
            }
            return result;
        }
    }
}
