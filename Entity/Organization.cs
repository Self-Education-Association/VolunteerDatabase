using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Entity
{
    public class Organization
    {
        public int Id  { get; set; }//主键

        public string Name { get; set; }

        public OrganizationEnum OrganizationEnum { get; set; }

        public virtual List<AppUser> Members { get; set; }

        public virtual List<Project> Projects { get; set; }

        public virtual List<BlackListRecord> BlackListRecords { get; set; }
    }

    public enum OrganizationStatus
    {
        Enabled,
        Disabled
    }

    public enum OrganizationEnum
    {
        SEA团队,
        校志愿服务中心,
        国际经济贸易学院,
        金融学院,
        国际商学院,
        公共管理学院,
        法学院,
        中国语言文学学院,
        保险学院,
        信息学院,
        统计学院,
        TestOnly
    }
}
