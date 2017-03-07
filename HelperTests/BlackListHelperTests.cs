using Microsoft.VisualStudio.TestTools.UnitTesting;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Interface;
using System.Data.Entity.Infrastructure;

namespace VolunteerDatabase.Helper.Tests
{
    [TestClass()]
    public class BlackListHelperTests
    {
        Database database = DatabaseContext.GetInstance();
        BlackListHelper helper = BlackListHelper.GetInstance();
        ProjectManageHelper projectmanagerhelper = ProjectManageHelper.GetInstance();
        IdentityHelper identityhelper = IdentityHelper.GetInstance();
        VolunteerHelper volunteerhelper;
        private AppUserIdentityClaims testclaims;


        [TestInitialize()]
        public void Init()
        {
            string userName = "BlackListRecordHelper";
            AppUser user = new AppUser
            {
                AccountName = userName,
                Name = "BlackListRecordHelper",
                Mobile = "1888888888",
                Email = "123@ab.com"
            };
            string password = "VolunteerHelperPassword";
            var dbUser = database.Users.SingleOrDefault(u => u.AccountName == user.AccountName);
            if (dbUser != null)
            {
                //等一下注释掉这一句
                database.Users.Remove(dbUser);
                database.SaveChanges();
            }
            IdentityResult result = identityhelper.CreateUser(user, password, Interface.AppRoleEnum.OrgnizationMember, OrganizationEnum.SEA团队);
            testclaims = identityhelper.CreateClaims(user.AccountName, password);
            volunteerhelper = VolunteerHelper.GetInstance(testclaims);
        }

        [TestMethod()]
        public void GetInstanceTest()
        {
            Assert.IsInstanceOfType(helper, typeof(BlackListHelper)); ;
        }

        [TestMethod()]
        public void AddBlackListTest()
        {
            // 创建第一个volunteer
            Random tempnum = new Random();
            int studentnum = tempnum.Next(000, 999);
            Guid uid = Guid.NewGuid();
            string name = uid.ToString();
            Volunteer v1 = new Volunteer()
            {
                //Id = 000,
                StudentNum = studentnum,
                Mobile = "1234567890-",
                Name = name,
                Email = "AddTest@test.com",
                Class = "AddTestClass",
                Room = "AddTestRoom"
            };
            volunteerhelper.AddVolunteer(v1);
            v1 = database.Volunteers.Single(b => b.StudentNum == v1.StudentNum);
            // 创建一个adder
            Guid temp = Guid.NewGuid();
            string appusername = temp.ToString();
            Random rnd = new Random();
            int usertempnum = rnd.Next(000, 999);
            int usernum = usertempnum;
            AppUser adder = new AppUser()
            {
                AccountName = appusername,
                StudentNum = usernum,
                Mobile = "1234567890",
                Email = "test@test.com"
            };
            identityhelper.CreateUser(adder, "23457890-", AppRoleEnum.OrgnizationMember, OrganizationEnum.TestOnly);
            adder = database.Users.Single(a => a.AccountName == adder.AccountName);
            // 创建一个org
            Organization org = identityhelper.CreateOrFindOrganization(OrganizationEnum.TestOnly);
            // 创建一个pro
            Guid prouid = Guid.NewGuid();
            string proname = uid.ToString();
            Project pro = new Project()
            {
                Name = proname,
                Place = "testplace",
                Organization = org
            };
            projectmanagerhelper.CreatNewProject(org, System.DateTime.Now, pro.Name, pro.Place, "", 70);
            pro = database.Projects.Single(p => p.Name == pro.Name);
            // 添加第一条黑名单记录
            BlackListRecord testaddrecord1 = new BlackListRecord
            {
                //Id = 1234567890,
                Volunteer = database.Volunteers.Single(b => b.StudentNum == v1.StudentNum),
                Adder = database.Users.Single(b => b.StudentNum == adder.StudentNum),
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2090, 2, 11),
                AddTime = System.DateTime.Now,
                Project = database.Projects.Single(b => b.Name == pro.Name)
            };
            BlackListResult result = helper.AddBlackListRecord(testaddrecord1);
            if (!result.Succeeded)
            {
                Assert.Fail("返回错误结果！请检查后重试");
            }
            var actual = helper.FindBlackList(v1);
            if (actual == null)
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
                EndTime = new DateTime(2090, 2, 11),
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
                EndTime = new DateTime(2017, 2, 1),
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
            //清空数据库
            DeleteOrgnization(org); // 清空org 同时清空了blacklistrecord和org下的project，users
            volunteerhelper.DeleteVolunteer(v1.StudentNum);

        }

        [TestMethod()]
        public void FindBlackListTest()
        {
            //创建两条新的黑名单记录
            //创建志愿者v1
            Random tempnum = new Random();
            int studentnum = tempnum.Next(000, 999);
            Guid uid = Guid.NewGuid();
            string name = uid.ToString();
            Volunteer v1 = new Volunteer()
            {
                //Id = 000,
                StudentNum = studentnum,
                Mobile = "1234567890-",
                Name = name,
                Email = "AddTest@test.com",
                Class = "AddTestClass",
                Room = "AddTestRoom"
            };
            volunteerhelper.AddVolunteer(v1);
            v1 = database.Volunteers.Single(b => b.StudentNum == v1.StudentNum);
            //创建adder1
            Guid temp = Guid.NewGuid();
            string appusername = temp.ToString();
            Random rnd = new Random();
            int usertempnum = rnd.Next(000, 999);
            int usernum = usertempnum;
            AppUser adder1 = new AppUser()
            {
                AccountName = appusername,
                StudentNum = usernum,
                Mobile = "1234567890",
                Email = "test@test.com"
            };
            identityhelper.CreateUser(adder1, "23457890-", AppRoleEnum.OrgnizationMember, OrganizationEnum.TestOnly);
            adder1 = database.Users.Single(b => b.AccountName == adder1.AccountName);
            //创建org
            Organization org = identityhelper.CreateOrFindOrganization(OrganizationEnum.TestOnly);
            Guid prouid = Guid.NewGuid();
            string proname = uid.ToString();
            //创建pro1
            Project pro1 = new Project()
            {
                Name = proname,
                Place = "testplace",
                Organization = org
            };
            projectmanagerhelper.CreatNewProject(org, System.DateTime.Now, pro1.Name, pro1.Place, "", 20);
            pro1 = database.Projects.Single(b => b.Name == pro1.Name);
            //创建黑名单blacklistrecord
            BlackListRecord blacklistrecord = new BlackListRecord
            {
                // Id = 1234567890,
                Volunteer = database.Volunteers.Single(b => b.StudentNum == v1.StudentNum),
                Adder = database.Users.Single(b => b.StudentNum == adder1.StudentNum),
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2090, 2, 11),
                AddTime = System.DateTime.Now,
                Project = database.Projects.Single(b => b.Name == pro1.Name)
            };
            helper.AddBlackListRecord(blacklistrecord);
            //创建项目pro2
            Guid prouid2 = Guid.NewGuid();
            string proname2 = prouid2.ToString();
            Project pro2 = new Project()
            {
                Name = proname2,
                Place = "testplace",
                Organization = org
            };
            projectmanagerhelper.CreatNewProject(org, System.DateTime.Now, pro2.Name, pro2.Place, "", 20);
            pro2 = database.Projects.Single(b => b.Name == pro2.Name);
            //创建blacklistrecord2
            BlackListRecord blacklistrecord2 = new BlackListRecord
            {
                // Id = 1234567890,
                Volunteer = database.Volunteers.Single(b => b.StudentNum == v1.StudentNum),
                Adder = database.Users.Single(b => b.StudentNum == adder1.StudentNum),
                Status = BlackListRecordStatus.Enabled,
                Organization = org,
                EndTime = new DateTime(2050, 11, 13),
                AddTime = System.DateTime.Now,
                Project = database.Projects.Single(b => b.Name == pro2.Name)
            };
            helper.AddBlackListRecord(blacklistrecord2);
            // 测试FindBlackListByVolunteer
            var findbyvolunteer = helper.FindBlackList(v1);
            if (findbyvolunteer == null)
            {
                Assert.Fail("找不到记录！");
            }
            else if (findbyvolunteer.Count() != 2)
            {
                Assert.Fail("测试方法findbyvolunteer失败！");
            }

            //测试FindBlackListByOrg
            var findbyorg = helper.FindBlackList(org);
            if (findbyorg == null)
            {
                Assert.Fail("找不到记录！");
            }
            else if (findbyorg.Count() < 2)
            {
                Assert.Fail("测试方法findbyorg失败！");
            }

            //测试FindBlackListByPro
            var findbypro1 = helper.FindBlackList(pro1);
            var findbypro2 = helper.FindBlackList(pro2);
            if (findbypro1.Count != 1 && findbypro2.Count != 1)
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
            else if (findbyaddtime.Count() < 1)
            {
                Assert.Fail("测试方法findbyaddtime失败！");
            }

            //测试FindBlackListByEndTime
            var findbyendtime = helper.FindBlackListByEndTime(blacklistrecord.AddTime, blacklistrecord.EndTime);
            if (findbyendtime == null)
            {
                Assert.Fail("找不到记录！");
            }
            else if (findbyendtime.Count() < 2)
            {
                Assert.Fail("测试方法findbyendtime失败");
            }

            //清空数据库
            DeleteOrgnization(org); // 清空org 同时清空了blacklistrecord和org下的project，users
            volunteerhelper.DeleteVolunteer(v1.StudentNum);

        }

        [TestMethod()]
        public void EditBlackListTest()
        {
            //添加新的记录
            //创建volunteer v
            Random tempnum = new Random();
            int studentnum = tempnum.Next(000, 999);
            Guid uid = Guid.NewGuid();
            string name = uid.ToString();
            Volunteer v = new Volunteer()
            {
                //Id = 000,
                StudentNum = studentnum,
                Mobile = "1234567890-",
                Name = name,
                Email = "AddTest@test.com",
                Class = "AddTestClass",
                Room = "AddTestRoom"
            };
            volunteerhelper.AddVolunteer(v);
            v = database.Volunteers.Single(b => b.StudentNum == v.StudentNum);
            //创建adder
            Guid temp = Guid.NewGuid();
            string appusername = temp.ToString();
            Random rnd = new Random();
            int usertempnum = rnd.Next(000, 999);
            int usernum = usertempnum;
            AppUser adder = new AppUser()
            {
                AccountName = appusername,
                StudentNum = usernum,
                Mobile = "1234567890",
                Email = "test@test.com"
            };
            identityhelper.CreateUser(adder, "23457890-", AppRoleEnum.OrgnizationMember, OrganizationEnum.TestOnly);
            Organization org = identityhelper.CreateOrFindOrganization(OrganizationEnum.TestOnly);
            //创建pro 
            Guid prouid = Guid.NewGuid();
            string proname = uid.ToString();
            Project pro = new Project()
            {
                Name = proname,
                Place = "testplace",
                Organization = org
            };
            projectmanagerhelper.CreatNewProject(org, System.DateTime.Now, pro.Name, pro.Place, "", 20);
            //创建blacklistrecord
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
            helper.AddBlackListRecord(blacklistrecord);
            var tempblacklist = helper.FindBlackList(v);
            BlackListRecord blacklist = tempblacklist.First();
            // 测试 EmptyId
            var tempendtime = new DateTime(2020, 2, 11);
            //BlackListResult testresult = helper.EditBlackListRecord(blacklistrecord.UID  , tempendtime, BlackListRecordStatus.Enabled);
            //if ( testresult.Succeeded )
            //{
            //    Assert.Fail("返回结果异常！");
            //}
            //测试EditRecord
            BlackListResult result = helper.EditBlackListRecord("没啥理由", blacklist.UID, tempendtime, BlackListRecordStatus.Enabled);
            blacklist = helper.FindBlackList(v).First();
            var actual = helper.FindBlackListByEndTime(blacklist.AddTime, tempendtime);
            var actualendtime = actual.First();
            if (!result.Succeeded)
            {
                Assert.Fail("结果返回异常！");
            }
            else if (actualendtime.EndTime != tempendtime)
            {
                Assert.Fail("edit方法失败！");
            }
            //清空数据库
            DeleteOrgnization(org); // 清空org 同时清空了blacklistrecord和org下的project，users
            volunteerhelper.DeleteVolunteer(v.StudentNum);
        }

        [TestMethod()]
        public void DeleteBlackListTest()
        {
            //添加新的记录
            //创建volunteer v
            Random tempnum = new Random();
            int studentnum = tempnum.Next(000, 999);
            Guid uid = Guid.NewGuid();
            string name = uid.ToString();
            Volunteer v = new Volunteer()
            {
                //Id = 000,
                StudentNum = studentnum,
                Mobile = "1234567890-",
                Name = name,
                Email = "AddTest@test.com",
                Class = "AddTestClass",
                Room = "AddTestRoom"
            };
            volunteerhelper.AddVolunteer(v);
            //创建adder
            Guid temp = Guid.NewGuid();
            string appusername = temp.ToString();
            Random rnd = new Random();
            int usertempnum = rnd.Next(000, 999);
            int usernum = usertempnum;
            AppUser adder = new AppUser()
            {
                AccountName = appusername,
                StudentNum = usernum,
                Mobile = "1234567890",
                Email = "test@test.com"
            };
            identityhelper.CreateUser(adder, "23457890-", AppRoleEnum.OrgnizationMember, OrganizationEnum.TestOnly);
            Organization org = identityhelper.CreateOrFindOrganization(OrganizationEnum.TestOnly);
            //创建pro
            Guid prouid = Guid.NewGuid();
            string proname = uid.ToString();
            Project pro = new Project()
            {
                Name = proname,
                Place = "testplace",
                Organization = org
            };
            projectmanagerhelper.CreatNewProject(org, System.DateTime.Now, pro.Name, pro.Place, "", 20);
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
            helper.AddBlackListRecord(blacklistrecord);
            var tempblacklist = helper.FindBlackList(adder);
            var blacklist = tempblacklist.First();
            adder = database.Users.Single(u => u.AccountName == adder.AccountName);
            if (helper.FindBlackList(adder) == null)
            {
                Assert.Fail("记录添加失败！");
            }
            // 测试delete方法
            var result = helper.DeleteBlackListRecord(blacklist.UID);
            if (!result.Succeeded)
            {
                Assert.Fail("返回结果异常！");
            }
            var actual = helper.FindBlackList(adder);
            if (actual.Count != 0)
            {
                Assert.Fail("方法测试失败！");
            }
            //清空数据库
            DeleteOrgnization(org); // 清空org 同时清空了blacklistrecord和org下的project，users
            volunteerhelper.DeleteVolunteer(v.StudentNum);
        }


        public void DeleteOrgnization(Organization org)
        {

            var list = database.Users.Where(u => u.Organization.Id == org.Id).ToList();
            foreach (var item in list)
            {
                database.Users.Remove(item);
            }
            var projectList = database.Projects.Where(p => p.Organization.Id == org.Id).ToList();
            foreach (var item in projectList)
            {
                database.Projects.Remove(item);
            }
            var blacklist = database.BlackListRecords.Where(u => u.Organization.Id == org.Id).ToList();
            foreach (var item in blacklist)
            {
                database.BlackListRecords.Remove(item);
            }
            //var orgInDb = database.Organizations.SingleOrDefault(o => o.Name == org.Name);
            database.Organizations.Remove(org);
            Save();
        }

        [TestCleanup()]
        public void DeleteClaimsUsers()
        {
            var dbUser = database.Users.SingleOrDefault(u => u.AccountName == "BlackListRecordHelper");
            if (dbUser != null)
            {
                database.Users.Remove(dbUser);
                database.SaveChanges();
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
                        flag = false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            } while (flag);
        }
        public static bool AreSame(BlackListRecord a, BlackListRecord b)
        {
            if (a == null && b == null)
                return true;
            if ((a == null && b != null) || (a != null && b == null))
                return false;
            else if (a.Adder == b.Adder && a.AddTime == b.AddTime && a.EndTime == b.AddTime && a.Volunteer == b.Volunteer && a.Project == b.Project && a.Organization == b.Organization && a.Status == b.Status)
            {

                return true;

            }
            else
                return false;
        }
    }
}

