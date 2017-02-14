﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Helper.Tests
{
    [TestClass()]
    public class BlackListHelperTests
    {
        Database database = new Database();
        BlackListHelper helper = BlackListHelper.GetInstance();
        #region 创建第一个Volunteer
        Volunteer TestVolunteer1 = new Volunteer
        {
            //Id = 000,
            Mobile = "1234567890-",
            Name = "TestVolunteer1"
        };
        #endregion
        #region 创建第二个Volunteer
        Volunteer TestVolunteer2 = new Volunteer
        {
            //Id = 001,
            Mobile = "-0987654321",
            Name = "TestVolunteer2"
        };
        #endregion
        #region 创建第一个Adder
        AppUser Testadder1 = new AppUser();
        #endregion
        #region 创建第二个Adder
        AppUser Testadder2 = new AppUser();
        #endregion
        #region 创建Organization
        Organization org = new Organization()
        {

            Name = "TestOnly",
            OrganizationEnum = OrganizationEnum.TestOnly
        };
        #endregion
        #region 创建一个Project
        Project pro = new Project();
        #endregion

        [TestMethod()]
        public void GetInstanceTest()
        {
            Assert.IsInstanceOfType(helper, typeof(BlackListHelper)); ;
        }

        [TestMethod()]
        public void AddBlackListTest()
        {
            #region 添加第一条黑名单记录
            BlackListRecord testaddrecord1 = new BlackListRecord
            {
                //Id = 1234567890,
                Volunteer = TestVolunteer1,
                Adder = Testadder1,
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2090/ 2 / 11),
                AddTime = System.DateTime.Now,
                Project = pro
            };
         /* var result = helper.AddBlackListRecord(testaddrecord1);
            if (result == BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.ExistingRecord))
            {
                var existedrecord = database.BlackListRecords.Find(testaddrecord1.UID);
                database.BlackListRecords.Remove(existedrecord);
                database.SaveChanges();
            } */
            helper.AddBlackListRecord(testaddrecord1);
               var actual1 = database.BlackListRecords.Find(testaddrecord1.UID);
            //Assert.AreEqual(result, actual1);
            //if (testaddrecord1 == actual1)
                Assert.IsTrue(testaddrecord1 == actual1, "添加第一条黑名单记录失败！");
            #endregion

            #region 添加第二条黑名单记录
            BlackListRecord testaddrecord2 = new BlackListRecord
            {
                //Id = 0987654321,
                Volunteer = TestVolunteer2,
                Adder = Testadder2,
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2090 / 2 / 10),
                AddTime = System.DateTime.Now,
                Project = pro
            };
            helper.AddBlackListRecord(testaddrecord2);
            var actual2 = database.BlackListRecords.Find(testaddrecord2.UID);
            Assert.IsTrue(testaddrecord2==actual2,"添加第二条黑名单记录失败！");
            #endregion

            #region   测试ExistingRecord
            BlackListRecord testaddrecord3 = new BlackListRecord
            {
                //Id = 0987654321
                Volunteer = TestVolunteer1,
                Adder = Testadder1,
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2090 / 2 / 11),
                AddTime = System.DateTime.Now,
                Project = pro
            };
            var existingrecordresult = helper.AddBlackListRecord(testaddrecord3);
            Assert.AreEqual(existingrecordresult, BlackListResult.AddBlackListRecordErrorEnum.ExistingRecord,"检验existingrecord失败！");
            #endregion

            #region 测试WrongTime
            BlackListRecord testaddrecord4 = new BlackListRecord
            {
                EndTime = new DateTime(2017 / 2 / 1),
                AddTime = System.DateTime.Now
            };
            var wrongtimeresult = helper.AddBlackListRecord(testaddrecord4);
            Assert.AreEqual(wrongtimeresult, BlackListResult.AddBlackListRecordErrorEnum.WrongTime,"测试wrongtime失败！");
            #endregion

            #region 测试Nullrecord
            BlackListRecord testaddrecord5 = new BlackListRecord();
            var nullrecordresult = helper.AddBlackListRecord(testaddrecord5);
            Assert.AreEqual(nullrecordresult, BlackListResult.AddBlackListRecordErrorEnum.NullRecord,"测试nullrecord失败！");
            #endregion
        }

        [TestMethod()]
        public void FindBlackListTest()
        {
            #region AddNewRecord
            BlackListRecord testaddrecord1 = new BlackListRecord
            {
               // Id = 1234567890,
                Volunteer = TestVolunteer1,
                Adder = Testadder1,
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2090 / 2 / 11),
                AddTime = System.DateTime.Now,
                Project = pro
            };
            var result = helper.AddBlackListRecord(testaddrecord1);
            if (result == BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.ExistingRecord))
            {
                var existedrecord = database.BlackListRecords.Find(testaddrecord1.UID);
                database.BlackListRecords.Remove(existedrecord);
                database.SaveChanges();
            }
            #endregion

            #region FindRecordById
            Guid testfindbyid = testaddrecord1.UID;
            Assert.IsNotNull(database.BlackListRecords.Find(testfindbyid),"无法通过id查找！");
            #endregion

            #region  FindRecordByVolunteer
            Volunteer testfindbyvolunteer1 = TestVolunteer1;
            Assert.IsNotNull(database.BlackListRecords.Find(testfindbyvolunteer1),"无法通过volunteer查找！");
            #endregion

            #region FindRecordByOrg
            Organization testfindbyorg = org;
            Assert.IsNotNull(database.BlackListRecords.Find(org),"无法通过org查找！");
            #endregion

            #region FindRecordByPro
            Project testfindbypro = pro;
            Assert.IsNotNull(database.BlackListRecords.Find(pro),"无法通过pro查找！");
            #endregion

            #region FindRecordByadder
            AppUser testfindbyadder = Testadder1;
            Assert.IsNotNull(database.BlackListRecords.Find(testfindbyadder),"无法通过adder查找！");
            #endregion

            #region FindRecordByAddTime
            DateTime testfindbyaddtime = System.DateTime.Now;
            Assert.IsNotNull(database.BlackListRecords.Find(testfindbyaddtime), "无法通过addtime查找！");
            #endregion

            #region FindRecordByEndTime
            Assert.IsNotNull(database.BlackListRecords.Find(System.DateTime.Now),"无法通过endtime查找");
            #endregion

            //是否为同一条记录
            Assert.AreSame(database.BlackListRecords.Find(testfindbyid), database.BlackListRecords.Find(testfindbyvolunteer1),"声明失败不是同一条记录！");

        }

        [TestMethod()]
        public void EditBlackListTest()
        {
            #region AddNewRecord
            BlackListRecord testaddrecord1 = new BlackListRecord
            {
                //Id = 1234567890,
                Volunteer = TestVolunteer1,
                Adder = Testadder1,
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2090 / 2 / 11),
                AddTime = System.DateTime.Now,
                Project = pro
            };
            var result = helper.AddBlackListRecord(testaddrecord1);
            if (result == BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.ExistingRecord))
            {
                var existedrecord = database.BlackListRecords.Find(testaddrecord1.UID);
                database.BlackListRecords.Remove(existedrecord);
                database.SaveChanges();
            }
            #endregion

            #region EmptyId
            //int tempid = testaddrecord1.Id--;
            var tempendtime = new DateTime(2090 / 2 / 11);
            BlackListRecord tempid = new BlackListRecord()
            {
                EndTime = tempendtime
            };
            if (database.BlackListRecords.Find(tempid) == null)
            { 
            Assert.IsNotNull(database.BlackListRecords.Find(tempendtime), "editemptyid 此条记录不在数据库中");
            Assert.AreEqual(helper.EditBlackListRecord(tempid.UID,tempendtime, BlackListRecordStatus.Enabled), BlackListResult.EditBlackListRecordErrorEnum.EmptyId);
             }
            else
            {
                Assert.Fail("Edit部分EmptyID已有记录！");
            }
            #endregion

            #region NoExistingRecord
            BlackListRecord testeditnoexistence = new BlackListRecord()
            {
                Volunteer = TestVolunteer1
            };
            var editresult = helper.EditBlackListRecord(testeditnoexistence.UID ,tempendtime, BlackListRecordStatus.Enabled);
            Assert.AreSame(editresult, BlackListResult.EditBlackListRecordErrorEnum.NoExistingRecord,"editnoexistingrecord");
            #endregion

            #region Editrecord
            var editorigin = database.BlackListRecords.Find(testaddrecord1.UID);
            var modifiedendtime = new DateTime(2090 / 1 / 11);
            helper.EditBlackListRecord(testaddrecord1.UID ,modifiedendtime, BlackListRecordStatus.Enabled);
            var editresult1 = database.BlackListRecords.Find(testaddrecord1.UID );
            Assert.AreNotSame(editresult1, editorigin);
            #endregion

    
        }

        [TestMethod()]
        public void DeleteBlackListTest()
        {
            #region addrecord
            BlackListRecord testdeleterecord = new BlackListRecord
            {
                //Id = 0987654321,
                Volunteer = TestVolunteer2,
                Adder = Testadder2,
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2090 / 2 / 10),
                AddTime = System.DateTime.Now,
                Project = pro
            };
            #endregion

            if (database.BlackListRecords.Find(testdeleterecord) == null)
            {
                helper.AddBlackListRecord(testdeleterecord);
            }
            else
            {
                testdeleterecord = database.BlackListRecords.Find(testdeleterecord);
            }
            helper.DeleteBlackListRecord(testdeleterecord.UID);
           var testdeleteresult =database.BlackListRecords.Find(testdeleterecord.UID);
            Assert.IsNull(testdeleterecord);
        }
    }
}