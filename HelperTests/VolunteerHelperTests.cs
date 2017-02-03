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
                Name = "TestVolunteer"
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
            Assert.AreSame(v, actual);
            Assert.AreSame(result, VolunteerResult.Success());
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
            Assert.AreSame(result, VolunteerResult.Success());
            if (actual == null)
            {
                Assert.Fail();
            }
            #endregion

            #region 插入null志愿者对象
            result = helper.AddVolunteer(null);
            Assert.AreSame(result, VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.NullVolunteerObject));
            #endregion

            #region 插入学号为0的志愿者对象
            result = helper.AddVolunteer(0);
            Assert.AreSame(result, VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.EmptyId));
            #endregion

            #region 插入重复对象

            result = helper.AddVolunteer(v.Id, "shadowman");
            Assert.AreSame(VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.SameIdVolunteerExisted), result);
            #endregion
        }

        [TestMethod()]
        public void DeleteVolunteerTest()
        {
            int tempid = 8888;
            Volunteer v;
            if (database.Volunteers.Find(tempid) != null)
            {
                v = new Volunteer
                {
                    Id = 8888,
                    Name = "DeleteTest"
                };
                helper.AddVolunteer(v);
             }
            else
            {
                v = database.Volunteers.Find(tempid);
            }
            helper.DeleteVolunteer(v);
        }
    }
}