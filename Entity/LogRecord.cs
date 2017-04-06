using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Entity
{
    public class LogRecord : ILog<AppUser>
    {
        //用于人类阅读的日志
        //日志信息：添加者（组织）；添加内容（联系方式修改需要独立出来）；添加时间；与日志有关的操作对象；使用令牌（？）
        //读取日志时需通过令牌认证是否有权限读取
        //公开与否（需要令牌/不需令牌）
        public int Id { get; set; }

        public virtual AppUser Adder { get; set; }
        public Organization AddOrganization //不存进数据库 是从Adder里面派生的
            => Adder.Organization;

        [Required]
        public DateTime AddTime { get; set; }

        public bool IsPulblic { get; set; }

        public LogType Type { get { return (LogType)TypeNum; } set { TypeNum = (int)value; } }//enum其实是可以存进数据库的

        public int TypeNum { get; set; }

        [Required]
        public string Operation { get; set; }

        [Required]
        public string LogContent { get; set; }//只能用添加方法修改日志内容

        public Volunteer TargetVolunteer { get; set; }

        public AppUser TargetAppUser { get; set; }

        public Project TargetProject { get; set; }
        
    }
}
