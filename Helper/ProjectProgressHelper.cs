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
    public class ProjectProgressHelper
    {
        private static ProjectProgressHelper helper;
        private static readonly object helperlocker = new object();
        Database database = DatabaseContext.GetInstance();

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public static ProjectProgressHelper GetInstance()
        {
            if (helper == null)
            {
                lock (helperlocker)
                {
                    if (helper == null)
                    {
                        helper = new ProjectProgressHelper();
                    }
                }
            }
            return helper;
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public async Task<ProjectProgressHelper> GetInstanceAsync()
        {
            Task<ProjectProgressHelper> helper = Task.Run(() =>
            {
                return GetInstance();
            });
            return await helper;
        }
        public ProjectProgressHelper()
        {
            database = DatabaseContext.GetInstance();
        }

        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        public List<Project> FindAuthorizedProjectsByUser(AppUser user)
        {
            var Project=database.Projects.Where(t=>t.Managers.Contains(user)); 
            var Projects = Project.Where(o => o.Condition == ProjectCondition.Ongoing).ToList();
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

      /*  public ProgressResult CreatVolunteer(Project Pro, int num, string Room, string Name, string email, string Phone)
        {
            ProgressResult result;
            if (Room == "" || Name == "" || email == "" || Phone == "")
            {
                ProgressResult.Error("志愿者信息不完整");
            }
            lock (database)
            {
                Volunteer Vol = new Volunteer();
                Vol.Project.Add(Pro);   //这句话 System.nullReferenceException
              //  Vol.Project = Pro ;
                Vol.StudentNum = num;
                Vol.Email = email;
                Vol.Mobile = Phone;
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
        }*/ 
        
            //需要一个将volunteer 添加至 project 的方法；

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        public List<Volunteer> FindSortedVolunteersByProject(Project project)
        {
            var volunteer = from o in database.Volunteers
                            where o.Project.Contains(project)
                            select o;
            List<Volunteer> Volunteers = new List<Volunteer>();
            foreach (var item in volunteer)
            {
                Volunteers.Add(item);
            }
            Volunteers.Sort(); //根据Score平均值降序排列
            return Volunteers;
        }

        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ProgressResult SingleVolunteerInputById(int num, Project Pro)
        {
            ProgressResult result;
            var Volunteer = database.Volunteers.SingleOrDefault(r => r.StudentNum == num);
            if (Volunteer == null)
            {
                return ProgressResult.Error("志愿者不存在于数据库中");
            }
            int volunteerscount = Pro.Volunteers.Count;
            if(Pro.Maximum <= volunteerscount)
            {
                return ProgressResult.Error("已达项目人数上限，添加失败");
            }
            lock (database)
            {
                var Project = Pro;
                Project.Volunteers.Add(Volunteer);
                Save();
            }
            result = ProgressResult.Success();
            return result;
        }

        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ProgressResult DeleteVolunteerFromProject(Volunteer Vol, Project Pro)
        {
            ProgressResult result;
            if (!Pro.Volunteers.Contains(Vol))
            {
                return ProgressResult.Error("志愿者不在该项目中");
            }
            else
            lock (database)
            {
                Pro.Volunteers.Remove(Vol);
                Save();
            }
            result = ProgressResult.Success();
            return result;
        }

        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ProgressResult Scoring4ForVolunteers(Project Pro)
        {
            ProgressResult result;
            var Volunteers = database.Volunteers.Where(o => o.Project.Contains(Pro));     //找不到怎么办？异常处理？？还是返回空？
            lock (database)
            {
                foreach (var item in Volunteers)
                {
                    item.Score += 4;
                }
                Pro.ScoreCondition = ProjectScoreCondition.Scored;
                Save();
            }
            result = ProgressResult.Success();
            return result;
        }

        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ProgressResult ScoreSingleVolunteer(int Score, Volunteer Vol)
        {
            ProgressResult result;
            if (Score < 1 || Score > 5)
            {
              return  ProgressResult.Error("分数超出合法范围");
            }
            Vol.Score = Vol.Score + Score - 4;
            Save();
            result = ProgressResult.Success();
            return result;
        }

        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ProgressResult FinishProject(Project Pro)
        {
            ProgressResult result;
            if (Pro.Condition == ProjectCondition.Finished || Pro.ScoreCondition == ProjectScoreCondition.UnScored)
            {
               return  ProgressResult.Error("项目不满足结项条件，请检查项目状态和评分");
            }
            else
            lock (database)
            {
                Pro.Condition = ProjectCondition.Finished;
                Save();
            }
            result = ProgressResult.Success();
            return result;
        }

        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ProgressResult EditScore(Volunteer volunteer,Project project,int score)
        {
            CreditRecord crecord = database.CreditRecords.SingleOrDefault(r => r.Participant.UID == volunteer.UID && r.Project.Id == project.Id);
            if(crecord == null)
            {
                return ProgressResult.Error("不存在对应的征信记录.");
            }
            else
            {
                try
                {
                    volunteer.Score -= crecord.Score;
                    volunteer.Score += score;
                    crecord.Score = score;
                    return ProgressResult.Success();
                }
                catch(Exception)
                {
                    throw;
                }
            }
        }

        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ProgressResult DeleteCreditRecord(CreditRecord crecord)
        {
            if(crecord == null)
            {
                return ProgressResult.Error("不存在对应的征信记录.");
            }
            else
            {
                try
                {
                    database.CreditRecords.Remove(crecord);//多对多的中间表，应该可以直接删除.
                    return ProgressResult.Success();
                }
                catch(Exception)
                {
                    throw;
                }
            }

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
