using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;
using System.Data.Entity.Infrastructure;
using static VolunteerDatabase.Helper.ProjectProgress;

namespace VolunteerDatabase.Helper
{
    public class ProjectManage
    {
        private Database database;
        public List<Project> ShowProjectList(Organization org,bool OnGoing)//true时得到正在进行中的项目
        {
            var pro = from o in database.Projects
                      where o.Creater == org
                      select o;
            if (!OnGoing)
                pro = pro.Where(o => o.Condition == ProjectCondition.Finished);
            else
                pro = pro.Where(o => o.Condition == ProjectCondition.Ongoing);
            List < Project > Projects = new List<Project>();
            foreach(var item in pro)
            {
                Projects.Add(item);
            }
            return Projects;
        }

        public List<AppUser> FindManagerListById(params int[] Ids)
        {
            List<AppUser> Managers = new List<AppUser>();
            foreach (int Id in Ids)
            {
                var Manager = database.Users.SingleOrDefault(o=>o.Id==Id);
                if(Manager==null)
                {
                    ProgressResult.Error("用户不存在");
                }
                Managers.Add(Manager);
            }
            return Managers;
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ProgressResult CreatNewProject(Organization org, DateTime? Time=null,List<Volunteer> Volunteer=null , List<AppUser> Managers=null, string Name ="", string Place = "",string Detail="", int Maximum = 70)
        {
            ProgressResult result;
            lock (database)
            {
                var Project = new Project();
                Project.Time = Time;
                Project.Id = database.Projects.Count() + 1;
                Project.CreatTime = DateTime.Now;
                Project.Maximum = Maximum;
                Project.Managers = Managers;
                Project.Place = Place;
                Project.Name = Name;
                Project.Details = Detail;
                Project.Volunteer = Volunteer;
                Project.Condition = ProjectCondition.Ongoing;
                Project.ScoreCondition = ProjectScoreCondition.UnScored;
                Project.Creater = org;
                Project.BlackListRecords = null;
                database.Projects.Add(Project);
                Save();
            }
            result = ProgressResult.Success();
            return result;
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ProgressResult ProjectMessageInput(string Name, string Det,string Place, int Max,DateTime? Time, List<AppUser> Managers,Project Pro)
        {
            ProgressResult result;
            if (Name==""|| Det == "" || Place =="" ||Time== null || Managers == null)
            {
                ProgressResult.Error("信息不完整，无法输入信息");
            }
            Pro.Place = Place;
            Pro.Maximum = Max;
            Pro.Time = Time;
            Pro.Managers = Managers;
            Pro.Details = Det;
            Save();
            result = ProgressResult.Success();
            return result;
        }
        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ProgressResult ProjectDelete(Project Pro)
        {
            ProgressResult result;
            if(database.Projects.Contains(Pro)||Pro.Condition==ProjectCondition.Ongoing)
            {
                ProgressResult.Error("删除失败，项目不存在或已经结项");
            }
                lock(database)
                {
                    database.Projects.Remove(Pro);
                    Save();
                    result = ProgressResult.Success();
                }

            return result;
        }
        #region 封装好的Save方法
        private void Save()
        {
            bool flag = false;
            do
            {
                try
                {
                    database.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    flag = true;
                    foreach (var entity in database.ChangeTracker.Entries())
                    {
                        database.Entry(entity).Reload();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            } while (flag);
        }
        #endregion

    }

}
