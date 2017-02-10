using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Id = 001,
            Mobile = "1234567890-",
            Name = "TestVolunteer1"
        };
        #endregion
        #region 创建第二个Volunteer【3位Id】
        Volunteer TestVolunteer2 = new Volunteer
        {
            Id = 002,
            Mobile = "-0987654321",
            Name = "TestVolunteer2"
        };
        #endregion
        #region 创建第一个Adder
        AppUser Testadder1 = new AppUser()
        {
            Id = 12345678

        };
        #endregion
        #region 创建第二个Adder【9位Id】
        AppUser Testadder2 = new AppUser()
        {
            Id = 12345679

        };
        #endregion
        #region 创建Organization
        Organization org = new Organization()
        {
            Id = 1234,
            Name = "TestOnly",
            OrganizationEnum = OrganizationEnum.TestOnly
        };
        #endregion
        #region 创建一个Project【4位Id】
        Project pro = new Project
        {
            Id = 1234
        };
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
            BlackListRecord result1 = new BlackListRecord
            {
                Id = 1234567890,
                Volunteer = TestVolunteer1,
                Adder = Testadder1,
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2017 / 2 / 11),
                AddTime = System.DateTime.Now,
                Project = pro
            };
                helper.AddBlackListRecord(result1);
            #endregion

            #region 添加第二条黑名单记录
            BlackListRecord result2 = new BlackListRecord
            {
                Id = 0987654321,
                Volunteer = TestVolunteer2,
                Adder = Testadder2,
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2017 / 2 / 10),
                AddTime = System.DateTime.Now,
                Project = pro
            };
            helper.AddBlackListRecord(result1);
            #endregion

            #region   测试ExistingRecord
            BlackListRecord result3 = new BlackListRecord
            {
                Id = 0987654321
            };
            helper.AddBlackListRecord(result1);
            #endregion

            #region 测试WrongTime
            BlackListRecord result4 = new BlackListRecord
            {
                EndTime = new DateTime(2017 / 2 / 1),
                AddTime = System.DateTime.Now
            };
            helper.AddBlackListRecord(result1);
            #endregion

            #region 测试Nullrecord
            BlackListRecord result5 = new BlackListRecord
            {
                Id = 0
            };
            helper.AddBlackListRecord(result1);
            #endregion
        }

        [TestMethod()]
        public void FindBlackListTest()
        {
            #region FindRecordById
            int testfindbyid = 1234567890;
            Assert.IsNotNull(database.BlackListRecords.Find(testfindbyid));

            #endregion

            #region  FindRecordByVolunteer

        #endregion

        #region FindRecordByOrg

        #endregion

        #region FindRecordByPro

        #endregion

        #region FindRecordByadder

        #endregion

        #region FindRecordByAddTime

        #endregion

        #region FindRecordByEndTime

        #endregion
    }

        [TestMethod()]
        public void EditBlackListTest()
        {
            #region EmptyId
            BlackListResult testedit1 = new BlackListResult();
            #endregion

            #region NoExistingRecord
            BlackListResult testedit2 = new BlackListResult()
            {
                id = 1234567
            };
            #endregion
        }

        [TestMethod()]
        public void DeleteBlackListTest()
        {
            int tempid = 1234567890;
            BlackListRecord TestDeleteResult1;
            if (database.BlackListRecords.Find(tempid) != null)
            {
                TestDeleteResult1 = new BlackListRecord()
                {
                    Id = 1234567890
                };
                helper.AddBlackListRecord(TestDeleteResult1);
            }
            else
            {
                TestDeleteResult1 = database.BlackListRecords.Find(tempid);
            }
            helper.DeleteBlackListRecord(tempid);
        }
    }
}
