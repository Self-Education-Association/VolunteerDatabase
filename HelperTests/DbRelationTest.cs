using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VolunteerDatabase.Entity;
using System.Xml;
using System.Data.Entity.Infrastructure;

namespace HelperTests
{
    [TestClass]
    public class DbRelationTest
    {
        private Database database;

        [TestMethod]
        public void TestMethod1()
        {
                database = DatabaseContext.GetInstance();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(@"Model.edmx", settings))
                {
                    EdmxWriter.WriteEdmx(database, writer);
                }

        }
    }
}
