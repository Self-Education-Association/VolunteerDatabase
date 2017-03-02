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
        /* ProjectManageHelper projectmanagehelper = ProjectManageHelper.GetInstance();
         IdentityHelper identityhelper = IdentityHelper.GetInstance();
         VolunteerHelper volunteerhelper = VolunteerHelper.GetInstance();*/


        [TestMethod()]
        public void GetInstanceTest()
        {
            Assert.IsInstanceOfType(helper, typeof(ProjectProgressHelper));
        }

        [TestMethod()]
        public void FindAuthorizedProjectsByUserTest()
        {
            ProjectProgressHelper helper = ProjectProgressHelper.GetInstance();
            ProjectManageHelper projectmanagehelper = ProjectManageHelper.GetInstance();
            IdentityHelper identityhelper = IdentityHelper.GetInstance();
            VolunteerHelper volunteerhelper = VolunteerHelper.GetInstance();
            #region 创建一个originalmanager
            Guid temptnum = Guid.NewGuid();
            string studentnum = temptnum.ToString();
            AppUser originalmanager = new AppUser
            {
                StudentNum = studentnum,
                AccountName = "testuser",
                Name = "test",
                Room = "888",
                Mobile = "123456789"
            };
            identityhelper.CreateUser(originalmanager, "zxcvbnm,./", Interface.AppRoleEnum.OrgnizationMember, OrganizationEnum.TestOnly);
            #endregion
            #region 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);
            #endregion
            #region 创建两个项目
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
            var manager = database.Users.Single(m => m.AccountName== originalmanager.AccountName);
            var managerlist = projectmanagehelper.FindManagerListByStudentNum(studentnum);
            projectmanagehelper.ProjectMessageInput(name1, "FindAuthorizedProjectsByUser1", "uibe",10,DateTime.Now, managerlist,addresult1 );
            projectmanagehelper.ProjectMessageInput(name2, "FindAuthorizedProjectsByUser1", "uibe", 10, DateTime.Now, managerlist, addresult2);
            #endregion
            #region 测试FindAuthorizedProjByUser
            AppUser findmanager = database.Users.Single(b => b.StudentNum == originalmanager.StudentNum);
            var result = helper.FindAuthorizedProjectsByUser(findmanager);
            if ( result == null || result.Count() != 2)
            {
                Assert.Fail("通过user查询项目失败！");
            }
            #endregion
            #region 删除数据库的有关数据[org pro manager]
            var deleteorg = database.Organizations.Where(o => o.Id == org.Id).ToList();
            if (deleteorg != null)
            {
                foreach (var item in deleteorg)
                {
                    DeleteOrgnization(item);
                }
            }
            var deletepro = database.Projects.Where(p => p.Place.ToString() == "uibe");
            if (deletepro != null)
            {
                foreach (var item in deletepro)
                {
                    database.Projects.Remove(item);
                }
                database.SaveChanges();
            }
            var deletemanager = database.Users.Where(m => m.AccountName == originalmanager.AccountName).ToList();
            if (deletemanager != null)
            {
                foreach (var item in deletemanager)
                {
                    database.Users.Remove(item);
                }
                database.SaveChanges();
            }
            #endregion
        } //Helper 45行

        [TestMethod()]
        public void FindProjectByProjectIdTest()
        {
            ProjectProgressHelper helper = ProjectProgressHelper.GetInstance();
            ProjectManageHelper projectmanagehelper = ProjectManageHelper.GetInstance();
            IdentityHelper identityhelper = IdentityHelper.GetInstance();
            VolunteerHelper volunteerhelper = VolunteerHelper.GetInstance();
            #region 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);
            #endregion
            #region 创建一个项目
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
                Creater = org
            };
            projectmanagehelper.CreatNewProject(org, DateTime.Now, name, "uibe", "nothing", 20);
            var addresult = database.Projects.Where(b => b.Name == name);
            if (addresult == null)
            {
                Assert.Fail("添加记录失败！");
            }
            #endregion#region 创建一个project
            #region 测试FindProjectByProjId
            var testrecord = database.Projects.SingleOrDefault(p => p.Name == name);
            var result =helper.FindProjectByProjectId(testrecord.Id);
            if( result == null)
            {
                Assert.Fail("查询记录失败！");
            }
            #endregion
            #region 删除数据库的有关数据[pro org]
           /* var deleteorg = database.Organizations.Where(o => o.Id == org.Id).ToList();
            if (deleteorg != null)
            {
                foreach (var item in deleteorg)
                {
                    DeleteOrgnization(item);
                }
            }
            var deletepro = database.Projects.Where(p => p.Name == name);
            if (deletepro != null)
            {
                foreach (var item in deletepro)
                {
                    database.Projects.Remove(item);
                }
                database.SaveChanges();
            }*/
            #endregion
            //修改删除方法，删除方法中无法完全清空别处引用
        }

        /*   [TestMethod()]
         public void CreatVolunteerTest()
           {
               ProjectProgressHelper helper = ProjectProgressHelper.GetInstance();
               ProjectManageHelper projectmanagehelper = ProjectManageHelper.GetInstance();
               IdentityHelper identityhelper = IdentityHelper.GetInstance();
               VolunteerHelper volunteerhelper = VolunteerHelper.GetInstance();
               #region 创建org
               Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);
               #endregion
               #region 创建一个project
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
                   Creater = org
               };
               projectmanagehelper.CreatNewProject(org, DateTime.Now, name, "uibe", "nothing", 20);
               var addresult = database.Projects.Where(b => b.Name == name);
               if (addresult == null)
               {
                   Assert.Fail("添加记录失败！");
               }
               #endregion
               #region 测试CreateVolunteer
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
               #endregion
               #region 删除数据库的有关数据[org vol pro]

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

               #endregion
               //修改删除方法，删除方法中无法完全清空别处引用
           }*/

        [TestMethod()]
        public void FindSortedVolunteersByProjectTest()
        {
            ProjectProgressHelper helper = ProjectProgressHelper.GetInstance();
            ProjectManageHelper projectmanagehelper = ProjectManageHelper.GetInstance();
            IdentityHelper identityhelper = IdentityHelper.GetInstance();
            VolunteerHelper volunteerhelper = VolunteerHelper.GetInstance();
            #region 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);
            #endregion
            #region 创建一个project
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
                Creater = org
            };
            projectmanagehelper.CreatNewProject(org, DateTime.Now, name, "uibe", "nothing", 20);
            var addresult = database.Projects.Where(b => b.Name == name);
            if (addresult == null)
            {
                Assert.Fail("添加记录失败！");
            }
            #endregion
            #region 向项目中添加志愿者
            #region 创建两个志愿者
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
           database.Volunteers .Add(v1);  //志愿者1添加至数据库 志愿者2先进行无志愿者测试再添加进入数据库
            if (database.Volunteers.SingleOrDefault(v => v.StudentNum == studentnum1) == null)
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
            #endregion
            helper.SingleVolunteerInputById(studentnum1, pro);
            helper.SingleVolunteerInputById(studentnum2, pro);

            #endregion
            #region 测试FindSortedVolunteersByProject
           var volunteerlist  =   helper.FindSortedVolunteersByProject(pro);
            Volunteer testresult = volunteerlist.First();
            if ( testresult.StudentNum != studentnum1)
            {
                Assert.Fail("测试FindSortedVolunteersByProject失败！");
            }
            #endregion
            #region 删除数据库的有关数据[org pro vol]
            /*DeleteVolunteer(v1);
            DeleteVolunteer(v2);
            if (volunteerhelper.FindVolunteer(studentnum1) != null && volunteerhelper.FindVolunteer(studentnum2) != null)
            {
                Assert.Fail("删除志愿者失败！");
            }
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
            }*/
            #endregion
            //修改删除方法，删除方法中无法完全清空别处引用
        }

        [TestMethod()]
        public void FindVolunteerByIdTest()
        {
            ProjectProgressHelper helper = ProjectProgressHelper.GetInstance();
            ProjectManageHelper projectmanagehelper = ProjectManageHelper.GetInstance();
            IdentityHelper identityhelper = IdentityHelper.GetInstance();
            VolunteerHelper volunteerhelper = VolunteerHelper.GetInstance();
            #region 创建一个志愿者
            Random random = new Random();
            int studentnum = random.Next(000, 999);
            Volunteer v = new Volunteer
            {
                StudentNum = studentnum,
                Name = "FindVolunteerById",
                Class = "testclass",
                Room = "testroom",
                Mobile = "18888888888",
                Email="6666666666"
                
            };
            volunteerhelper.AddVolunteer(v);
            #endregion
            #region 测试 FindVolunteerById
           var result =  helper.FindVolunteerById(studentnum);
            var actual = volunteerhelper.FindVolunteer(studentnum);
            if ( result != actual )
            {
                Assert.Fail("FindVolunteerById测试失败！");
            }
            #endregion
            #region 删除数据库的有关数据[vol]
            DeleteVolunteer(result);
            if (volunteerhelper.FindVolunteer(studentnum) != null)
            {
                Assert.Fail("删除志愿者失败！");
            }
            #endregion
            //修改删除方法，删除方法中无法完全清空别处引用
        }

        [TestMethod()]
        public void SingleVolunteerInputByIdTest()
        {
            ProjectProgressHelper helper = ProjectProgressHelper.GetInstance();
            ProjectManageHelper projectmanagehelper = ProjectManageHelper.GetInstance();
            IdentityHelper identityhelper = IdentityHelper.GetInstance();
            VolunteerHelper volunteerhelper = VolunteerHelper.GetInstance();
            #region 创建两个志愿者
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
            volunteerhelper.AddVolunteer(studentnum1, "FindVolunteerById", "testclass", "18888888888");//志愿者1添加至数据库 志愿者2先进行无志愿者测试再添加进入数据库
            var addvolunteer1 = database.Volunteers.Where(v => v.StudentNum == v1.StudentNum).ToList();
            if (addvolunteer1.Count() == 0)
            {
                Assert.Fail("创建志愿者失败！");
            }
            Random random2 = new Random ( );
            int studentnum2 = random2.Next(000, 999);
            Volunteer v2 = new Volunteer
            {
                StudentNum = studentnum2,
                Name = "FindVolunteerById",
                Class = "testclass",
                Room = "testroom",
                Mobile = "18888888888"
            };
    
            #endregion
            #region 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);
            #endregion
            #region 创建一个project
            Guid uid = Guid.NewGuid();
            string name = "CreatVolunteerTest" + uid.ToString();
            Project pro = new Project
            {
                Name = name,
                Place = "uibe",
                Maximum = 1,
                Details = "nothing",
                Time = DateTime.Now,
                Condition = Interface.ProjectCondition.Ongoing,
                Creater = org
            };
            projectmanagehelper.CreatNewProject(org, DateTime.Now, name, "uibe", "nothing", 1);
            var addresult = database.Projects.Where(b => b.Name == name);
            if (addresult == null)
            {
                Assert.Fail("添加记录失败！");
            }
            #endregion
            #region 测试SingleVolunteerInputById
            ProgressResult result = helper.SingleVolunteerInputById(studentnum1, pro);
            if( ! result.Succeeded )
            {
                Assert.Fail("SingleVolunteerInputById方法结果返回失败！");
            }
            var actual = pro.Volunteer.Where(b => b.StudentNum == studentnum1).Count();
            if ( actual == 0)
            {
                Assert.Fail("测试SingleVolunteerInputById失败！未能成功添加志愿者！");
            }
            #endregion
            #region 测试error"志愿者不存在于数据库中"
            ProgressResult novolunteererro = helper.SingleVolunteerInputById(studentnum2, pro);
            if ( novolunteererro.Succeeded )
            {
                Assert.Fail("error返回结果异常！");
            }
            #endregion
            #region 测试error"已达项目人数上限，添加失败"
            volunteerhelper.AddVolunteer(v2);
            ProgressResult overmaximum = helper.SingleVolunteerInputById(studentnum2, pro);
            if( overmaximum.Succeeded )
            {
                Assert.Fail("error返回结果异常！");
            }
            #endregion
            #region 删除数据库的有关数据[org pro vol]
        /*    volunteerhelper.DeleteVolunteer(studentnum1);
            volunteerhelper.DeleteVolunteer(studentnum2);
            if (volunteerhelper.FindVolunteer(studentnum1) != null && volunteerhelper .FindVolunteer(studentnum2) != null)
            {
                Assert.Fail("删除志愿者失败！");
            }
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
            }*/
            #endregion
            //修改删除方法，删除方法中无法完全清空别处引用
        }

        [TestMethod()]
        public void DeleteVolunteerFromProjectTest()
        {
            ProjectProgressHelper helper = ProjectProgressHelper.GetInstance();
            ProjectManageHelper projectmanagehelper = ProjectManageHelper.GetInstance();
            IdentityHelper identityhelper = IdentityHelper.GetInstance();
            VolunteerHelper volunteerhelper = VolunteerHelper.GetInstance();
            #region 创建两个志愿者
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
            volunteerhelper.AddVolunteer(v1);  
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
            volunteerhelper.AddVolunteer(v2); 
            #endregion
            #region 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);
            #endregion
            #region 创建一个project
            Guid uid = Guid.NewGuid();
            string name = "CreatVolunteerTest" + uid.ToString();
            Project pro = new Project
            {
                Name = name,
                Place = "uibe",
                Maximum = 1,
                Details = "nothing",
                Time = DateTime.Now,
                Condition = Interface.ProjectCondition.Ongoing,
                Creater = org
            };
            projectmanagehelper.CreatNewProject(org, DateTime.Now, name, "uibe", "nothing", 1);
            var addresult = database.Projects.Where(b => b.Name == name);
            if (addresult == null)
            {
                Assert.Fail("添加记录失败！");
            }
            #endregion
            #region 利用SingleVolunteerInputById
            ProgressResult addvolunteerresult = helper.SingleVolunteerInputById(studentnum1, pro);
            if (!addvolunteerresult.Succeeded)
            {
                Assert.Fail("SingleVolunteerInputById方法结果返回失败！");
            }
            #endregion
            #region 测试DeleteVolunteerFromProject
            ProgressResult result = helper.DeleteVolunteerFromProject(v1, pro);
            if ( !result.Succeeded )
            {
                Assert.Fail("返回结果异常！");
            }
            var actual = pro.Volunteer.Where(b => b.StudentNum == v1.StudentNum).Count();
            if ( actual != 0 )
            {
                Assert.Fail("测试DeleteVolunteerFromProject失败！");
            }
            #endregion
            #region 测试error"志愿者不在该项目中"
            ProgressResult novolunteererro = helper.DeleteVolunteerFromProject(v2,pro);
            if (novolunteererro.Succeeded)
            {
                Assert.Fail("error返回结果异常！");
            }
            #endregion
            #region 测试error"已达项目人数上限，添加失败"
            ProgressResult overmaximum = helper.SingleVolunteerInputById(studentnum2, pro);
            if (overmaximum.Succeeded)
            {
                Assert.Fail("error返回结果异常！");
            }
            #endregion
            #region 删除数据库的有关数据[org pro vol]
           /* volunteerhelper.DeleteVolunteer(studentnum1);
            volunteerhelper.DeleteVolunteer(studentnum2);
            if (volunteerhelper.FindVolunteer(studentnum1) != null && volunteerhelper.FindVolunteer(studentnum2) != null)
            {
                Assert.Fail("删除志愿者失败！");
            }
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
            }*/
            #endregion
            //修改删除方法，删除方法中无法完全清空别处引用
        }

        [TestMethod()]
        public void Scoring4ForVolunteersTest()
        {
            ProjectProgressHelper helper = ProjectProgressHelper.GetInstance();
            ProjectManageHelper projectmanagehelper = ProjectManageHelper.GetInstance();
            IdentityHelper identityhelper = IdentityHelper.GetInstance();
            VolunteerHelper volunteerhelper = VolunteerHelper.GetInstance();
            #region 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);
            #endregion
            #region 创建一个project
            Guid uid = Guid.NewGuid();
            string name = "CreatVolunteerTest" + uid.ToString();
            Project pro = new Project
            {
                Name = name,
                Place = "uibe",
                Maximum = 1,
                Details = "nothing",
                Time = DateTime.Now,
                Condition = Interface.ProjectCondition.Ongoing,
                Creater = org
            };
            projectmanagehelper.CreatNewProject(org, DateTime.Now, name, "uibe", "nothing", 1);
            var addresult = database.Projects.Where(b => b.Name == name);
            if (addresult == null)
            {
                Assert.Fail("添加记录失败！");
            }
            #endregion
            #region 创建两个志愿者
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
            volunteerhelper.AddVolunteer(v1);
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
            volunteerhelper.AddVolunteer(v2);
            helper.SingleVolunteerInputById(studentnum1, pro);
            helper.SingleVolunteerInputById(studentnum2, pro);
            #endregion
            #region 测试Scoring4ForVolunteers
            ProgressResult result = helper.Scoring4ForVolunteers(pro);
            if ( !result.Succeeded && pro.ScoreCondition == Interface.ProjectScoreCondition.Scored )
            {
                Assert.Fail("结果返回异常！");
            }
            if ( v1.Score != 4 || v2.Score != 4 )
            {
                Assert.Fail("Scoring4ForVolunteers失败！");
            }
            #endregion 
            #region 删除数据库的有关数据[org pro vol]
          /*  DeleteVolunteer(v1);
            DeleteVolunteer(v2);
            if (volunteerhelper.FindVolunteer(studentnum1) != null && volunteerhelper.FindVolunteer(studentnum2) != null)
            {
                Assert.Fail("删除志愿者失败！");
            }
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
            }*/
            #endregion
            //修改删除方法，删除方法中无法完全清空别处引用

        }

        [TestMethod()]
        public void ScoreSingleVolunteerTest()
        {
            ProjectProgressHelper helper = ProjectProgressHelper.GetInstance();
            ProjectManageHelper projectmanagehelper = ProjectManageHelper.GetInstance();
            IdentityHelper identityhelper = IdentityHelper.GetInstance();
            VolunteerHelper volunteerhelper = VolunteerHelper.GetInstance();
            #region 创建一个志愿者
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
            #endregion
            #region 测试error
            ProgressResult result = helper.ScoreSingleVolunteer(0, v);
            if ( !result.Succeeded )
            {
                Assert.Fail("结果返回失败！");
            }
            #endregion
            #region 测试ScoreSingleVolunteer
            result = helper.ScoreSingleVolunteer(5, v);
            if ( !result.Succeeded )
            {
                Assert.Fail("添加失败！");
            }
            if ( v.Score != 1 )
            {
                Assert.Fail("测试ScoreSingleVolunteer失败！");
            }
            #endregion
            #region 删除数据库的有关数据[vol]
         /*   DeleteVolunteer(v);
            if (volunteerhelper.FindVolunteer(studentnum) != null)
            {
                Assert.Fail("删除志愿者失败！");
            }*/
            #endregion
            //修改删除方法，删除方法中无法完全清空别处引用
        }

        [TestMethod()]
        public void FinishProjectTest()
        {
            ProjectProgressHelper helper = ProjectProgressHelper.GetInstance();
            ProjectManageHelper projectmanagehelper = ProjectManageHelper.GetInstance();
            IdentityHelper identityhelper = IdentityHelper.GetInstance();
            VolunteerHelper volunteerhelper = VolunteerHelper.GetInstance();
            #region 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);
            #endregion
            #region 创建一个project
            Guid uid = Guid.NewGuid();
            string name = "CreatVolunteerTest" + uid.ToString();
            Project pro = new Project
            {
                Name = name,
                Place = "uibe",
                Maximum = 1,
                Details = "nothing",
                Time = DateTime.Now,
                Condition = Interface.ProjectCondition.Ongoing,
                Creater = org
            };
            projectmanagehelper.CreatNewProject(org, DateTime.Now, name, "uibe", "nothing", 1);
            var addresult = database.Projects.Where(b => b.Name == name);
            if (addresult == null)
            {
                Assert.Fail("添加记录失败！");
            }
            #endregion
            #region 测试finishproject
            ProgressResult result = helper.FinishProject(pro);
            if ( result.Succeeded )
            {
                Assert.Fail("在unscored条件下通过测试，失败！");
            }
            pro.ScoreCondition = Interface.ProjectScoreCondition.Scored;
            result = helper.FinishProject(pro);
            if ( !result.Succeeded )
            {
                Assert.Fail("方法返回失败结果！请检查重试！");
            }
            if (pro.Condition != Interface.ProjectCondition.Finished)
            {
                Assert.Fail("方法FinishProject测试失败！");
            }
            #endregion
            #region 删除数据库的有关数据[org pro vol]
           /* var deletepro = database.Projects.Where(p => p.Name == name);
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
            }*/
            #endregion
            //修改删除方法，删除方法中无法完全清空别处引用

        }

        public void DeleteOrgnization(Organization org)
        {

            var list = database.Users.Where(u => u.Organization.Id == org.Id).ToList();
            foreach (var item in list)
            {
                item.Organization = null;
            }
            var projectList = database.Projects.Where(p => p.Creater.Id == org.Id).ToList();
            foreach (var item in projectList)
            {
                item.Creater = null;
            }
            var blacklist = database.BlackListRecords.Where(u => u.Organization.Id == org.Id).ToList();
            foreach(var item in blacklist)
            {
                item.Organization = null;
            }
            //var orgInDb = database.Organizations.SingleOrDefault(o => o.Name == org.Name);
            database.Organizations.Remove(org);
            Save();
        }
        public void DeleteVolunteer(Volunteer vol)
        {
            //var list = database.BlackListRecords.Where(u => u.Volunteer.UID == vol.UID).ToList();
            //foreach (var item in list)
            //{
            //    item.Volunteer = null;
            //}
            lock (database)
            {
                vol.BlackListRecords.Clear();
                vol.Project.Clear();
                vol.TargetedBy.Clear();
                database.Volunteers.Remove(vol);
                Save();
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