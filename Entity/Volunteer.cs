using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VolunteerDatabase.Interface;

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
        public virtual List<BlackListRecord> BlackListRecords { get; set; }
        public virtual List<LogRecord> TargetedBy { get; set; }
        public int Score { get; set; }//以后改成double
        public bool AreSameWith(Volunteer b)
        {
            if (b == null)
                return false;
            if (b.UID == this.UID)
            {
                return true;
            }
            else
                return false;
        }
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

        public int CompareTo(Volunteer obj)//Volunteer里存double平均分，加个CreditRecord来存单次的分数
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
