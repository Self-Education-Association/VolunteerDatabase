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
    public class ProjectManageHelper
    {
        Database database = DatabaseContext.GetInstance();
        private static ProjectManageHelper helper;
        private static readonly object helperlocker = new object();
        public static ProjectManageHelper GetInstance()
        {
            if (helper == null)
            {
                lock (helperlocker)
                {
                    if (helper == null)
                    {
                        helper = new ProjectManageHelper();
                    }
                }
            }
            return helper;
        }
        public async Task<ProjectManageHelper> GetInstanceAsync()
        {
            Task<ProjectManageHelper> helper = Task.Run(() =>
            {
                return GetInstance();
            });
            return await helper;
        }
        public ProjectManageHelper()
        {
            database = DatabaseContext.GetInstance();
        }
        public List<Project> ShowProjectList(Organization org,bool OnGoing)//true时得到正在进行中的项目
        {
            var pro = from o in database.Projects
                      where o.Organization.Id == org.Id
                      select o;
            if (OnGoing)
                pro = pro.Where(o => o.Condition == ProjectCondition.Ongoing);
            else
                pro = pro.Where(o => o.Condition == ProjectCondition.Finished);
            var Projects = pro.ToList();
            return Projects;
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        //      public List<AppUser> FindManagerListByStudentNum(params string[] StuNums)  //将原本是string类型的参数改为 int
        public List<AppUser> FindManagerListByStudentNum(params int[] StuNums)
        {
            List<AppUser> Managers = new List<AppUser>();
            foreach (int StuNum in StuNums)
            {
                var Manager = database.Users.SingleOrDefault(o=>o.StudentNum==StuNum);
                if(Manager==null)
                {
                    ProgressResult.Error("用户不存在");
                }
                else
                Managers.Add(Manager);
            }
            return Managers;
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ProgressResult CreatNewProject(Organization org, DateTime Time, string Name = "", string Place = "", string Detail = "", int Maximum = 70)
        {
            ProgressResult result;
            if (Time == null)
            {
                return ProgressResult.Error("新项目"+Name+"时间不合法，请重新输入");
            }
            lock (database)
            {
                var Project = new Project();
                Project.Time = Time;
                Project.CreatTime = DateTime.Now;
                Project.Maximum = Maximum;
                Project.Managers = null;
                Project.Place = Place;
                Project.Name = Name;
                Project.Details = Detail;
                Project.Condition = ProjectCondition.Ongoing;
                Project.ScoreCondition = ProjectScoreCondition.UnScored;
                Project.Organization = org;
                Project.Volunteers = null;
                database.Projects.Add(Project);
                Save();
                result = ProgressResult.Success();
                return result;
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ProgressResult ProjectMessageInput(string Name, string Detail,string Place, int Max,DateTime Time, List<AppUser> Managers,Project Pro)
        {
            ProgressResult result;
            if (Name==""|| Detail == "" || Place =="" ||Time== null || Managers == null)
            {
                ProgressResult.Error("信息不完整，无法输入信息");
            }
            Pro.Place = Place;
            Pro.Maximum = Max;
            Pro.Time = Time;
            Pro.Managers = Managers;
            Pro.Details = Detail;
            Save();
            result = ProgressResult.Success();
            return result;
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ProgressResult AddManager(AppUser Manager, Project Pro)
        {
            if(Pro==null||Pro.Condition==ProjectCondition.Finished)
            {
                return ProgressResult.Error("修改项目时失败!项目不存在或已结项.");
            }
            if(Manager == null||Manager.Status!=AppUserStatus.Enabled)
            {
                return ProgressResult.Error("待加入的用户身份非法.");
            }

            Pro.Managers.Add(Manager);
            Save();
            return ProgressResult.Success();
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public List<ProgressResult> AddManager(List<AppUser> managers,Project project)
        {
            List<ProgressResult> resultlist = new List<ProgressResult>();
            foreach (AppUser manager in managers)
            {
                resultlist.Add(AddManager(manager, project));
            }
            return resultlist;
        }
        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ProgressResult ProjectDelete(Project Pro)
        {
            ProgressResult result;
            if(database.Projects.Where( p => p.Id == Pro.Id).Count() == 0 ||Pro.Condition==ProjectCondition.Finished)//可以用contains?
            {
                ProgressResult.Error("删除失败，项目不存在或已经结项");
            }
            List<LogRecord> loglist = database.LogRecords.Where(l => l.TargetProject.Id == Pro.Id).ToList();
            List<BlackListRecord> blacklist = database.BlackListRecords.Where(b => b.Project.Id == Pro.Id).ToList();
            database.Projects.Remove(Pro);
            Save();
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
                        flag = false;
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
