using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;

namespace VolunteerDatabase.Desktop.Pages
{
    public class BlackListViewModel
    {
        public BlackListRecord Record { get; private set; }
        public string Name { get { return Record.Volunteer.Name; } }
        public long StuNum { get { return Record.Volunteer.StudentNum; } }
        public string AddTime { get { return Record.AddTime.ToShortDateString(); } }
        public string Adder { get { return Record.Adder.Name; } }//只给管理员显示。用Attribute?
        public string Project { get { return Record.Project.Name; } }
        public string Organization { get { return Record.Organization.Name; } }
        public string Detail { get {return Record.Detail; } }
        public BlackListViewModel(BlackListRecord b)
        {
            this.Record = b;
        }
        
    }
}
