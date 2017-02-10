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
    public class VolunteerHelperTests
    {
        Database database = new Database();
        VolunteerHelper helper = VolunteerHelper.GetInstance();
        [TestMethod()]
        public void GetInstanceTest()
        {
            Assert.IsInstanceOfType(helper, typeof(VolunteerHelper));
        }

        [TestMethod()]
        public void AddVolunteerTest()
        {
            #region 插入第一个volunteer对象
            Volunteer v = new Volunteer
            {
                Id = 888,
                Mobile = "13812345678",
                Name = "AddTest",
                Email="AddTest@test.com",
                Class="AddTestClass",
                Room="AddTestRoom"
            };
            var result = helper.AddVolunteer(v);
            if (result == VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.SameIdVolunteerExisted))
            {
                var existedvolunteer = database.Volunteers.Find(v.Id);
                database.Volunteers.Remove(existedvolunteer);
                database.SaveChanges();
            }
            result = helper.AddVolunteer(v);
            var actual = database.Volunteers.Find(v.Id);
            if (!Volunteer.AreSame(v, actual))
                Assert.Fail("插入第一个volunteer对象失败");
            //Assert.AreEqual(v, actual);
            if (!VolunteerResult.AreSame(result, VolunteerResult.Success()))
                Assert.Fail("插入第一个volunteer对象失败");
            #endregion

            #region 插入第二个volunteer对象
            int tempid = 999;
            string tempname = "TestVolunteer2";
            result = helper.AddVolunteer(tempid, tempname);
            if (result == VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.SameIdVolunteerExisted))
            {
                var existedvolunteer = database.Volunteers.Find(tempid);
                database.Volunteers.Remove(existedvolunteer);
                database.SaveChanges();
            }
            result = helper.AddVolunteer(tempid, tempname);
            actual = database.Volunteers.Find(tempid);
            if (!VolunteerResult.AreSame(result, VolunteerResult.Success()))
                Assert.Fail("插入第二个volunteer对象失败.");
            if (actual == null)
            {
                Assert.Fail();
            }
            #endregion

            #region 插入null志愿者对象
            result = helper.AddVolunteer(null);
            if (!VolunteerResult.AreSame(result, VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.NullVolunteerObject)))
                Assert.Fail("插入的null对象未能正确处理.");
            #endregion

            #region 插入学号为0的志愿者对象
            result = helper.AddVolunteer(0);
            Assert.AreEqual(result, VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.EmptyId));
            if (!VolunteerResult.AreSame(result, VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.EmptyId)))
                Assert.Fail("插入的学号为0的对象未能正确处理.");
            #endregion

            #region 插入重复对象

            result = helper.AddVolunteer(v.Id, "shadowman");
            Assert.AreEqual(VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.SameIdVolunteerExisted), result);
            if (!VolunteerResult.AreSame(VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.SameIdVolunteerExisted), result))
                Assert.Fail("插入的学号为0的对象未能正确处理.");

            #endregion
        }

        [TestMethod()]
        public void DeleteVolunteerTest()
        {
            #region 新建一个志愿者对象
            int tempid = 8888;
            Volunteer v;
            if (database.Volunteers.Find(tempid) == null)
            {
                v = new Volunteer
                {
                    Id = tempid,
                    Name = "DeleteTest",
                    Class = "TestClass",
                    Email = "test@test.com",
                    Room = "testroom",
                    Mobile = "testmobilenumber"
                };
                helper.AddVolunteer(v);
            }
            else
            {
                v = database.Volunteers.Find(tempid);
            }
            #endregion

            #region 删除志愿者条目
            helper.DeleteVolunteer(v);
            var result = database.Volunteers.Find(tempid);
            Assert.IsNull(result);
            #endregion

            #region 由id 删除志愿者条目
            helper.AddVolunteer(v);
            helper.DeleteVolunteer(tempid);
            result = database.Volunteers.Find(tempid);
            Assert.IsNull(result);
            #endregion

        }

        [TestMethod()]
        public void FindVolunteerTest()
        {
            #region 新建一个志愿者对象
            Volunteer v = new Volunteer
            {
                Id = 0301,
                Name = "DeleteTest",
                Class = "TestClass",
                Email = "test@test.com",
                Room = "testroom",
                Mobile="18888888888"
            };
            helper.AddVolunteer(v);
            #endregion

            #region 用两种方式查找同一个志愿者
            var result1 = helper.FindVolunteer(v.Id);
            var result2 = helper.FindVolunteer(v.Name);
            if (!Volunteer.AreSame(v, result1))
                Assert.Fail("通过id查找志愿者失败");
            if (!result2.Contains(v))
                Assert.Fail("通过Name查找志愿者失败");
            #endregion
        }

        [TestMethod()]
        public void EditVolunteerTest()
        {
            #region 新建一个志愿者对象
            Volunteer v = new Volunteer
            {
                Id = 0401,
                Name = "EditTestName",
                Class = "TestClass",
                Email = "test@test.com",
                Room = "testroom"
            };
            helper.AddVolunteer(v);
            #endregion

            #region 第一次修改
            v.Name = "ModifiedEditTestName";
            var tempvolunteer = helper.FindVolunteer(v.Id);
            helper.EditVolunteer(tempvolunteer, v);
            tempvolunteer = helper.FindVolunteer(v.Id);
            if (!Volunteer.AreSame(v, tempvolunteer))
                Assert.Fail("修改志愿者信息失败");
            #endregion

            #region 第二次修改
            v.Name = "EditByParamTestName";
            helper.EditVolunteer(tempvolunteer, v.Id, v.Name);
            tempvolunteer = helper.FindVolunteer(v.Id);
            if (!Volunteer.AreSame(v, tempvolunteer))
                Assert.Fail("第二次修改志愿者信息失败");
            #endregion
        }

    }
}