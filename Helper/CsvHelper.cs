using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using Microsoft.Win32;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;
using System.Data.Entity.Infrastructure;


namespace VolunteerDatabase.Helper
{
    public class CsvHelper
    {
        Database database;
        private CsvHelper()
        {
            database = DatabaseContext.GetInstance();
        }
        private static CsvHelper helper;
        private static readonly object helperlocker = new object();
        public static CsvHelper GetInstance()
        {
            if (helper == null)
            {
                lock (helperlocker)
                {
                    if (helper == null)
                    {
                        helper = new CsvHelper();
                    }
                }
            }
            return helper;
        }
        public async Task<CsvHelper> GetInstanceAsync()
        {
            Task<CsvHelper> helper = Task.Run(() =>
            {
                return GetInstance();
            });
            return await helper;
        }

        public List<string> errorList=new List<string>();
        public List<string> informingMessage=new List<string>();
        public static List<Volunteer> ChangedVols=new List<Volunteer>();

        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        public void MassiveVolunteersInput(OpenFileDialog op,Project Pro)
        {
            op.Filter="csv文件|*.csv";
            errorList.Clear();
            informingMessage.Clear();
            List<Volunteer> Temp = new List<Volunteer>();          
            FileStream fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding(936));

            string str = "";
            string Title = sr.ReadLine();
            while(str!=null)
                    {
                        str = sr.ReadLine();
                        if (str == null)
                            break;

                        string[] eachLine = new string[7];
                        eachLine = str.Split(',');
                        int StudentNum = int.Parse(eachLine[0]);
                        string Name = eachLine[1];
                        string Mobile = eachLine[2];
                        string Email = eachLine[3];
                        string Room = eachLine[4];
                        string Class = eachLine[5];
                        string Skill = eachLine[6];        
                
                        Volunteer v = new Volunteer();
                        v.StudentNum = StudentNum;
                        v.Name = Name;
                        v.Score = 0;
                        v.Mobile = Mobile;
                        v.Class = Class;
                        v.Email = Email;
                        v.Room = Room;
                        v.Skill = Skill;
                        v.BlackListRecords = null;
                        v.Project.Add(Pro);
                        Temp.Add(v);
                    }
            if(Temp.Count()>Pro.Maximum||Temp.Count()==0)
            {
                informingMessage.Add("该次导入的人数不合法");
            }
            else
            {
                foreach (var item in Temp)
                {
                    var vol = database.Volunteers.SingleOrDefault(o => o.StudentNum == item.StudentNum);
                    if (vol.Name == item.Name && vol.Class == item.Class && vol.Email == item.Email && vol.Mobile == item.Mobile)
                    {
                        vol.Project.Add(Pro);
                        continue;
                    }
                    else
                    if (vol == null)
                    {
                        if (item.StudentNum > 20500000 || item.StudentNum < 20110000)
                        {
                            errorList.Add("不合法的学号:" + item.StudentNum);
                        }
                        else
                        {
                            lock (database)
                            {
                                database.Volunteers.Add(item);
                                Save();
                            }
                        }
                    }
                    else
                    {
                        string inform = String.Format(@"个人信息更改的志愿者 - 学号：{0}        
                                                    原姓名：{1}                            现姓名{2}
                                                    原电话：{3}                            现电话{4}
                                                    原邮箱：{5}                            现邮箱{6}
                                                    原宿舍：{7}                            现宿舍{8}
                                                    原班级：{9}                            现班级{10}",
    item.StudentNum, vol.Name, item.Name, vol.Mobile, item.Mobile, vol.Email, item.Email, vol.Room, item.Room, vol.Class, item.Class);
                        informingMessage.Add(inform);
                        ChangedVols.Add(item);
                    }
                }
                informingMessage.Add("导入成功");
            }
        }

        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        public void determChanges(params int[] StuNums)
        {
            foreach (var item in StuNums)
            {
                var nv = ChangedVols.SingleOrDefault(o => o.StudentNum == item);
                if (nv != null)
                {
                    var ov = database.Volunteers.SingleOrDefault(i => i.StudentNum == item);
                    database.Volunteers.Remove(ov);
                    database.Volunteers.Add(nv);
                }
               else
                {                   
                    errorList.Add("学号:"+item+"的志愿者不存在于信息变动的志愿者列表中");
                }
            }
            Save();
            ChangedVols.Clear();
        }
      
        
        #region 封装好的Save方法
        public void Save()
        {
            bool flag = false;
            do
            {
                try
                {
                    database.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    flag = true;
                    foreach (var entity in database.ChangeTracker.Entries())
                    {
                        database.Entry(entity).Reload();
                        flag = false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            } while (flag);
        }
        #endregion
    }
}
