using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VolunteerDatabase.Entity
{
    public class Volunteer:IComparable<Volunteer>
    {
        //[Required]
        public int StudentNum { get; set;}//学号

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UID { get; set; }//唯一标识符：主键
        //[Required]
        public string Mobile { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Room { get; set; }

        public string Class { get; set; }

        public virtual List<Project> Project{ get; set; }

        //public virtual List<CreditRecord> Records { get; set; }

        public virtual List<BlackListRecord> BlackListRecords { get; set; }

        public int Score { get; set; }
        public static bool AreSame(Volunteer a,Volunteer b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            if ((a == null && b != null) || (a != null && b == null))
            {
                return false;
            }
            else if (a.StudentNum == b.StudentNum &&
                a.Name == b.Name &&
                a.Mobile == b.Mobile &&
                a.Email == b.Email &&
                a.Class == b.Class &&
                a.Room == b.Room
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /*public static bool operator == (Volunteer a,Volunteer b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            if ((a == null && b != null) || (a != null && b == null))
            {
                return false;
            }
            else if (a.Id == b.Id &&
                a.Name == b.Name &&
                a.Mobile == b.Mobile &&
                a.Email == b.Email &&
                a.Class == b.Class &&
                a.Room == b.Room
                )
            {
                return true;
            }
            else
            { 
                return false;
            }
        }
        public static bool operator !=(Volunteer a,Volunteer b)
        {
            if (a == null && b == null)
            {
                return false;
            }
            if ((a == null && b != null) || (a != null && b == null))
            {
                return true;
            }
            else if (a.Id == b.Id &&
                a.Name == b.Name &&
                a.Mobile == b.Mobile &&
                a.Email == b.Email &&
                a.Class == b.Class &&
                a.Room == b.Room
                )
            {
                return false;
            }
            else
            {
                return true;
            }
        }*/
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
