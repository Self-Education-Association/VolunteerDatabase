using Microsoft.VisualStudio.TestTools.UnitTesting;
using VolunteerDatabase.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Helper.Tests
{
    [TestClass()]
    public class SecurityHelperTests
    {
        [TestMethod()]
        public void HashTest()
        {
            string test = "!@#$%^&*()_+-=1234567890qwertyuiop[]\\{}|asdfghjkl;':\"zxcvbnm,./<>?";
            var result = SecurityHelper.Hash(test);
            Assert.AreEqual(result, SecurityHelper.Hash(test));
        }

        [TestMethod()]
        public void GetSaltTest()
        {
            Assert.AreNotEqual(SecurityHelper.GetSalt(), SecurityHelper.GetSalt());
        }

        [TestMethod()]
        public void CheckPasswordTest()
        {
            var salt = SecurityHelper.GetSalt();
            var password = "!@#$%^&*()_+-=1234567890qwertyuiop[]\\{}|asdfghjkl;':\"zxcvbnm,./<>?";
            var hashedpassword = SecurityHelper.Hash(password + salt);
            Assert.IsTrue(SecurityHelper.CheckPassword(password, salt, hashedpassword));
        }
    }
}