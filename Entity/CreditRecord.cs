using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VolunteerDatabase.Entity
{
    public class CreditRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UID { get; set; }
        public Volunteer Participant { get; set; }
        
        public Project Project { get; set; }
        public Organization Organization { get { return Project.Organization; } } 
        public CreditScore Score { get; set; }

        public class CreditScore
        {
            public int PncScore { get; set; }
            public int SrvScore { get; set; }
            public int CmmScore { get; set; }
            public double AvgScore { get
                {
                    return Math.Round((PncScore + SrvScore + CmmScore) / 3.0, 2);
                } }//保留两位小数
            public int TotalScore { get { return PncScore + SrvScore + CmmScore; } }
        }
        
    }
}
