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
        private Volunteer FindVolunteerByStuNum(Volunteer v)
        {
            Volunteer result = database.Volunteers.SingleOrDefault(o => o.StudentNum == v.StudentNum);
            return result;
        }
        private Volunteer FindVolunteerByStuNum(int num)
        {
            Volunteer result = database.Volunteers.SingleOrDefault(o => o.StudentNum == num);
            return result;
        }
        private Volunteer FindVolunteer(Volunteer v)
        {
            if (v.StudentNum == 0)
                return null;
            Volunteer result = FindVolunteerByStuNum(v.StudentNum);
            if (Volunteer.AreSame(v, result))
                return result;
            else return null;
        }

        [TestMethod()]
        public void AddVolunteerTest()
        {
            #region 插入第一个volunteer对象
            Volunteer v = new Volunteer
            {
                StudentNum = 888,
                Mobile = "13812345678",
                Name = "AddTest",
                Email="AddTest@test.com",
                Class="AddTestClass",
                Room="AddTestRoom"
            };
            var result = helper.AddVolunteer(v);
            if (VolunteerResult.AreSame(result,VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.SameIdVolunteerExisted,v.StudentNum)))
            {
                var existedvolunteer = FindVolunteerByStuNum(v);
                database.Volunteers.Remove(existedvolunteer);
                database.SaveChanges();
            }
            
            result = helper.AddVolunteer(v);
            var actual = FindVolunteerByStuNum(v.StudentNum);
            if (!Volunteer.AreSame(v, actual))
                Assert.Fail("插入第一个volunteer对象失败");
            //Assert.AreEqual(v, actual);
            if (!VolunteerResult.AreSame(result, VolunteerResult.Success()))
                Assert.Fail("插入第一个volunteer对象失败");
            #endregion

            #region 插入第二个volunteer对象
            int tempnum = 999;
            string tempname = "TestVolunteer2";
            result = helper.AddVolunteer(tempnum, tempname);
            if (VolunteerResult.AreSame(result, VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.SameIdVolunteerExisted,tempnum)))
            {
                var existedvolunteer = FindVolunteerByStuNum(tempnum);
                database.Volunteers.Remove(existedvolunteer);
                database.SaveChanges();
            }
            result = helper.AddVolunteer(tempnum, tempname);
            actual = FindVolunteerByStuNum(tempnum);
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
            if (!VolunteerResult.AreSame(result, VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.EmptyId)))
                Assert.Fail("插入的学号为0的对象未能正确处理.");
            #endregion

            #region 插入重复对象

            result = helper.AddVolunteer(v.StudentNum, "shadowman");
            if (!VolunteerResult.AreSame(VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.SameIdVolunteerExisted,v.StudentNum), result))
                Assert.Fail("插入的学号为0的对象未能正确处理.");
                
            #endregion
        }

        [TestMethod()]
        public void DeleteVolunteerTest()
        {
            #region 新建一个志愿者对象
            int tempnum = 8888;
            Volunteer v;
            if (FindVolunteerByStuNum(tempnum) == null)
            {
                v = new Volunteer
                {
                    StudentNum = tempnum,
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
                v = FindVolunteerByStuNum(tempnum);
            }
            #endregion

            #region 删除志愿者条目
            helper.DeleteVolunteer(v);
            var result = FindVolunteerByStuNum(tempnum);
            Assert.IsNull(result);
            #endregion

            #region 由id 删除志愿者条目
            helper.AddVolunteer(v);
            helper.DeleteVolunteer(tempnum);
            result = FindVolunteerByStuNum(tempnum);
            Assert.IsNull(result);
            #endregion

        }

        [TestMethod()]
        public void FindVolunteerTest()
        {
            #region 新建一个志愿者对象
            Volunteer v = new Volunteer
            {
                StudentNum = 0301,
                Name = "DeleteTest",
                Class = "TestClass",
                Email = "test@test.com",
                Room = "testroom",
                Mobile="18888888888"
            };
            helper.AddVolunteer(v);
            #endregion

            #region 用两种方式查找同一个志愿者
            var result1 = helper.FindVolunteer(v.StudentNum);
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
                StudentNum = 0401,
                Name = "EditTestName",
                Class = "TestClass",
                Email = "test@test.com",
                Room = "testroom"
            };
            helper.AddVolunteer(v);
            #endregion

            #region 第一次修改
            v.Name = "ModifiedEditTestName";
            var tempvolunteer = helper.FindVolunteer(v.StudentNum);
            helper.EditVolunteer(tempvolunteer, v);
            tempvolunteer = helper.FindVolunteer(v.StudentNum);
            if (!Volunteer.AreSame(v, tempvolunteer))
                Assert.Fail("修改志愿者信息失败");
            #endregion

            #region 第二次修改
            v.Name = "EditByParamTestName";
            helper.EditVolunteer(tempvolunteer, v.StudentNum, v.Name);
            tempvolunteer = helper.FindVolunteer(v.StudentNum);
            if (!Volunteer.AreSame(v, tempvolunteer))
                Assert.Fail("第二次修改志愿者信息失败");
            #endregion
        }

    }
}