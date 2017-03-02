using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Entity
{
    public class Project : IEqualityComparer<Project>
    {
        public string Name { get; set; }

        public string Place { get; set; }

        public int Id { get; set; }

        public int Maximum { get; set; }

        public string Details { get; set; }

        public DateTime Time { get; set; }

        public DateTime? CreatTime { get; set; }

        public ProjectCondition Condition { get; set; }

        public ProjectScoreCondition ScoreCondition { get; set; }

        public Organization Creater { get; set; }

        public virtual List<Volunteer> Volunteers{ get; set; }

        public virtual List<AppUser>   Managers { get; set; }

        //public virtual List<BlackListRecord> BlackListRecords { get; set; } 因为黑名单来自于volunteers，所以Project里不需要加入黑名单列表
        public virtual List<LogRecord> TargetedBy { get; set; }

        public bool AreSameWith(Project b)
        {
            if(b==null)
            {
                return false;
            }
            if(b.Id==Id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Project x, Project y)
        {
            if (x.Id == y.Id)
            {
                return true;
            }
            else return false;
        }

        public int GetHashCode(Project obj)
        {
           return GetHashCode(obj);
        }
    }

}
