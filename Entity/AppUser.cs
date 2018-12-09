using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Entity
{
    public class AppUser : IUser, IEqualityComparer<AppUser>

    {
        public int Id { get; set; }
        
        //public long StudentNum { get; set; }  //studentnum 可以空? 
        public string StudentNum { get; set; }

        public string AccountName { get; set; }

        public string Name { get; set; }

        public string Salt { get; set; }

        public string HashedPassword { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string Room { get; set; }

        public virtual List<AppRole> Roles { get; set; }

        public AppUserStatus Status { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual List<AppUser> Underlings { get; set; }//下属

        public virtual List<AppUser> Superiors { get; set; }

        public virtual List<Project> Projects { get; set; }//管理的项目

        public virtual List<BlackListRecord> BlackListRecords { get; set; }

        public virtual List<LogRecord> AddedLogRecords { get; set; }

        public virtual List<LogRecord> TargetedBy { get; set; }

        public bool AreSameWith(AppUser b)
        {
            if(this.Id==b.Id)
            {
                return true;
            }
            if(this.AccountName==b.AccountName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(AppUser x, AppUser y)
        {
            if (x.Id == y.Id)
            {
                return true;
            }
            else
            {
                return false;
            };
        }

        public int GetHashCode(AppUser obj)
        {
            return GetHashCode(obj);
        }
    }
}
