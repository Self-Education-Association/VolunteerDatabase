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
        private Database database;
        public List<string> errorList;
        public List<string> informingMessage;
        public static List<Volunteer> ChangedVols;

        public void MassiveVolunteersInput(OpenFileDialog op,Project Pro)
        {
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

                        string[] eachLine = new string[6];
                        eachLine = str.Split(',');
                        int StudentNum = int.Parse(eachLine[0]);
                        string Name = eachLine[1];
                        string Mobile = eachLine[2];
                        string Email = eachLine[3];
                        string Room = eachLine[4];
                        string Class = eachLine[5];            
                
                        Volunteer v = new Volunteer();
                        v.StudentNum = StudentNum;
                        v.Name = Name;
                        v.Score = 0;
                        v.Mobile = Mobile;
                        v.Class = Class;
                        v.Email = Email;
                        v.Room = Room;
                        v.BlackListRecords = null;
                        v.Project.Add(Pro);
                        Temp.Add(v);
                     }
            foreach (var item in Temp)
            {
                var vol = database.Volunteers.SingleOrDefault(o => o.StudentNum == item.StudentNum);
                if (vol.Equals(item))
                {
                    continue;
                }
                else
                if (vol == null)
                {
                    if (item.StudentNum > 20500000 || item.StudentNum < 20110000)
                    {
                        string error = string.Format("不合法的学号{0}", item.StudentNum);
                        errorList.Add(error);
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
       item.StudentNum,vol.Name,item.Name,vol.Mobile,item.Mobile,vol.Email,item.Email,vol.Room,item.Room,vol.Class,item.Class);
                    informingMessage.Add(inform);
                    ChangedVols.Add(item);
                }               
            }
        }
        public void determChanges(int[]UsingNewInformation)
        {
            foreach (var item in UsingNewInformation)
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
                    string err = string.Format("学号{0}的志愿者不存在于信息变动的志愿者列表中", item);
                    errorList.Add(err);
                }
            }
            Save();
            ChangedVols.Clear();
        }
        #region 封装好的Save方法
        private void Save()
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
