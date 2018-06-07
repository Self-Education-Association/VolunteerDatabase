﻿using System;
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
        Database database = DatabaseContext.GetInstance();

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public static BlackListHelper GetInstance()
        {
            if (helper == null)
            {
                lock (helperlocker)
                {
                    if (helper == null)
                    {
                        helper = new BlackListHelper();
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
        public async Task<BlackListHelper> GetInstanceAsync()//异步同行，到await挂起直到其它方法完成
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

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public List<BlackListRecord> FindBlackList(Guid uid)//按照uid查找黑名单
        {
            try
            {
                var result = database.BlackListRecords.Where(b => b.UID == uid).ToList();
                return result;
            }
            catch (Exception)//错误
            {
                return null;
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public List<BlackListRecord> FindBlackList(Volunteer v)//给出变量志愿者，按照uid查找黑名单
        {
            try
            {
                var result = database.BlackListRecords.Where(b => b.Volunteer.UID == v.UID).ToList();
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
        public List<BlackListRecord> FindBlackList(Organization org)//按照组织id查找黑名单
        {
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

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public List<BlackListRecord> FindBlackList(AppUser adder)//按照添加者id查找黑名单
        {
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

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public List<BlackListRecord> FindBlackList(Project project)//按照项目id查找黑名单
        {
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

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public List<BlackListRecord> FindBlackListByAddTime(DateTime start, DateTime end)//按照添加时间查找黑名单
        {
            if (start > end)
            {
                return new List<BlackListRecord>();
            }
            var result = database.BlackListRecords.Where(b => b.AddTime < end && b.AddTime > start).ToList();
            return result;
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        [AppAuthorize(AppRoleEnum.LogViewer)]
        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        [AppAuthorize(AppRoleEnum.TestOnly)]
        public List<BlackListRecord> FindBlackListByEndTime(DateTime start, DateTime end)//按照结束时间查找黑名单
        {
            if (start > end)
            {
                return new List<BlackListRecord>();
            }
            var result = database.BlackListRecords.Where(b =>( b.EndTime <= end && b.EndTime >= start)).ToList();
            return result;
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]//必须是被授权管理某个project的
        public BlackListResult AddBlackListRecord(BlackListRecord brec)//根据黑名单记录查找项目
        {
            if (brec == null)
            {
                return BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.NullRecord);
            }
            IEnumerable<BlackListRecord> tempbrec = null;
            if (brec.Project != null)
            {
                tempbrec = database.BlackListRecords.Where(b => b.Project.Id == brec.Project.Id).ToList();
            // if (database.BlackListRecords.Where(b=>b.Project==brec.Project).Count()!=0)
            if ( tempbrec.Count() != 0)//查看是否有重复的黑名单
            {
                foreach (var eptempbrec in tempbrec)
                {
                    if (AreSame(eptempbrec, brec))
                    {
                        return BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.ExistingRecord);
                    }
                }
                //  return BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.ExistingRecord);
            }
            }
            if (brec.EndTime < System.DateTime.Now)//查看结束时间是否允许
            {
                return BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.WrongTime);
            }

            try
            {
                database.BlackListRecords.Add(brec);
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
        //[AppAuthorize(AppRoleEnum.OrgnizationMember)]//必须被授权为该项目管理员 => 拿到机构管理员的令牌
        public BlackListResult AddBlackListRecord(string Detail, Volunteer volunteer, AppUser adder, DateTime endtime, Organization orgnization = null, Project project = null, BlackListRecordStatus status = BlackListRecordStatus.Enabled)
        {
            if (endtime < System.DateTime.Now)
            {
                return BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.WrongTime);
            }
            BlackListRecord result = new BlackListRecord
            {
                Detail = Detail,
                Volunteer = volunteer,
                Project = project,
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
            catch (Exception e)
            {
                return BlackListResult.Error("出现错误，错误信息:{0}", e.Message);
            }
            return BlackListResult.Success();
        }
        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public BlackListResult EditBlackListRecord(string detail, Guid uid, DateTime endtime, BlackListRecordStatus status)
        {
            var record = database.BlackListRecords.SingleOrDefault(b => b.UID == uid);
            if (uid == null)
            {
                return BlackListResult.Error(BlackListResult.EditBlackListRecordErrorEnum.EmptyId);
            }
            if (database.BlackListRecords.Where(b => b.UID == uid).Count() == 0)
            {
                return BlackListResult.Error(BlackListResult.EditBlackListRecordErrorEnum.NoExistingRecord);
            }
            try
            {
                record.Detail = detail;
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
        public BlackListResult DeleteBlackListRecord(Guid uid)
        {//异常：id为空,找不到id对应的条目
            var record = database.BlackListRecords.Find(uid);
            if (record == null)
            {
                return BlackListResult.Error(BlackListResult.DeleteBlackListRecordErrorEnum.NonExistingRecord);
            }
            try
            {
                database.BlackListRecords.Remove(record);
                Save();
            }
            catch (Exception e)
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

        #region 比较字段
        public static bool AreSame(BlackListRecord a, BlackListRecord b)
        {
            if (a == null && b == null)
                return true;
            if ((a == null && b != null) || (a != null && b == null))
                return false;
            else if (a.Adder.AccountName == b.Adder.AccountName && a.EndTime == b.EndTime && a.Volunteer.StudentNum == b.Volunteer.StudentNum && a.Project.Id == b.Project.Id && a.Organization.Id == b.Organization.Id && a.Status == b.Status)
            {

                return true;

            }
            else
                return false;
        }
        #endregion

    }
}
