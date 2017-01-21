using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;
using System.Data.Entity.Infrastructure;
namespace VolunteerDatabase.Helper
{
    public class ProjectProgress
    {
        private Database database;
        public Project FindAuthorizedProjectListById(AppUser Users)
        {
            var Project = database.Roles.SingleOrDefault(r => r.Users == Users);
            return Project;
        }

        public Project FindProjectByProjectId(int id)
        {
            var Project = database.Projects.SingleOrDefault(r => r.Id==id );
            if (Project == null)
            { return null; }
            else
            {
                return Project;
            }
        }

        public void ShowVolunteersByProjectId()
        {

        }
    
        public void FindVolunteerById()
        {

        }

        public void SingleVolunteerInput()
        {

        }

        public void MassiveVolunteerInput()
        {

        }

        public void DeleteVolunteerById()
        {

        }

    }
}
