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
    public class Volunteer:IComparable<Volunteer>, IEqualityComparer<Volunteer>
    {
        //[Required]
        //public long StudentNum { get; set; }
        public string StudentNum { get; set;}

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UID { get; set; }//唯一标识符：主键
        //[Required]
        public string Mobile { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Room { get; set; }

        public string Class { get; set; }

        public string Skill { get; set; }

        public string BlaclistCondition {
                            get {
                            if (BlackListRecords!=null&&BlackListRecords.Exists(o => o.Status == BlackListRecordStatus.Enabled))
                                            return "正在黑名单中";
                                            else return "当前无黑名单"; }
                                }

        public virtual List<Project> Project{ get; set; }
        public virtual List<BlackListRecord> BlackListRecords { get; set; }
        public virtual List<CreditRecord> CreditRecords { get; set; }
        public virtual List<LogRecord> TargetedBy { get; set; }
        public int ProjectCount { get; set; }
        public double AvgScore
        {
            get
            {
                double sum = 0;
                foreach (CreditRecord record in CreditRecords)
                {
                    sum += record.Score.AvgScore;
                }
                return Math.Round(sum / (CreditRecords.Count == 0 ? 1 : CreditRecords.Count),2);
            }
        }
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
            if(this.AvgScore==obj.AvgScore)
            {
                result = 0;
            }
            else
            if(this.AvgScore>obj.AvgScore)
            {
                result = -1;
            }
            else
            {
                result = 1;
            }
            return result;
        }

        public bool Equals(Volunteer x, Volunteer y)
        {
            if (x.UID == y.UID)
            {
                return true;
            }
            else return false; ;
        }

        public int GetHashCode(Volunteer obj)
        {
            return GetHashCode(obj);
        }




        
    }
}
