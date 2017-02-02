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
    public class VolunteerHelperTests
    {
        VolunteerHelper helper = VolunteerHelper.GetInstance();
        [TestMethod()]
        public void GetInstanceTest()
        {
            Assert.IsInstanceOfType(helper, typeof(VolunteerHelper));
        }


    }
}