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
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        public List<Project> FindAuthorizedProjectByUser(AppUser user)
        {
            var Project = from o in database.Projects where o.Managers.Equals(user) select o;
            Project = Project.Where(o => o.Condition == ProjectCondition.Ongoing);
            List<Project> Projects = new List<Project>();
            foreach (var item in Project)
            {
                Projects.Add(item);
            }
            return Projects;
        }
        [AppAuthorize(AppRoleEnum.Administrator)]
        public Project FindProjectByProjectId(int ProjectId)
        {
            var Project = database.Projects.SingleOrDefault(r => r.Id == ProjectId);
            if (Project == null)
            {
                ProgressResult.Error("项目不存在");
            }
            return Project;
        }

        public ProgressResult CreatVolunteer(Project Pro,string Room,string Name,string email,int Phone)
        {
            ProgressResult result;
            if(Room==""|Name==""||email==""||Phone==0)
            {
                ProgressResult.Error("志愿者信息不完整");
            }
            lock(database)
            {
                Volunteer Vol = new Volunteer();
                Vol.Project.Add(Pro);
                Vol.Id = database.Volunteers.Count() + 1;
                Vol.Email = email;
                Vol.PhoneNum = Phone;
                Vol.Name = Name;
                Vol.Room = Room;
                Vol.Score = 0;
                //Vol.Records = null;
                Vol.BlackListRecords = null;
                database.Volunteers.Add(Vol);
                Save();
            }
            result = ProgressResult.Success();
            return result;
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        public List<Volunteer> FindSortedVolunteersByProject(Project project)
        {
            var volunteer = from o in database.Volunteers
                            where o.Project.Equals(project)
                            select o;
            List<Volunteer> Volunteers = new List<Volunteer>();
            foreach (var item in volunteer)
            {
                Volunteers.Add(item);
            }
            Volunteers.Sort();
            return Volunteers;
        }

        public Volunteer FindVolunteerById(int VolunteerId)
        {
            var volunteer = database.Volunteers.SingleOrDefault(r => r.Id == VolunteerId);
            if (volunteer == null)
            {
                ProgressResult.Error("志愿者不存在于数据库中");
            }
            return volunteer;
        }

        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        public ProgressResult SingleVolunteerInputById(int VolunteerId,Project Pro)
        {
            ProgressResult result;
            var Volunteer = database.Volunteers.SingleOrDefault(r => r.Id == VolunteerId);
            if (Volunteer == null)
            {
                return ProgressResult.Error("志愿者不存在于数据库中");
            }
            lock (database)
            {
                var Project = Pro;
                Project.Volunteer.Add(Volunteer);
                Save();
            }
            result = ProgressResult.Success();
            return result;
        }
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        public ProgressResult DeleteVolunteerFromProject(Volunteer Vol,Project Pro)
        {
            ProgressResult result;
            bool? IsInProject = Pro.Volunteer.Contains(Vol);
            if (IsInProject==null)
            {
                return ProgressResult.Error("志愿者不在该项目中");
            }
            lock (database)
            {
                Pro.Volunteer.Remove(Vol);
                Save();
            }
            result = ProgressResult.Success();
            return result;
        }
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        public ProgressResult Scoring4ForVolunteers(Project Pro)
        {
            ProgressResult result;
            var Volunteers = database.Volunteers.Where(o => o.Project.Contains(Pro));
            lock(database)
            {
                foreach(var item in Volunteers)
                {
                    item.Score += 4;
                }
                Pro.ScoreCondition = ProjectScoreCondition.Scored;
                Save();
            }
            result = ProgressResult.Success();
            return result;
        }
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        public ProgressResult ScoreSingleVolunteer(int Score,Volunteer Vol)
        {
            ProgressResult result;
            if(Score<1||Score>5)
            {
                ProgressResult.Error("分数超出合法范围");
            }
            Vol.Score = Vol.Score + Score - 4;
            Save();
            result = ProgressResult.Success();
            return result;
        }
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        public ProgressResult FinishProject(Project Pro)
        {
            ProgressResult result;
            if (Pro.Condition != ProjectCondition.Ongoing ||Pro.ScoreCondition==ProjectScoreCondition.Scored)
            {
                ProgressResult.Error("项目不满足结项条件，请检查项目状态和评分");
            }
            lock (database)
            {
                Pro.Condition = ProjectCondition.Finished;
                Save();
            }
            result = ProgressResult.Success();
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

        #region 错误信息处理
        public class ProgressResult
        {
            private string[] _errors;

            private bool _succeeded;

            public string[] Errors { get { return _errors; } }

            public bool Succeeded { get { return _succeeded; } }

            public static ProgressResult Error(params string[] errors)
            {
                if (errors.Count() == 0)
                {
                    errors = new string[] { "未提供错误信息。" };
                }
                var result = new ProgressResult
                {
                    _succeeded = false,
                    _errors = errors
                };
                return result;
            }

            public static ProgressResult Success()
            {
                var result = new ProgressResult
                {
                    _succeeded = true,
                    _errors = { }
                };
                return result;
            }

            private ProgressResult() { }
        }
    }
    #endregion
}
