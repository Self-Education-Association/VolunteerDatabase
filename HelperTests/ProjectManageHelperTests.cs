﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace VolunteerDatabase.Helper.Tests
{
    [TestClass()]
    public class ProjectManageHelperTests
    {
        ProjectManageHelper helper = ProjectManageHelper.GetInstance();
        IdentityHelper identityhelper = IdentityHelper.GetInstance();
        Database database = DatabaseContext.GetInstance();

        [TestMethod()]
        public void GetInstanceTest()
        {
            Assert.IsInstanceOfType(helper, typeof(ProjectManageHelper));
        }

        [TestMethod()]
        public void ShowProjectListTest()
        {
            // 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);
            // 创建两个project
            Guid uid = Guid.NewGuid();
            string name1 = "testcreatafirstproject" + uid.ToString();
            string name2 = "testcreatasecondproject" + uid.ToString();
            ProgressResult addresult1 = helper.CreatNewProject(org, DateTime.Now, name1, "uibe", "nothing", 20);
            ProgressResult addresult2 = helper.CreatNewProject(org, DateTime.Now, name2, "uibe", "it's a test", 30);
            if (!addresult1.Succeeded || !addresult2.Succeeded)
            {
                Assert.Fail("添加记录失败！");
            }

            // 测试ShowProjectList
            var result = helper.ShowProjectList(org, true);
            var actual = database.Projects.Where(b => b.Organization.Id == org.Id).ToList();
            int resultcout = result.Count();
            int actualcount = actual.Count();
            if (result.Count() == 0 && actual.Count() == 0)
            {
                Assert.Fail("记录可能为空，为查询到有关记录！");
            }
            if (resultcout != actualcount)
            {
                Assert.Fail("未成功调用showlist方法查询到有关数据！showlist失败！");
            }

            // 删除数据库的有关数据[org pro]
            DeleteOrgnization(org);
        }           

        [TestMethod()]
        public void FindManagerListByStudentNumTest()
        {
            // 创建一个originalmanager
            Organization org = identityhelper.CreateOrFindOrganization(OrganizationEnum.TestOnly);
            Guid tempuser = Guid.NewGuid();
            string username = tempuser.ToString();
            Random tempusernum = new Random();
            int userstudentnum = tempusernum.Next(000, 999);
            AppUser originalmanager = new AppUser()
            {
                AccountName = username,
                StudentNum = userstudentnum,
                Mobile = "1234567890",
                Email = "test@test.com"
            };
            identityhelper.CreateUser(originalmanager, "zxcvbnm,./", Interface.AppRoleEnum.OrgnizationMember, OrganizationEnum.TestOnly);
            var addmanager = database.Users.Single(u => u.AccountName == originalmanager.AccountName);
            if (addmanager == null)
            {
                Assert.Fail("添加manager失败！");
            }

            // 测试FindManageListByStudentNum
            var result = helper.FindManagerListByStudentNum(originalmanager.StudentNum).ToList();
            if (result == null)
            {
                Assert.Fail("未能成功找到managelist，方法调用失败！");
            }
            else
            {
                foreach (var item in result)
                {
                    if ((item.AccountName != originalmanager.AccountName) && (item.Mobile != originalmanager.Mobile))
                    {
                        Assert.Fail("所查找记录与原纪录不匹配！");
                    }
                }
            }
            // 删除数据库中添加的数据[users]
            DeleteOrgnization(org);

        }

        [TestMethod()]
        public void CreatNewProjectTest()
        {
            // 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);

            // 测试CreateNewProject
            //创建一个pro
            Guid tempname = Guid.NewGuid();
            string projectname = tempname.ToString();
            ProgressResult result = helper.CreatNewProject(org, DateTime.Now, projectname, "uibe", "nothing", 20);
            if (!result.Succeeded)
            {
                Assert.Fail();
            }
            var actual = database.Projects.Where(b => b.Name.ToString() == projectname).ToList();
            if (actual == null)
            {
                Assert.Fail("CreatNewProject方法失败！无法查找到添加记录！");
            }

            // 删除数据库中添加的数据[org pro]
            DeleteOrgnization(org);
        }

        [TestMethod()]
        public void ProjectMessageInputTest()
        {
            // 创建一个manager
            Guid tempuser = Guid.NewGuid();
            string username = tempuser.ToString();
            Random tempusernum = new Random();
            int userstudentnum = tempusernum.Next(000, 999);
            AppUser manager = new AppUser()
            {
                AccountName = username,
                StudentNum = userstudentnum,
                Mobile = "1234567890",
                Email = "test@test.com"
            };
            identityhelper.CreateUser(manager, "zxcvbnm,./", Interface.AppRoleEnum.OrgnizationMember, OrganizationEnum.TestOnly);
            var addmanager = database.Users.Where(u => u.AccountName == manager.AccountName).ToList();
            if (addmanager == null)
            {
                Assert.Fail("添加manager失败！");
            }

            // 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);

            // 创建一个project
            Guid uid = Guid.NewGuid();
            string uiniqueName = "pro" + uid.ToString();
            Project pro = new Project
            {
                Name = uiniqueName,
                Place = "pro",
                Maximum = 20,
                Details = "nothing",
                Time = DateTime.Now,
                Condition = Interface.ProjectCondition.Ongoing,
                Organization = org
            };
            helper.CreatNewProject(org, DateTime.Now, uiniqueName, "uibe", "nothing", 20);
            var addresult = database.Projects.Where(b => b.Name.ToString() == uiniqueName);
            if (addresult == null)
            {
                Assert.Fail("添加记录失败！");
            }

            string name = "projectmassageinput";
            string detail = "jest a test";
            string place = "testplace";
            var promanager = database.Users.Where(m => m.StudentNum == manager.StudentNum).ToList();
            ProgressResult result = helper.ProjectMessageInput(name, detail, place, 20, DateTime.Now, promanager, pro);
            // 测试  ProgressResult.Error信息不完整 --因无法传入空参数，因此无法进行此处测试
            //result = helper.ProjectMessageInput(name, "", place, 0, null, promanager, pro);
            //if (result.Succeeded)
            //{
            //    Assert.Fail("无法判断空字段异常");
            //}

            // 测试ProjectMessageInput

            if (!result.Succeeded)
            {
                Assert.Fail("ProjectMessageInput结果返回失败！");
            }
            var actualresult = database.Projects.Where(p => p.Name == name).ToList();
            if (actualresult == null)
            {
                Assert.Fail("数据库中无法查找到相关数据，projectmessage添加失败！");
            }
            else
            {
                foreach (var item in actualresult)
                {
                    if (item.Name == pro.Name || item.Place == pro.Place || item.Details == pro.Details)
                    {
                        Assert.Fail("ProjectMessageInput 修改信息失败！");
                    }
                    else
                    {
                        database.Projects.Remove(item);
                        database.SaveChanges();
                    }
                }
            }

            // 删除数据库中添加的数据[users org pro]
            DeleteOrgnization(org);

        }


        [TestMethod()]
        public void ProjectDeleteTest()
        {
            // 创建org
            Organization org = identityhelper.CreateOrFindOrganization(Entity.OrganizationEnum.TestOnly);

            // 创建一个OnGoingProject
            Guid uid = Guid.NewGuid();
            string uiniqueName = "pro" + uid.ToString();
            Project pro = new Project
            {
                Name = uiniqueName,
                Place = "uibe",
                Maximum = 20,
                Details = "nothing",
                Time = DateTime.Now,
                Condition = Interface.ProjectCondition.Ongoing,
                Organization = org
            };
            helper.CreatNewProject(org, DateTime.Now, uiniqueName, "uibe", "nothing", 20);
            var addresult = database.Projects.SingleOrDefault(b => b.Name == uiniqueName);
            if (addresult == null)
            {
                Assert.Fail("添加记录失败！数据库无数据！");
            }

            // 测试ProjecDeleteTest
            var result = helper.ProjectDelete(addresult);
            if (!result.Succeeded)
            {
                Assert.Fail("未返回success结果！");
            }
            else
            {
                var actual = database.Projects.Where(b => b.Id == addresult.Id).Count();
                if (actual >= 1)
                {
                    Assert.Fail("记录删除失败！记录依旧存在！");
                }
            }
            var deleteorg = database.Organizations.Where(o => o.Id == org.Id).ToList();
            if (deleteorg != null)
            {
                foreach (var item in deleteorg)
                {
                    DeleteOrgnization(item);
                }
            }
            database.SaveChanges();

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
            Guid tempuser = Guid.NewGuid();
            string username = tempuser.ToString();
            Random tempusernum = new Random();
            int userstudentnum = tempusernum.Next(000, 999);
            AppUser user = new AppUser()
            {
                AccountName = username,
                StudentNum = userstudentnum,
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