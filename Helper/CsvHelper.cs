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
        private VolunteerHelper vhelper;

        private CsvHelper()
        {
            database = DatabaseContext.GetInstance();
            vhelper = VolunteerHelper.GetInstance();//claims没有传
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

        public List<string> errorList = new List<string>();
        public List<string> informingMessage = new List<string>();
        public List<Volunteer> ChangedVols = new List<Volunteer>();
        public List<string> conflictNums = new List<string>();

        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        public List<Volunteer> PrepareAddInBatch(OpenFileDialog op, Project Pro)
        {
            op.Filter = "csv文件|*.csv";
            errorList.Clear();
            informingMessage.Clear();
            List<Volunteer> Temp = new List<Volunteer>();
            //文件读写异常处理
            FileStream fs;
            StreamReader sr;
            try
            {
                fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read, FileShare.None);//？
                sr = new StreamReader(fs, Encoding.GetEncoding(936));
            }
            catch(Exception e)
            {
                errorList.Add(e.Message);
                return null;
            }
            string str = "";
            string Title = sr.ReadLine();
            while (str != null)
            {
                str = sr.ReadLine();
                if (str == null)
                    break;
                try
                {
                    string[] eachLine = new string[7];
                    eachLine = str.Split(',');
                    string StudentNum = eachLine[0];
                    //long StudentNum = long.Parse(eachLine[0]);
                    string Name = eachLine[1];
                    string Mobile = eachLine[2];
                    string Email = eachLine[3];
                    string Room = eachLine[4];
                    string Class = eachLine[5];
                    string Skill = eachLine[6];
                    Volunteer v = vhelper.CreateTempVolunteer(StudentNum, Name, Class, Mobile, Room, Email, Skill);
                    Temp.Add(v);
                }
                catch (Exception e)
                {
                    informingMessage.Add("出现错误，"+e.Message);// throw;
                }
                
            }
            if (Temp.Count() == 0)
            {
                errorList.Add("导入的表中没有找到志愿者信息,请检查输入格式!");
            }
            //if (Temp.Count()>Pro.Maximum)
            //{
            //    informingMessage.Add("该次导入的人数超过上限!");
            //}
            return Temp;
        }

        public void ConfirmAddInBatch(List<Volunteer> temp, Project pro)
        {
            foreach (var item in temp)
            {
                var vol = database.Volunteers.SingleOrDefault(o => o.StudentNum == item.StudentNum);
                if (vol.Name == item.Name && vol.Class == item.Class && vol.Email == item.Email && vol.Mobile == item.Mobile)
                {
                    vol.Project.Add(pro);
                    continue;
                }
                else
                if (vol == null)
                {
                    //if (item.StudentNum > 2000000000000 || item.StudentNum < 201100000)
                    //{
                        errorList.Add("下面这些学号是不合法的,未予以导入,确认没有输错吗:" + item.StudentNum);
                    //}
                    //else
                    //{
                        VolunteerResult result = vhelper.AddVolunteer(item);
                        if (result.Succeeded == false)
                        {
                            informingMessage.Add("出现错误,错误信息[:" + result.ErrorString + "] 错误相关志愿者学号:[" + result.ErrorVolunteerNum + "]");
                        }
                    //}
                }
                else//重复且信息冲突
                {
                    string inform = String.Format(@"个人信息更改的志愿者 - 学号：{0}
                                                    原姓名：{1}                            现姓名{2}
                                                    原电话：{3}                            现电话{4}
                                                    原邮箱：{5}                            现邮箱{6}
                                                    原宿舍：{7}                            现宿舍{8}
                                                    原班级：{9}                            现班级{10}",
item.StudentNum, vol.Name, item.Name, vol.Mobile, item.Mobile, vol.Email, item.Email, vol.Room, item.Room, vol.Class, item.Class);
                    informingMessage.Add(inform);
                    ChangedVols.Add(item);//冲突的话 只是给一个提示
                }
            }
            informingMessage.Add("导入成功");
        }

        [AppAuthorize(AppRoleEnum.OrgnizationMember)]
        public void determChanges(params long[] StuNums)
        {
            foreach (var item in StuNums)
            {
                string itemString = item.ToString();
                var nv = ChangedVols.SingleOrDefault(o => o.StudentNum == itemString);
                if (nv != null)
                {
                    Volunteer ov = vhelper.FindVolunteer(itemString);
                    VolunteerResult result = vhelper.EditVolunteer(ov, nv);
                    if (result.Succeeded == false)
                    {
                        errorList.Add("出现错误,错误信息:[" + result.ErrorString + "] 错误相关志愿者学号:[" + result.ErrorVolunteerNum + "]");
                    }
                }
                else
                {
                    errorList.Add("学号:" + item + "的志愿者不存在于信息变动的志愿者列表中");
                }
            }
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

        #endregion 封装好的Save方法
    }
}