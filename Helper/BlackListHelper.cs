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
        private static BlackListHelper helper;
        private static readonly object helperlocker = new object();
        Database database = new Database();
        public static BlackListHelper GetInstance()
        {
            if(helper == null)
            {
                lock(helperlocker)
                {
                    if(helper == null)
                    {
                        helper = new BlackListHelper();
                    }
                }
            }
            return helper;
        }
        public async Task<BlackListHelper> GetInstanceAsync()
        {
            Task<BlackListHelper> helper = Task.Run(() =>
            {
                return GetInstance();
            });
            return await helper;
        }
        public BlackListHelper()
        {
            database = DatabaseContext.GetInstance();
        }
        public List<BlackListRecord> FindBlackList(int id) {
            try
            {
                var result = database.BlackListRecords.Where(b => b.Id == id).ToList();
                return result;
            }
            catch(Exception)
            {
               return null;
            }
        }
        public List<BlackListRecord> FindBlackList(Volunteer v) {
            //异常处理
            try
            {
                var result = database.BlackListRecords.Where(b => b.Volunteer.Id == v.Id).ToList();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<BlackListRecord> FindBlackList(Organization org) {
            try
            {
                var result = database.BlackListRecords.Where(b => b.Organization.Id == org.Id).ToList();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<BlackListRecord> FindBlackList(AppUser adder) {
            try
            {
                var result = database.BlackListRecords.Where(b => b.Adder.Id == adder.Id).ToList();
                return result;
             }
            catch (Exception)
            {
                return null;
            }
        }
        public List<BlackListRecord> FindBlackList(Project project) {
            try
            {
                var result = database.BlackListRecords.Where(b => b.Project.Id == project.Id).ToList();
                return result;
            }
            catch (Exception)
            {
                return null;
            }   
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
        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public BlackListResult AddBlackListRecord(BlackListRecord brec)//id是如何生成的？
        {
            if(brec==null||brec.Id==0)
            {
                return BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.NullRecord);
            }
            if(database.BlackListRecords.Where(b=>b.Id==brec.Id).Count()!=0)
            {
                return BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.ExistingRecord);
            }
            if (brec.EndTime < System.DateTime.Now)
            {
                return BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.WrongTime);
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
        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public BlackListResult AddBlackListRecord(Volunteer volunteer, AppUser adder,DateTime endtime,Organization orgnization = null, Project project = null,BlackListRecordStatus status=BlackListRecordStatus.Enabled)
        {
            if(endtime<System.DateTime.Now)
            {
                return BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.WrongTime);
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
        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public BlackListResult EditBlackListRecord(int id,DateTime endtime,BlackListRecordStatus status)
        {
            var record = database.BlackListRecords.SingleOrDefault(b => b.Id == id);
            if (id == 0)
            {
                return BlackListResult.Error(BlackListResult.EditBlackListRecordErrorEnum.EmptyId);
            }
            if (database.BlackListRecords.Where(b => b.Id == id).Count() == 0)
            {
                return BlackListResult.Error(BlackListResult.EditBlackListRecordErrorEnum.NoExistingRecord);
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
        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public BlackListResult DeleteBlackListRecord(int id)
        {//异常：id为空,找不到id对应的条目
            var record = database.BlackListRecords.Find(id);
            if(record==null)
            {
                return BlackListResult.Error(BlackListResult.DeleteBlackListRecordErrorEnum.NonExistingRecord);
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
                    flag = false;
                }
                catch (DbUpdateConcurrencyException)
                {
                    flag = true;
                    foreach(var entity in database.ChangeTracker.Entries())
                    {
                        database.Entry(entity).Reload();
                        flag = false;
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
