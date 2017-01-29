using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Entity
{
    public class Project 
    {
        public string Name { get; set; }

        public string Place { get; set; }

        public int Id { get; set; }

        public int Maximum { get; set; }

        public string Details { get; set; }

        public DateTime? Time { get; set; }

        public DateTime? CreatTime { get; set; }

        public ProjectCondition Condition { get; set; }

        public ProjectScoreCondition ScoreCondition { get; set; }

        public Organization Creater { get; set; }

        public virtual List<Volunteer> Volunteer{ get; set; }

        public virtual List<AppUser>   Managers { get; set; }
    }

}
