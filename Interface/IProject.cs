using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Interface
{
    public interface IProject
    {

        string Name { get; set; }

        IUser Manager { get; set; }

        ProjectCondition Condition { get; set; }
    }

    public enum ProjectCondition
    {
        Ongoing,
        Finished
    }

    public enum ProjectScoreCondition
    {
        Scored,
        UnScored
    }
}
