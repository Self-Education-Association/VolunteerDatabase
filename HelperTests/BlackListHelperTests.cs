using Microsoft.VisualStudio.TestTools.UnitTesting;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper.Tests
{
    [TestClass()]
    public class BlackListHelperTests
    {
        Database database = DatabaseContext.GetInstance();
        BlackListHelper helper = BlackListHelper.GetInstance();
        VolunteerHelper volunteerhelper = VolunteerHelper.GetInstance();
        ProjectManageHelper projectmanagerhelper = ProjectManageHelper.GetInstance();
        IdentityHelper identityhelper = IdentityHelper.GetInstance();

        [TestMethod()]
        public void GetInstanceTest()
        {
            Assert.IsInstanceOfType(helper, typeof(BlackListHelper)); ;
        }

        [TestMethod()]
        public void AddBlackListTest()
        {
            // 创建第一个volunteer
            Volunteer v1 = CreateVolunteer();
            database.Volunteers.Add(v1);
            // 创建一个adder
            AppUser adder = CreateUser();
            database.Users.Add(adder);
            // 创建一个org
            Organization org = identityhelper.CreateOrFindOrganization(OrganizationEnum.TestOnly);
            // 创建一个pro
            Project pro = CreateProject();
            // 添加第一条黑名单记录
            BlackListRecord testaddrecord1 = new BlackListRecord
            {
                //Id = 1234567890,
                Volunteer = database.Volunteers.Single(b => b.StudentNum == v1.StudentNum ),
                Adder = database.Users.Single(b => b.StudentNum == adder.StudentNum ),
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2090 / 2 / 11),
                AddTime = System.DateTime.Now,
                Project = database.Projects.Single(b => b.Name == pro.Name)
            };
            BlackListResult result = helper.AddBlackListRecord(testaddrecord1);
               if ( !result.Succeeded )
               {
                Assert.Fail("返回错误结果！请检查后重试");
               }
            var actual = helper.FindBlackList(v1);
            if ( actual == null )
            {
                Assert.Fail("记录添加失败！数据库无此记录！");
            }

            // 测试ExistingRecord
            BlackListRecord testaddrecord3 = new BlackListRecord
            {
                //Id = 0987654321
                Volunteer = v1,
                Adder = adder,
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2090 / 2 / 11),
                AddTime = System.DateTime.Now,
                Project = pro
            };
            BlackListResult existingrecordresult = helper.AddBlackListRecord(testaddrecord3);
            //Assert.AreEqual(existingrecordresult, BlackListResult.AddBlackListRecordErrorEnum.ExistingRecord, "检验existingrecord失败！");
            if (!BlackListResult.AreSame(BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.ExistingRecord), existingrecordresult))
            {
                Assert.Fail();
            }

            // 测试WrongTime
            BlackListRecord testaddrecord4 = new BlackListRecord
            {
                EndTime = new DateTime(2017 / 2 / 1),
                AddTime = System.DateTime.Now
            };
            var wrongtimeresult = helper.AddBlackListRecord(testaddrecord4);
            //Assert.AreEqual(wrongtimeresult, BlackListResult.AddBlackListRecordErrorEnum.WrongTime, "测试wrongtime失败！");
            if (!BlackListResult.AreSame(BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.WrongTime), wrongtimeresult))
            {
                Assert.Fail();
            }

            // 测试Nullrecord
            BlackListRecord testaddrecord5 = new BlackListRecord();
            var nullrecordresult = helper.AddBlackListRecord(testaddrecord5);
            //Assert.AreEqual(nullrecordresult, BlackListResult.AddBlackListRecordErrorEnum.NullRecord, "测试nullrecord失败！");
            if (!BlackListResult.AreSame(BlackListResult.Error(BlackListResult.AddBlackListRecordErrorEnum.NullRecord), nullrecordresult))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void FindBlackListTest()
        {
            //创建两条新的黑名单记录
            Volunteer v1 = CreateVolunteer();
            database.Volunteers.Add(v1);
            AppUser adder1 = CreateUser();
            database.Users.Add(adder1);
            Organization org = identityhelper.CreateOrFindOrganization(OrganizationEnum.TestOnly);
            Project pro1 = CreateProject();
            database.Projects.Add(pro1);
            BlackListRecord blacklistrecord = new BlackListRecord
            {
                // Id = 1234567890,
                Volunteer = database.Volunteers.Single(b => b.StudentNum == v1.StudentNum ),
                Adder = database.Users.Single(b => b.StudentNum == adder1.StudentNum ),
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2090,2,11),
                AddTime = System.DateTime.Now,
                Project = database.Projects.Single(b => b.Name == pro1.Name )
            };
            database.BlackListRecords.Add(blacklistrecord);
            Project pro2 = CreateProject();
            database.Projects.Add(pro2);
            BlackListRecord blacklistrecord2 = new BlackListRecord
            {
                // Id = 1234567890,
                Volunteer = database.Volunteers.Single(b => b.StudentNum == v1.StudentNum),
                Adder = database.Users.Single(b => b.StudentNum == adder1.StudentNum),
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2050,11,13),
                AddTime = System.DateTime.Now,
                Project = database.Projects.Single(b => b.Name == pro2.Name)
            };
            database.BlackListRecords.Add(blacklistrecord2);
            // 测试FindBlackListByVolunteer
            var findbyvolunteer = helper.FindBlackList(v1);
            if ( findbyvolunteer == null )
            {
                Assert.Fail("找不到记录！");
            }
            else if ( findbyvolunteer.Count() != 2)
            {
                Assert.Fail("测试方法findbyvolunteer失败！");
            }

            //测试FindBlackListByOrg
            var findbyorg = helper.FindBlackList(org);
            if( findbyorg == null )
            {
                Assert.Fail("找不到记录！");
            }
            else if (findbyvolunteer.Count() != 2)
            {
                Assert.Fail("测试方法findbyorg失败！");
            }

            //测试FindBlackListByPro
            var findbypro1 = helper.FindBlackList(pro1);
            var findbypro2 = helper.FindBlackList(pro2);
            if ( findbypro1.Count != 1 && findbypro2.Count !=1)
            {
                Assert.Fail("测试方法FindBlackListByPro失败！返回值与预期值不符！");
            }

            //测试FindBlackListByAdder
            var findbyadder = helper.FindBlackList(adder1);
            if (findbyadder == null)
            {
                Assert.Fail("找不到记录！");
            }
            else if (findbyadder.Count() != 2)
            {
                Assert.Fail("测试方法findbyadder失败！");
            }

            //测试FindBlackListByAddTime
            var findbyaddtime = helper.FindBlackListByAddTime(blacklistrecord.AddTime, blacklistrecord.EndTime);
            if (findbyaddtime == null)
            {
                Assert.Fail("找不到记录！");
            }
            else if (findbyaddtime.Count() != 1 )
            {
                Assert.Fail("测试方法findbyaddtime失败！");
            }

            //测试FindBlackListByEndTime
            var findbyendtime = helper.FindBlackListByEndTime(blacklistrecord.AddTime, blacklistrecord2.EndTime);
            if ( findbyendtime == null )
            {
                Assert.Fail("找不到记录！");
            }
            else if ( findbyendtime.Count() != 2 )
            {
                Assert.Fail("测试方法findbyendtime失败");
            }

            //清空数据库

        }

        [TestMethod()]
        public void EditBlackListTest()
        {
            //添加新的记录
            Volunteer v = CreateVolunteer();
            database.Volunteers.Add(v);
            AppUser adder = CreateUser();
            database.Users.Add(adder);
            Organization org = identityhelper.CreateOrFindOrganization(OrganizationEnum.TestOnly);
            Project pro = CreateProject();
            database.Projects.Add(pro);
            BlackListRecord blacklistrecord = new BlackListRecord
            {
                // Id = 1234567890,
                Volunteer = database.Volunteers.Single(b => b.StudentNum == v.StudentNum),
                Adder = database.Users.Single(b => b.StudentNum == adder.StudentNum),
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2090, 2, 11),
                AddTime = System.DateTime.Now,
                Project = database.Projects.Single(b => b.Name == pro.Name)
            };
            database.BlackListRecords.Add(blacklistrecord);
            var tempblacklist = helper.FindBlackList(v);
            BlackListRecord blacklist = tempblacklist.First();
            // 测试 EmptyId
            var tempendtime = new DateTime( 2020,2,11 );
            BlackListResult testresult = helper.EditBlackListRecord(blacklistrecord.UID  , tempendtime, BlackListRecordStatus.Enabled);
            if ( testresult.Succeeded )
            {
                Assert.Fail("返回结果异常！");
            }
            //测试EditRecord
           BlackListResult result = helper.EditBlackListRecord(blacklist.UID, tempendtime , BlackListRecordStatus.Enabled);
            var actual = helper.FindBlackListByEndTime(blacklistrecord.AddTime, tempendtime);
            var actualendtime = actual.First();
          if ( !result.Succeeded )
            {
                Assert.Fail("结果返回异常！");
            }
          else if ( actualendtime.EndTime != tempendtime  )
            {
                Assert.Fail("edit方法失败！");
            }
        }

        [TestMethod()]
        public void DeleteBlackListTest()
        {
            //添加新的记录
            Volunteer v = CreateVolunteer();
            database.Volunteers.Add(v);
            AppUser adder = CreateUser();
            database.Users.Add(adder);
            Organization org = identityhelper.CreateOrFindOrganization(OrganizationEnum.TestOnly);
            Project pro = CreateProject();
            database.Projects.Add(pro);
            BlackListRecord blacklistrecord = new BlackListRecord
            {
                // Id = 1234567890,
                Volunteer = database.Volunteers.Single(b => b.StudentNum == v.StudentNum),
                Adder = database.Users.Single(b => b.StudentNum == adder.StudentNum),
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2090, 2, 11),
                AddTime = System.DateTime.Now,
                Project = database.Projects.Single(b => b.Name == pro.Name)
            };
            database.BlackListRecords.Add(blacklistrecord);
            var tempblacklist = helper.FindBlackList(adder);
            var blacklist = tempblacklist.First();
            if ( helper.FindBlackList(adder) != null )
            {
                Assert.Fail("记录添加失败！");
            }
            // 测试delete方法
            var result = helper.DeleteBlackListRecord(blacklist.UID);
            if ( !result.Succeeded )
            {
                Assert.Fail("返回结果异常！");
            }
            var actual = helper.FindBlackList(adder);
            if ( actual != null )
            {
                Assert.Fail("方法测试失败！");
            }
        }

        private Volunteer CreateVolunteer ( )
        {
            Random tempnum = new Random();
            int studentnum = tempnum.Next(000, 999);
            Guid uid = Guid.NewGuid();
            string name = uid.ToString();
            Volunteer volunteer = new Volunteer
            {
                //Id = 000,
                StudentNum = studentnum,
                Mobile = "1234567890-",
                Name = name,
                Email = "AddTest@test.com",
                Class = "AddTestClass",
                Room = "AddTestRoom"
            };
            return volunteer;
        }

        private AppUser CreateUser( )
        {
            Guid temp = Guid.NewGuid( );
            string name = temp.ToString( );
            Random tempnum = new Random();
            int studentnum = tempnum.Next(000,999);
            AppUser user = new AppUser()
            {
                AccountName = name,
                StudentNum = studentnum,
                Mobile = "1234567890",
                Email = "test@test.com"
            };
            return user;
        }

        private Project CreateProject( )
        {
            Organization org = identityhelper.CreateOrFindOrganization(OrganizationEnum.TestOnly);
            Guid uid = Guid.NewGuid();
            string name = uid.ToString();
            Project project = new Project()
            {
                Name = name,
                Place = "testplace",
                Creater = org
            };
            return project;

        }

        public static bool AreSame(BlackListRecord a, BlackListRecord b)
        {
            if (a == null && b == null)
                return true;
            if ((a == null && b != null) || (a != null && b == null))
                return false;
            else if (a.Adder== b.Adder && a.AddTime == b.AddTime&&a.EndTime==b.AddTime&&a.Volunteer==b.Volunteer&&a.Project==b.Project&&a.Organization==b.Organization&&a.Status==b.Status)
            {
  
                    return true;
            
            }
            else
                return false;
        }
    }
    }

