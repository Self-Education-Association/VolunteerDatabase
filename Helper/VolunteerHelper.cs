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
    public class VolunteerHelper : BaseHelper
    {

        //增删查改志愿者条目
        //查询某一志愿者

        private static VolunteerHelper helper;
        private static readonly object VolunteerLocker = new object();
        private static readonly object helperlocker = new object();
        private const string DEFAULTSTRING = "未填写";
        Database database;

        public static VolunteerHelper GetInstance(AppUserIdentityClaims claims = null)
        {
            if (helper == null)
            {
                lock (helperlocker)
                {
                    if (helper == null)
                    {

                        helper = new VolunteerHelper(claims);
                    }
                }
            }
            return helper;
        }
        
        /// <summary>
        /// 获取单例工厂类的异步方法，*未经测试*
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public async Task<VolunteerHelper> GetInstanceAsync(AppUserIdentityClaims claims)//不知道这个run()需不需要加入参数，未经测试
        {
            Task<VolunteerHelper> helper = Task.Run(() =>
            {
                return GetInstance(claims);
            });
            return await helper;
        }
        /// <summary>
        /// 构造最初单例的方法，在此处把日志的方法注册到事件上
        /// </summary>
        public VolunteerHelper(AppUserIdentityClaims claims)
        {
            Claims = claims;
            database = DatabaseContext.GetInstance();
            logger = LogHelper.GetInstance(Claims);
            Success += logger.Succeeded;
            Failure += logger.Failed;
        }
        
        public Volunteer FindVolunteer(int num)
        {
            var result = database.Volunteers.SingleOrDefault( v => v.StudentNum == num);
            return result;
        }
        public List<Volunteer> FindVolunteer(string name)
        {
            var result = database.Volunteers.Where(v => v.Name == name).ToList();//返回一个集合
            return result;
        }
        
        [AppAuthorize]
        public VolunteerResult AddVolunteer(Volunteer v)
        {
            if(v==null)
                return VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.NullVolunteerObject);
            if (v.StudentNum == 0)
                return VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.EmptyId);
            if (database.Volunteers.Where(o => o.StudentNum == v.StudentNum).Count() != 0)
                return VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.SameIdVolunteerExisted, v.StudentNum);
            else
            {
                int stunum = v.StudentNum;
                v.Mobile = v.Mobile == DEFAULTSTRING ? DEFAULTSTRING : v.Mobile;
                v.Room = v.Room == DEFAULTSTRING ? DEFAULTSTRING : v.Room;
                v.Name = v.Name == DEFAULTSTRING ? DEFAULTSTRING : v.Name;
                v.Class = v.Class == DEFAULTSTRING ? DEFAULTSTRING : v.Class;
                v.Email = v.Email == DEFAULTSTRING ? DEFAULTSTRING : v.Email;
                database.Volunteers.Add(v);
                Save();
                Volunteer target = FindVolunteer(stunum);
                bool logresult = VolunteerOperationSucceeded(string.Format("已添加学号:[{0}],姓名:[{1}] 的志愿者条目进入数据库.", target.StudentNum, target.Name), target, LogType.EditContact, true);
                return VolunteerResult.Success();
            }
        }
        [AppAuthorize]
        public VolunteerResult AddVolunteer(int num,string name=DEFAULTSTRING,string c=DEFAULTSTRING,string mobile=DEFAULTSTRING,string room=DEFAULTSTRING,string email=DEFAULTSTRING)
        {
            if(num==0)
                return VolunteerResult.Error(VolunteerResult.EditVolunteerErrorEnum.EmptyId);
            var v = new Volunteer
            {
                StudentNum = num,
                Name = name,
                Class = c,
                Mobile = mobile,
                Room = room,
                Email = email
            };
            return AddVolunteer(v);
        }
        //编辑志愿者
        [AppAuthorize]
        public VolunteerResult EditVolunteer(Volunteer a,Volunteer b)
        {
            if (a == null || a.StudentNum == 0)
                return VolunteerResult.Error(VolunteerResult.EditVolunteerErrorEnum.NonExistingVolunteer);
            var v = database.Volunteers.SingleOrDefault(o => o.StudentNum == a.StudentNum);
            v.StudentNum=b.StudentNum;
            v.Name = b.Name;
            v.Class = b.Class;
            v.Mobile = b.Mobile;
            v.Room = b.Room;
            v.Email = b.Email;
            Save();
            v = database.Volunteers.SingleOrDefault(o => o.StudentNum == a.StudentNum);
            Volunteer target = v;
            Volunteer edited = v;
            //Volunteer edited = FindVolunteer(b.StudentNum);
            if(edited.Mobile!=target.Mobile||edited.Email!=target.Email||edited.Room!=target.Room)//日志字符串改成“修改联系方式”，现在联系方式：手机 电子邮件 寝室
            {
                bool logresult = VolunteerOperationSucceeded(string.Format("修改原学号:{0},姓名:{1}的志愿者基本信息.现学号:{2},姓名:{3}", target.StudentNum, target.Name, edited.StudentNum, edited.Name), target, LogType.EditContact, true);
            }
            else
            {
                bool logresult = VolunteerOperationSucceeded(string.Format("修改原学号:{0},姓名:{1}的志愿者基本信息.现学号:{2},姓名:{3}", target.StudentNum, target.Name, edited.StudentNum, edited.Name), target, LogType.EditVolunteer, true);
            }
            return VolunteerResult.Success();
        }
        [AppAuthorize]
        public VolunteerResult EditVolunteer(Volunteer a, int num, string name = DEFAULTSTRING,string c= DEFAULTSTRING, string mobile = DEFAULTSTRING, string room = DEFAULTSTRING, string email = DEFAULTSTRING)
        {
            if (num == 0)
                return VolunteerResult.Error(VolunteerResult.EditVolunteerErrorEnum.EmptyId);
            var v = database.Volunteers.SingleOrDefault(o => o.StudentNum == a.StudentNum);
            if(v!=null)
            {
                v.StudentNum = num;
                v.Name = (name == DEFAULTSTRING) ? v.Name : name;
                v.Class = (c == DEFAULTSTRING) ? v.Class : c;
                v.Mobile = (mobile == DEFAULTSTRING) ? v.Mobile : mobile;
                v.Room = (room == DEFAULTSTRING) ? v.Room : room;
                v.Email = (email == DEFAULTSTRING )? v.Email : email;
                Save();
                Volunteer target = a;
                Volunteer edited = FindVolunteer(num);
                if (edited.Mobile != target.Mobile || edited.Email != target.Email || edited.Room != target.Room)
                {
                    bool logresult = VolunteerOperationSucceeded(string.Format("修改原学号:{0},姓名:{1} (现学号:{2},姓名:{3}) 的志愿者基本信息.", target.StudentNum, target.Name, edited.StudentNum, edited.Name), target, LogType.EditContact, true);
                }
                else
                {
                    bool logresult = VolunteerOperationSucceeded(string.Format("修改原学号:{0},姓名:{1} (现学号:{2},姓名:{3}) 的志愿者基本信息.", target.StudentNum, target.Name, edited.StudentNum, edited.Name), target, LogType.EditVolunteer, true);
                }
                return VolunteerResult.Success();
            }
            else
            {
                return VolunteerResult.Error(VolunteerResult.EditVolunteerErrorEnum.NonExistingVolunteer);
            }
        }
        //删除志愿者
        [AppAuthorize]
        public VolunteerResult DeleteVolunteer(Volunteer a)
        {
            if (a != null)
            {
                int deletedVolunteerStuNum = a.StudentNum;
                string deletedVolunteerName = a.Name;
                var volunteer = FindVolunteer(a.StudentNum);
                List<LogRecord> loglist = logger.FindLogRecordByTargetVolunteer(volunteer).ToList();
                List<BlackListRecord> blacklist = database.BlackListRecords.Where(b => b.Volunteer.UID == volunteer.UID).ToList();
                //foreach (var log in loglist)
                //{
                //    log.TargetVolunteer = null;
                //}
                //Save();//等待被注释
                database.Volunteers.Remove(volunteer);
                Save();
                bool logresult = VolunteerOperationSucceeded(string.Format("删除学号:{0},姓名:{1}的志愿者条目.", deletedVolunteerStuNum,deletedVolunteerName),null,LogType.DeleteVolunteer);
                return VolunteerResult.Success();
            }
            else
            {
                return VolunteerResult.Error(VolunteerResult.DeleteVolunteerErrorEnum.NonExistingVolunteer);
            }
        }
        [AppAuthorize]
        public VolunteerResult DeleteVolunteer(int num)
        {
            var v = database.Volunteers.Include("Project.TargetedBy").SingleOrDefault(o => o.StudentNum == num);
            return DeleteVolunteer(v);
            /*if (v != null)
            {
                int deletedVolunteerStuNum = v.StudentNum;
                string deletedVolunteerName = v.Name;
                database.Volunteers.Remove(v);
                Save();
                bool logresult = VolunteerOperationSucceeded(string.Format("删除学号:{0},姓名:{1}的志愿者条目.", deletedVolunteerStuNum,deletedVolunteerName), null, LogType.DeleteVolunteer);
                return VolunteerResult.Success();
            }
            else
            {
                return VolunteerResult.Error(VolunteerResult.DeleteVolunteerErrorEnum.NonExistingVolunteer);
            }*/
        }

        #region Save
        private void Save()
        {
            bool flag = false;
            do
            {
                try
                {
                    database.SaveChanges();
                    flag = false;
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
