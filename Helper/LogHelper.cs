using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using System.Data.Entity.Infrastructure;
using System.Runtime.CompilerServices;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper
{
    /// <summary>
    ///     管理对于日志的撰写
    ///     请使用GetInstance方法获取单例工厂类
    ///     实例化前请先对Claims
    /// </summary>
    public class LogHelper
    {

        //“查看志愿者联系方式”时要提供上一次修改记录，需要一个（单独的）查询上一次修改的方法 x
        //添加日志方法：重载：基本字符串；分辨不同操作（使用调用者信息）；x
        //查询日志的方法：返回一个列表形式的集合（不一定是list，可能是datatable之类的类型）（25日更新：还是用List），查询字段有：日志添加时间，添加者，类型（*），对象（分为三个target） x
        //LogHelper 是 观察者

        //日志的增删查改，不允许删、改 x

        //异常处理完全没有写
        public static LogHelper helper;
        public static AppUserIdentityClaims Claims { get; set; }

        Database database;

        private static readonly object loghelperlocker = new object();


        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        /// <summary>
        /// 获得单例工厂类.
        /// </summary>
        /// <returns></returns>
        public static LogHelper GetInstance(AppUserIdentityClaims claims = null)//关于令牌：日志系统的使用令牌是必须的；事实上除了identityhelper和securityhelper不需要(?)之外，其余helper由于需要鉴权，都需要令牌
        {
            if (helper == null)
            {
                lock (loghelperlocker)
                {
                    if (helper == null)
                    {
                        helper = new LogHelper(claims);
                    }
                }
            }
            return helper;
        }

        public LogHelper(AppUserIdentityClaims claims)
        {
            Claims = claims;
            database = DatabaseContext.GetInstance();
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        /// <summary>
        /// 查询某个用户添加的所有日志.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<LogRecord> FindPrivateLogRecord(AppUser user)
        {
            try
            {
                var result = database.LogRecords.Where(r => r.Adder.Id == user.Id).ToList();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        /// <summary>
        /// 查询某个组织添加的所有日志.（公开）
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        public List<LogRecord> FindPublicLogRecord(Organization org)
        {
            try
            {
                var result = database.LogRecords.Where(r => r.Adder.Organization.OrganizationEnum == org.OrganizationEnum && r.IsPulblic == true).ToList();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///
        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        public List<LogRecord> FindPrivateLogRecord(Organization org)
        {
            try
            {
                var result = database.LogRecords.Where(r => r.Adder.Organization.OrganizationEnum == org.OrganizationEnum && r.IsPulblic == true).ToList();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        /// <summary>
        /// 查询某个时间区间内添加的所有公开日志.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<LogRecord> FindPublicLogRecord(DateTime start, DateTime end)
        {
            try
            {
                var result = database.LogRecords.Where(r => r.AddTime > start && r.AddTime < end && r.IsPulblic == true).ToList();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// *此方法仅限测试使用
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<LogRecord> FindPrivateLogRecord(DateTime start, DateTime end)
        {
            try
            {
                var result = database.LogRecords.Where(r => r.AddTime > start && r.AddTime < end).ToList();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        /// <summary>
        /// 查找某一个操作类型的日志.限测试使用.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<LogRecord> FindLogRecord(LogType type)//做一个operation<=>type的映射
        {
            try
            {
                var result = database.LogRecords.Where(r => r.Type == type).ToList();
                return result;
            }
            catch (Exception)
            {
                return null;
            }

        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        /// <summary>
        /// (注意：低效率的查询方法,限测试使用)
        /// 查询操作名(调用者方法名)为 operation 的所有日志.
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public List<LogRecord> FindLogRecord(string operation)
        {
            try
            {
                var result = database.LogRecords.Where(r => r.Operation == operation && r.IsPulblic == true).ToList();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public List<LogRecord> FindLogRecordByTargetUser(AppUser target)//
        {
            try
            {
                if (target == null)
                {
                    return null;
                }
                var result = database.LogRecords.Where(r => r.TargetAppUser.AreSameWith(target) && r.IsPulblic == true).ToList();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public List<LogRecord> FindLogRecordByTargetVolunteer(Volunteer target)
        {
            try
            {
                if (target == null)
                {
                    return new List<LogRecord>();
                }
                var result = database.LogRecords.Where(r => r.TargetVolunteer.UID == target.UID && r.IsPulblic == true).ToList();
                return result;
            }
            catch (Exception)
            {
                return new List<LogRecord>();
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public List<LogRecord> FindLogRecordByTargetProject(Project target)
        {
            try
            {
                if (target == null)
                {
                    return null;
                }
                var result = database.LogRecords.Where(r => r.TargetProject.AreSameWith(target) && r.IsPulblic == true).ToList();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public LogRecord FindLastContactEditLogRecord(Volunteer v)
        {
            try
            {
                var list = database.LogRecords.Where(r => r.Type == LogType.LastContactEdit && r.TargetVolunteer.AreSameWith(v) && r.IsPulblic == true).ToList();
                if (list.Count() == 0)
                {
                    return null;
                }
                LogRecord result = list.First();
                if (list.Count > 1)
                {
                    foreach (LogRecord item in list)
                    {
                        if (result.AddTime < item.AddTime)
                        {
                            result = item;
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        /// <summary>
        /// 查询某个管理者下属添加的日志.
        /// </summary>
        /// <param name="superior"></param>
        /// <returns></returns>
        public List<LogRecord> FindLogRecordBySuperior(AppUser superior)
        {
            List<AppUser> underlings = superior.Underlings;
            List<LogRecord> loglist = new List<LogRecord>();
            foreach (AppUser underling in underlings)
            {
                List<LogRecord> log = FindPrivateLogRecord(underling);
                if (log == null)
                {
                    continue;
                }
                else
                {
                    loglist.AddRange(log);
                }
            }
            return loglist;
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public bool Succeeded(string content, bool ispublic, LogType type = LogType.Default, AppUser targetuser = null, Volunteer targetvolunteer = null, Project targetproject = null, [CallerMemberName] string caller = "")
        {
            AppUser adder = database.Users.SingleOrDefault(u => u.AccountName == Claims.User.AccountName);
            if (caller == "")
                return false;
            string str = string.Format("操作:[{0}]成功.操作者:{1} " + content, caller, adder?.Name);
            return AddLogRecord(adder: adder, content: content, ispublic: ispublic, type: type, targetuser: targetuser, targetvolunteer: targetvolunteer, targetproject: targetproject, caller: caller);
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public bool Failed(string content, bool ispublic, LogType type, AppUser targetuser = null, Volunteer targetvolunteer = null, Project targetproject = null, [CallerMemberName] string caller = "")
        {
            AppUser adder = database.Users.SingleOrDefault(u => u.AccountName == Claims.User.AccountName);
            if (caller == "")
                return false;
            string str = string.Format("操作：[{0}]失败.操作者:{1} " + content, caller, adder.Name);
            return AddLogRecord(adder: adder, content: content, ispublic: ispublic, type: type, targetuser: targetuser, targetvolunteer: targetvolunteer, targetproject: targetproject, caller: caller);
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public bool AddLogRecord(AppUser adder, string content, bool ispublic, LogType type = LogType.Default, AppUser targetuser = null, Volunteer targetvolunteer = null, Project targetproject = null, [CallerMemberName] string caller = "")
        {
            if (caller == "")
                return false;
            try
            {
                LogRecord record = new LogRecord
                {
                    Type = type,
                    Adder = adder,//
                    AddTime = DateTime.Now,
                    Operation = caller,
                    TargetAppUser = targetuser,
                    TargetVolunteer = targetvolunteer,
                    TargetProject = targetproject,
                    LogContent = content,
                    IsPulblic = ispublic,
                };
                AddLogRecord(record);
                if (type == LogType.EditContact)
                {
                    var list = database.LogRecords.Where(l => l.Type == LogType.LastContactEdit && l.TargetVolunteer.AreSameWith(targetvolunteer)).ToList();
                    foreach (LogRecord item in list)
                    {
                        DeleteLogRecord(item);
                    }
                    record.Type = LogType.LastContactEdit;
                    AddLogRecord(record);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public void AddLogRecord(LogRecord record)
        {
            try
            {
                database.LogRecords.Add(record);
                Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public void DeleteLogRecord(LogRecord record)
        {
            try
            {
                database.LogRecords.Remove(record);
                Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

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
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            } while (flag);
        }
    }
}
