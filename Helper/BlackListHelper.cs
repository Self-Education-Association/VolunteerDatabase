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
    public class BlackListHelper
    {
        //黑名单，记录增删查改
        Database database = new Database();
        public BlackListHelper()
        {
            database = DatabaseContext.GetInstance();
        }
        public List<BlackListRecord> FindBlackList(int id) {
            //异常处理
            var result = database.BlackListRecords.Where(b => b.Id == id).ToList();
            return result;
        }
        public List<BlackListRecord> FindBlackList(Volunteer v) {
            //异常处理
            var result = database.BlackListRecords.Where(b => b.Volunteer.Id == v.Id).ToList();
            return result;
        }
        public List<BlackListRecord> FindBlackList(Organization org) {
            var result = database.BlackListRecords.Where(b => b.Organization.Id == org.Id).ToList();
            return result;
        }
        public List<BlackListRecord> FindBlackList(AppUser adder) {
            var result = database.BlackListRecords.Where(b => b.Adder.Id == adder.Id).ToList();
            return result;
        }
        public List<BlackListRecord> FindBlackList(Project project) {
            var result = database.BlackListRecords.Where(b => b.Project.Id == project.Id).ToList();
            return result;
        }
        public List<BlackListRecord> FindBlackListByAddTime(DateTime start,DateTime end)
        {
            if (start > end)
            {
                return null;
            }
            var result = database.BlackListRecords.Where(b => b.AddTime<end&&b.AddTime>start ).ToList();
            return result;
        }
        public List<BlackListRecord> FindBlackListByEndTime(DateTime start,DateTime end)
        {
            if (start > end)
            {
                return null;
            }
            var result = database.BlackListRecords.Where(b => b.EndTime < end && b.EndTime > start).ToList();
            return result;
        }
        public BlackListResult AddBlackListRecord(BlackListRecord brec)//id是如何生成的？
        {
            if(brec==null||brec.Id==0)
            {
                return BlackListResult.Error("欲添加的黑名单记录为空，请检查后重试！");
            }
            if(database.BlackListRecords.Where(b=>b.Id==brec.Id).Count()!=0)
            {
                return BlackListResult.Error("数据库中存在相同id的条目，请检查后重试！");
            }
            if (brec.EndTime < System.DateTime.Now)
            {
                return BlackListResult.Error("输入的结束时间已过，请检查后重试。");
            }

            try
            {
                database.BlackListRecords.Add(brec);
                Save();
            }
            catch(Exception e)
            {
                return BlackListResult.Error("出现错误，错误信息:{0}", e.Message);
            }
            return BlackListResult.Success();
        }
        public BlackListResult AddBlackListRecord(Volunteer volunteer, AppUser adder,DateTime endtime,Organization orgnization = null, Project project = null,BlackListRecordStatus status=BlackListRecordStatus.Enabled)
        {
            //异常处理：id未设置，或已有id重复的条目，或输入的结束时间在当前时间之前，必填字段为空或不是相应类型(这个在前端检查)

            if(endtime<System.DateTime.Now)
            {
                return BlackListResult.Error("输入的结束时间已过，请检查后重试。");
            }
            BlackListRecord result = new BlackListRecord
            {
                Volunteer = volunteer,
                Adder = adder,
                Status = status,
                Organization = orgnization,
                EndTime = endtime,
                AddTime = System.DateTime.Now
            };
            try
            {
                AddBlackListRecord(result);
            }
            catch(Exception e)
            {
                return BlackListResult.Error("出现错误，错误信息:{0}", e.Message);
            }
            return BlackListResult.Success();
        }
        public BlackListResult EditBlackListRecord(int id,DateTime endtime,BlackListRecordStatus status)
        {
            var record = database.BlackListRecords.SingleOrDefault(b => b.Id == id);
            if (id == 0)
            {
                return BlackListResult.Error("欲添加的黑名单记录为空，请检查后重试！");
            }
            if (database.BlackListRecords.Where(b => b.Id == id).Count() == 0)
            {
                return BlackListResult.Error("数据库中不存在该的条目，请检查后重试！");
            }
            try
            {
                record.EndTime = endtime;
                record.Status = status;
                Save();
            }
            catch (Exception e)
            {
                return BlackListResult.Error("出现错误，错误信息:{0}", e.Message);
            }
            return BlackListResult.Success();
        }
        public BlackListResult DeleteBlackListRecord(int id)
        {//异常：id为空,找不到id对应的条目
            var record = database.BlackListRecords.Find(id);
            if(record==null)
            {
                return BlackListResult.Error("删除失败，找不到该id对应的黑名单条目。");
            }
            try
            {
                database.BlackListRecords.Remove(record);
                Save();
            }
            catch(Exception e)
            {
                return BlackListResult.Error("出现错误，错误信息:{0}", e.Message);
            }
            return BlackListResult.Success();
        }
    

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
                    foreach(var entity in database.ChangeTracker.Entries())
                    {
                        database.Entry(entity).Reload();
                    }
                }
                catch(Exception)
                {
                    throw;
                }
                
            } while (flag);
        }
    }
}
