using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Entity;

namespace HelperTests
{
    class UILogicTests
    {
        //为admin添加几个负责项目.
        ProjectManageHelper helper1 = ProjectManageHelper.GetInstance();
        ProjectProgressHelper helper2 = ProjectProgressHelper.GetInstance();
        
        [TestMethod()]
        public void addProjects()
        {

        }
    }
}
