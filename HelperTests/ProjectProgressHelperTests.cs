using Microsoft.VisualStudio.TestTools.UnitTesting;
using VolunteerDatabase.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace VolunteerDatabase.Helper.Tests
{
    [TestClass()]
    public class ProjectProgressHelperTests
    {
        Database database = DatabaseContext.GetInstance();
        ProjectProgressHelper helper = ProjectProgressHelper.GetInstance();
        ProjectManageHelper projectmanagehelper = ProjectManageHelper.GetInstance();
        IdentityHelper identityhelper = IdentityHelper.GetInstance();
        private AppUserIdentityClaims testclaims;
        VolunteerHelper volunteerhelper;


        [TestInitialize()]
        public void Init()
        {
            AppUser user = CreateUser();
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
            Assert.IsInstanceOfType(helper, typeof(ProjectProgressHelper));
        }

        [TestMethod()]
        public void FindAuthorizedProjectsByUserTest()
        {
            //创建一个originalmanager
            AppUser originalmanager = CreateUser();
            AppUser actualmanager = database.Users.SingleOrDefault(b => b.StudentNum == originalmanager.StudentNum);
            if (actualmanager != null)
            {
                database.Users.Remove(actualmanager);
                Save();
            }
            identityhelper.CreateUser(originalmanager, "zxcvbnm,./", Interface.AppRoleEnum.OrgnizationMember, OrganizationEnum.TestOnly);
            originalmanager = database.Users.Single(b => b.AccountName == originalmanager.AccountName);
            //创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);
            //创建两个项目
            Guid uid1 = Guid.NewGuid();
            Guid uid2 = Guid.NewGuid();
            string name1 = "pro" + uid1.ToString();
            string name2 = "pro" + uid2.ToString();
            projectmanagehelper.CreatNewProject(org, DateTime.Now, name1, "uibe", "FindAuthorizedProjectsByUser1", 20);
            projectmanagehelper.CreatNewProject(org, DateTime.Now, name2, "uibe", "FindAuthorizedProjectsByUser2", 20);
            var addresult1 = database.Projects.SingleOrDefault(b => b.Name == name1);
            var addresult2 = database.Projects.SingleOrDefault(b => b.Name == name2);
            if (addresult1 == null && addresult2 == null)
            {
                Assert.Fail("添加记录失败！");
            }
            var manager = database.Users.Single(m => m.AccountName == originalmanager.AccountName);
            var managerlist = projectmanagehelper.FindManagerListByStudentNum(originalmanager.StudentNum);
            projectmanagehelper.ProjectMessageInput(name1, "FindAuthorizedProjectsByUser1", "uibe", 10, DateTime.Now, managerlist, addresult1);
            projectmanagehelper.ProjectMessageInput(name2, "FindAuthorizedProjectsByUser1", "uibe", 10, DateTime.Now, managerlist, addresult2);

            // 测试FindAuthorizedProjByUser
            AppUser findmanager = database.Users.Single(b => b.StudentNum == originalmanager.StudentNum);
            var result = helper.FindAuthorizedProjectsByUser(findmanager);//59行!!想办法找到contain方法的替代
            if (result == null || result.Count() != 2)
            {
                Assert.Fail("通过user查询项目失败！");
            }

            // 删除数据库的有关数据[org pro manager]
            DeleteOrgnization(org);

        } //Helper 45行

        [TestMethod()]
        public void FindProjectByProjectIdTest()
        {
            //创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);

            // 创建一个项目
            Guid uid = Guid.NewGuid();
            string name = "pro" + uid.ToString();
            Project pro = new Project
            {
                Name = name,
                Place = "pro",
                Maximum = 20,
                Details = "nothing",
                Time = DateTime.Now,
                Condition = Interface.ProjectCondition.Ongoing,
                Organization = org
            };
            projectmanagehelper.CreatNewProject(org, DateTime.Now, name, "uibe", "nothing", 20);
            var addresult = database.Projects.Where(b => b.Name == name);
            if (addresult == null)
            {
                Assert.Fail("添加记录失败！");
            }
            // 测试FindProjectByProjId
            var testrecord = database.Projects.SingleOrDefault(p => p.Name == name);
            var result = helper.FindProjectByProjectId(testrecord.Id);
            if (result == null)
            {
                Assert.Fail("查询记录失败！");
            }

            // 删除数据库的有关数据[pro org]
            DeleteOrgnization(org);

            //修改删除方法，删除方法中无法完全清空别处引用
        }

        /*   [TestMethod()]
         public void CreatVolunteerTest()
           {
               ProjectProgressHelper helper = ProjectProgressHelper.GetInstance();
               ProjectManageHelper projectmanagehelper = ProjectManageHelper.GetInstance();
               IdentityHelper identityhelper = IdentityHelper.GetInstance();
               VolunteerHelper volunteerhelper = VolunteerHelper.GetInstance();
              // 创建org
               Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);
               
              // 创建一个project
               Guid uid = Guid.NewGuid();
               string name = "CreatVolunteerTest" + uid.ToString();
               Project pro = new Project
               {
                   Name = name,
                   Place = "uibe",
                   Maximum = 20,
                   Details = "nothing",
                   Time = DateTime.Now,
                   Condition = Interface.ProjectCondition.Ongoing,
                   Organization = org
               };
               projectmanagehelper.CreatNewProject(org, DateTime.Now, name, "uibe", "nothing", 20);
               var addresult = database.Projects.Where(b => b.Name == name);
               if (addresult == null)
               {
                   Assert.Fail("添加记录失败！");
               }
               
              // 测试CreateVolunteer
               pro = database.Projects.SingleOrDefault(p => p.Name == name);     //volunteer 的studentnum 是int appuser是string型。
               Random tempnum = new Random();
               int studentnum = tempnum.Next(000, 999);
              ProgressResult result = helper.CreatVolunteer(pro,studentnum, "testroom", "CreatVolunteerTest", "CreatVolunteerTest@test", "1233456789");
               if ( !result.Succeeded )
               {
                   Assert.Fail("CreateVolunteer返回结果失败！");
               }
               var actual = volunteerhelper.FindVolunteer(123);
               if ( actual == null )
               {
                   Assert.Fail("CreateVolunteer失败！方法调用失败！");
               }
               
              // 删除数据库的有关数据[org vol pro]

               var deletepro = database.Projects.Where(p => p.Name == name);
               if (deletepro != null)
               {
                   foreach (var item in deletepro)
                   {
                       database.Projects.Remove(item);
                       database.SaveChanges();
                   }
               }
               var deleteorg = database.Organizations.Where(o => o.OrganizationEnum == org.OrganizationEnum).ToList();
               if (deleteorg != null)
               {
                   foreach (var item in deleteorg)
                   {
                       DeleteOrgnization(item);
                   }
               }
               if (volunteerhelper.FindVolunteer(123) != null)
               {
                   Assert.Fail("删除志愿者失败！");
               }

               
               //修改删除方法，删除方法中无法完全清空别处引用
           }*/

        [TestMethod()]
        public void FindSortedVolunteersByProjectTest()
        {

            // 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);

            // 创建一个project
            Guid uid = Guid.NewGuid();
            string name = "CreatVolunteerTest" + uid.ToString();
            Project pro = new Project
            {
                Name = name,
                Place = "uibe",
                Maximum = 20,
                Details = "nothing",
                Time = DateTime.Now,
                Condition = Interface.ProjectCondition.Ongoing,
                Organization = org
            };
            projectmanagehelper.CreatNewProject(org, DateTime.Now, name, "uibe", "nothing", 20);
            pro = database.Projects.Single(p => p.Name == pro.Name);
            var addresult = database.Projects.Where(b => b.Name == name);
            if (addresult == null)
            {
                Assert.Fail("添加记录失败！");
            }

            // 向项目中添加志愿者
            // 创建两个志愿者
            Random random1 = new Random();
            int studentnum1 = random1.Next(000, 999);
            Volunteer v1 = new Volunteer
            {
                StudentNum = studentnum1,
                Name = "FindSortedVolunteersByProject",
                Class = "testclass",
                Room = "testroom",
                Mobile = "18888888888",
                Score = 4
            };
            volunteerhelper.AddVolunteer(v1);
            if (database.Volunteers.Where(v => v.StudentNum == v1.StudentNum).Count() == 0)
            {
                Assert.Fail("插入对象失败！");
            }
            Random random2 = new Random();
            int studentnum2 = random2.Next(000, 999);
            Volunteer v2 = new Volunteer
            {
                StudentNum = studentnum2,
                Name = "FindSortedVolunteersByProject",
                Class = "testclass",
                Room = "testroom",
                Mobile = "18888888888",
                Score = 2
            };
            volunteerhelper.AddVolunteer(v2);
            helper.SingleVolunteerInputById(v1.StudentNum, pro);
            helper.SingleVolunteerInputById(v2.StudentNum, pro);


            // 测试FindSortedVolunteersByProject
            var volunteerlist = helper.FindSortedVolunteersByProject(pro);
            Volunteer testresult = volunteerlist.First();
            if (testresult.StudentNum != studentnum1)
            {
                Assert.Fail("测试FindSortedVolunteersByProject失败！");
            }

            // 删除数据库的有关数据[org pro vol]
            volunteerhelper.DeleteVolunteer(v1.StudentNum);
            volunteerhelper.DeleteVolunteer(v2.StudentNum);
            DeleteOrgnization(org);
        }



        [TestMethod()]
        public void SingleVolunteerInputByIdTest()
        {
            // 创建两个志愿者
            Random random1 = new Random();
            int studentnum1 = random1.Next(000, 999);
            Volunteer v1 = new Volunteer
            {
                StudentNum = studentnum1,
                Name = "FindVolunteerById",
                Class = "testclass",
                Room = "testroom",
                Mobile = "18888888888"
            };
            volunteerhelper.AddVolunteer(v1.StudentNum, "FindVolunteerById", "testclass", "18888888888");//志愿者1添加至数据库 志愿者2先进行无志愿者测试再添加进入数据库
            var addvolunteer1 = database.Volunteers.Where(v => v.StudentNum == v1.StudentNum).ToList();
            if (addvolunteer1.Count() == 0)
            {
                Assert.Fail("创建志愿者失败！");
            }
            Random random2 = new Random();
            int studentnum2 = random2.Next(000, 999);
            Volunteer v2 = new Volunteer
            {
                StudentNum = studentnum2,
                Name = "FindVolunteerById",
                Class = "testclass",
                Room = "testroom",
                Mobile = "18888888888"
            };


            // 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);

            // 创建一个project
            Project pro = CreateProject();
            projectmanagehelper.CreatNewProject(org, DateTime.Now, pro.Name, "uibe", "nothing", 1);
            var addresult = database.Projects.Where(b => b.Name == pro.Name);
            if (addresult == null)
            {
                Assert.Fail("添加记录失败！");
            }

            // 测试SingleVolunteerInputById
            pro = database.Projects.Single(p => p.Name == pro.Name);
            ProgressResult result = helper.SingleVolunteerInputById(v1.StudentNum, pro); //未将对象引用设置到对象的实例
            if (!result.Succeeded)
            {
                Assert.Fail("SingleVolunteerInputById方法结果返回失败！");
            }
            var actual = pro.Volunteers.Where(b => b.StudentNum == v1.StudentNum).Count();
            if (actual == 0)
            {
                Assert.Fail("测试SingleVolunteerInputById失败！未能成功添加志愿者！");
            }

            // 测试error"志愿者不存在于数据库中"
            ProgressResult novolunteererro = helper.SingleVolunteerInputById(v2.StudentNum, pro);
            if (novolunteererro.Succeeded)
            {
                Assert.Fail("error返回结果异常！");
            }

            // 测试error"已达项目人数上限，添加失败"
            volunteerhelper.AddVolunteer(v2);
            ProgressResult overmaximum = helper.SingleVolunteerInputById(v2.StudentNum, pro);
            if (overmaximum.Succeeded)
            {
                Assert.Fail("error返回结果异常！");
            }

            // 删除数据库的有关数据[org pro vol]
            volunteerhelper.DeleteVolunteer(studentnum1);
            volunteerhelper.DeleteVolunteer(studentnum2);
            DeleteOrgnization(org);
        }

        [TestMethod()]
        public void DeleteVolunteerFromProjectTest()
        {
            // 创建两个志愿者
            Volunteer v1 = CreateVolunteer();
            volunteerhelper.AddVolunteer(v1);
            // database.Volunteers.Add(v1);
            Random random2 = new Random();
            int studentnum2 = random2.Next(000, 999);
            Volunteer v2 = CreateVolunteer();
            volunteerhelper.AddVolunteer(v2);

            // 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);

            // 创建一个project
            Project pro = CreateProject();
            projectmanagehelper.CreatNewProject(org, DateTime.Now, pro.Name, "uibe", "nothing", 1);
            var addresult = database.Projects.Where(b => b.Name == pro.Name);
            if (addresult == null)
            {
                Assert.Fail("添加记录失败！");
            }

            // 利用SingleVolunteerInputById
            pro = database.Projects.Single(p => p.Name == pro.Name);
            ProgressResult addvolunteerresult = helper.SingleVolunteerInputById(v1.StudentNum, pro);
            if (!addvolunteerresult.Succeeded)
            {
                Assert.Fail("SingleVolunteerInputById方法结果返回失败！");
            }


            // 测试error"志愿者不在该项目中"
            ProgressResult novolunteererro = helper.DeleteVolunteerFromProject(v2, pro);
            if (novolunteererro.Succeeded)
            {
                Assert.Fail("error返回结果异常！");
            }

            // 测试error"已达项目人数上限，添加失败"
            ProgressResult overmaximum = helper.SingleVolunteerInputById(v2.StudentNum, pro);
            if (overmaximum.Succeeded)
            {
                Assert.Fail("error返回结果异常！");
            }

            // 测试DeleteVolunteerFromProject
            ProgressResult result = helper.DeleteVolunteerFromProject(v1, pro);
            if (!result.Succeeded)
            {
                Assert.Fail("返回结果异常！");
            }
            var actual = pro.Volunteers.Where(b => b.StudentNum == v1.StudentNum).Count();
            if (actual != 0)
            {
                Assert.Fail("测试DeleteVolunteerFromProject失败！");
            }

            // 删除数据库的有关数据[org pro vol]
            volunteerhelper.DeleteVolunteer(v1.StudentNum);
            volunteerhelper.DeleteVolunteer(v2.StudentNum);
            DeleteOrgnization(org);

            //修改删除方法，删除方法中无法完全清空别处引用
        }

        [TestMethod()]
        public void ScoringDefaultForVolunteersTest()
        {
            // 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);

            // 创建一个project
            Guid uid = Guid.NewGuid();
            string name = "ScoringDefaultForVolunteers" + uid.ToString();
            Project pro = new Project
            {
                Name = name,
                Place = "uibe",
                Maximum = 2,
                Details = "nothing",
                Time = DateTime.Now,
                Condition = Interface.ProjectCondition.Ongoing,
                Organization = org
            };
            projectmanagehelper.CreatNewProject(org, DateTime.Now, name, "uibe", "nothing", 2);
            var addresult = database.Projects.Where(b => b.Name == name);
            if (addresult == null)
            {
                Assert.Fail("添加记录失败！");
            }

            // 创建两个志愿者
            Random random1 = new Random();
            int studentnum1 = random1.Next(000, 999);
            Volunteer v1 = new Volunteer
            {
                StudentNum = studentnum1,
                Name = "ScoringDefaultForVolunteer",
                Class = "testclass",
                Room = "testroom",
                Mobile = "18888888888",
                Score = 0
            };
            volunteerhelper.AddVolunteer(v1);
            Random random2 = new Random();
            int studentnum2 = random2.Next(000, 999);
            Volunteer v2 = new Volunteer
            {
                StudentNum = studentnum2,
                Name = "ScoringDefaultForVolunteer",
                Class = "testclass",
                Room = "testroom",
                Mobile = "18888888888",
                Score = 0
            };
            volunteerhelper.AddVolunteer(v2);
            pro = database.Projects.SingleOrDefault(b => b.Name == pro.Name);
            helper.SingleVolunteerInputById(v1.StudentNum, pro);
            helper.SingleVolunteerInputById(v2.StudentNum, pro);
            ProgressResult result = helper.ScoringDefaultForVolunteers(pro);
            v1 = database.Volunteers.Single(b => b.StudentNum == v1.StudentNum);
            v2 = database.Volunteers.Single(b => b.StudentNum == v2.StudentNum);
            if (!result.Succeeded && pro.ScoreCondition == Interface.ProjectScoreCondition.Scored)
            {
                Assert.Fail("结果返回异常！");
            }
            if (v1.Score != 4 || v2.Score != 4)
            {
                Assert.Fail("Scoring4ForVolunteers失败！");
            }

            // 删除数据库的有关数据[org pro vol]
            volunteerhelper.DeleteVolunteer(v1.StudentNum);
            volunteerhelper.DeleteVolunteer(v2.StudentNum);
            DeleteOrgnization(org);
        }

        [TestMethod()]
        public void ScoreSingleVolunteerTest()
        {
            // 创建一个志愿者
            Random random = new Random();
            int studentnum = random.Next(000, 999);
            Volunteer v = new Volunteer
            {
                StudentNum = studentnum,
                Name = "FindVolunteerById",
                Class = "testclass",
                Room = "testroom",
                Mobile = "18888888888"
            };
            v.Score = 0;
            volunteerhelper.AddVolunteer(v);

            // 测试error
            ProgressResult result = helper.ScoreSingleVolunteer(0, v);
            if (result.Succeeded)
            {
                Assert.Fail("结果返回失败！");
            }

            // 测试ScoreSingleVolunteer
            result = helper.ScoreSingleVolunteer(5, v);
            if (!result.Succeeded)
            {
                Assert.Fail("添加失败！");
            }
            if (v.Score != 1)
            {
                Assert.Fail("测试ScoreSingleVolunteer失败！");
            }

            // 删除数据库的有关数据[vol]
            volunteerhelper.DeleteVolunteer(v.StudentNum);
        }

        [TestMethod()]
        public void FinishProjectTest()
        {
            // 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);

            // 创建一个project
            Guid uid = Guid.NewGuid();
            string name = "FinishProjectTest" + uid.ToString();
            Project pro = new Project
            {
                Name = name,
                Place = "uibe",
                Maximum = 1,
                Details = "nothing",
                Time = DateTime.Now,
                Organization = org,
                //    ScoreCondition = Interface.ProjectScoreCondition.UnScored
            };
            projectmanagehelper.CreatNewProject(org, DateTime.Now, name, "uibe", "nothing", 1); //unsocred 条件 突然变成scored
            var addresult = database.Projects.Where(b => b.Name == name).Count();
            if (addresult == 0)
            {
                Assert.Fail("添加记录失败！");
            }
            pro = database.Projects.Single(p => p.Name == pro.Name);
            //向项目添加志愿者
            // 创建一个志愿者
            Random random = new Random();
            int studentnum = random.Next(000, 999);
            Volunteer v = new Volunteer
            {
                StudentNum = studentnum,
                Name = "FindVolunteerById",
                Class = "testclass",
                Room = "testroom",
                Mobile = "18888888888"
            };
            ProgressResult tempresult = helper.SingleVolunteerInputById(v.StudentNum, pro);

            // 测试finishproject  
            ProgressResult result = helper.FinishProject(pro);
            if (result.Succeeded)
            {
                Assert.Fail("在unscored条件下通过测试，失败！");
            }
            pro.ScoreCondition = Interface.ProjectScoreCondition.Scored;
            result = helper.FinishProject(pro);
            if (!result.Succeeded)
            {
                Assert.Fail("方法返回失败结果！请检查重试！");
            }
            if (pro.Condition != Interface.ProjectCondition.Finished)
            {
                Assert.Fail("方法FinishProject测试失败！");
            }

            // 删除数据库的有关数据[org pro vol]
            DeleteOrgnization(org);
            volunteerhelper.DeleteVolunteer(v.StudentNum);
        }


        [TestMethod()]
        public void EditScoreTest()
        {
            // 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);

            // 创建一个project
            Guid uid = Guid.NewGuid();
            string name = "FinishProjectTest" + uid.ToString();
            Project pro = new Project
            {
                Name = name,
                Place = "uibe",
                Maximum = 1,
                Details = "nothing",
                Time = DateTime.Now,
                Organization = org,
                //    ScoreCondition = Interface.ProjectScoreCondition.UnScored
            };
            projectmanagehelper.CreatNewProject(org, DateTime.Now, name, "uibe", "nothing", 1); //unsocred 条件 突然变成scored
            var addresult = database.Projects.Where(b => b.Name == name).Count();
            if (addresult == 0)
            {
                Assert.Fail("添加记录失败！");
            }
            pro = database.Projects.Single(p => p.Name == pro.Name);
            //向项目添加志愿者
            // 创建一个志愿者
            Random random = new Random();
            int studentnum = random.Next(000, 999);
            Volunteer v = new Volunteer
            {
                StudentNum = studentnum,
                Name = "FindVolunteerById",
                Class = "testclass",
                Room = "testroom",
                Mobile = "18888888888"
            };
            volunteerhelper.AddVolunteer(v);
            v = database.Volunteers.SingleOrDefault(b => b.StudentNum == v.StudentNum);
            helper.SingleVolunteerInputById(v.StudentNum, pro);
            ProgressResult tempresult = helper.EditScore(v, pro, );
            if (tempresult.Succeeded)
            {
                Assert.Fail("不存在对应的征信记录failed.");
            }
            CreditRecord record = new CreditRecord
            {
                Participant = v,
                Project = pro,
                //Organization = org,
                Score = 0
            };
            database.CreditRecords.Add(record);
            Save();
            record = database.CreditRecords.Single(c => c.Organization.Id == org.Id);
            tempresult = helper.EditScore(v, pro, 2);
            if (!tempresult.Succeeded)
            {
                Assert.Fail("Result is beyond expections!");
            }
            //删除数据库
            database.CreditRecords.Remove(record);
            DeleteOrgnization(org);
            volunteerhelper.DeleteVolunteer(v);
            Save();
        }

        [TestMethod()]
        public void DeleteCreditRecordTest()
        {
            // 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);

            // 创建一个project
            Guid uid = Guid.NewGuid();
            string name = "FinishProjectTest" + uid.ToString();
            Project pro = new Project
            {
                Name = name,
                Place = "uibe",
                Maximum = 1,
                Details = "nothing",
                Time = DateTime.Now,
                Organization = org,
                //    ScoreCondition = Interface.ProjectScoreCondition.UnScored
            };
            projectmanagehelper.CreatNewProject(org, DateTime.Now, name, "uibe", "nothing", 1); //unsocred 条件 突然变成scored
            var addresult = database.Projects.Where(b => b.Name == name).Count();
            if (addresult == 0)
            {
                Assert.Fail("添加记录失败！");
            }
            pro = database.Projects.Single(p => p.Name == pro.Name);
            //向项目添加志愿者
            // 创建一个志愿者
            Random random = new Random();
            int studentnum = random.Next(000, 999);
            Volunteer v = new Volunteer
            {
                StudentNum = studentnum,
                Name = "FindVolunteerById",
                Class = "testclass",
                Room = "testroom",
                Mobile = "18888888888"
            };
            volunteerhelper.AddVolunteer(v);
            v = database.Volunteers.SingleOrDefault(b => b.StudentNum == v.StudentNum);
            helper.SingleVolunteerInputById(v.StudentNum, pro);
            CreditRecord record = new CreditRecord
            {
                Participant = v,
                Project = pro,
                //Organization = org,
                Score = 0
            };
            database.CreditRecords.Add(record);
            Save();
            record = database.CreditRecords.Single(c => c.Organization.Id == org.Id);
           ProgressResult result = helper.DeleteCreditRecord(record);
            if ( !result.Succeeded )
            {
                Assert.Fail();
            }
            if (database.CreditRecords.SingleOrDefault(c => c.Organization.Id == org.Id) != null)
            {
                Assert.Fail();
            }
            //删除数据库
            DeleteOrgnization(org);
            volunteerhelper.DeleteVolunteer(v);
            Save();
        }

        private Volunteer CreateVolunteer()
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

        private AppUser CreateUser()
        {
            Guid temp = Guid.NewGuid();
            string name = temp.ToString();
            Random tempnum = new Random();
            int studentnum = tempnum.Next(000, 999);
            AppUser user = new AppUser()
            {
                AccountName = name,
                StudentNum = studentnum,
                Name = "ProjectProgressHelper",
                Mobile = "1234567890",
                Email = "test@test.com"
            };
            return user;
        }

        private Project CreateProject()
        {
            Organization org = identityhelper.CreateOrFindOrganization(OrganizationEnum.TestOnly);
            Guid uid = Guid.NewGuid();
            string name = uid.ToString();
            Project project = new Project()
            {
                Name = name,
                Place = "testplace",
                Organization = org
            };
            return project;

        }
        public void DeleteOrgnization(Organization org)
        {

            var list = database.Users.Where(u => u.Organization.Id == org.Id).ToList();
            if (list != null)
            {
                foreach (var item in list)
                {
                    database.Users.Remove(item);

                }

            }
            var projectList = database.Projects.Where(p => p.Organization.Id == org.Id).ToList();
            if (projectList != null)
            {
                foreach (var item in projectList)
                {
                    database.Projects.Remove(item);
                }
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

 
    }
}