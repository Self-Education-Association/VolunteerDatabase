using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;
using System.Data.Entity.Infrastructure;

namespace VolunteerDatabase.Helper
{
    public class VolunteerHelper
    {
        //增删查改志愿者条目
        //查询某一志愿者
        private static VolunteerHelper helper;
        private static readonly object VolunteerLocker = new object();
        private static readonly object helperlocker = new object();
        private const string DEFAULTSTRING = "未填写";
        Database database;

        public static VolunteerHelper GetInstance()
        {
            if (helper == null)
            {
                lock (helperlocker)
                {
                    if (helper == null)
                    {
                        helper = new VolunteerHelper();
                    }
                }
            }
            return helper;
        }

        public async Task<VolunteerHelper> GetInstanceAsync()
        {
            Task<VolunteerHelper> helper = Task.Run(() =>
            {
                return GetInstance();
            });
            return await helper;
        }
        public VolunteerHelper()
        {
            database = DatabaseContext.GetInstance();
        }
        
        public Volunteer FindVolunteer(int num)
        {
            var result = database.Volunteers.SingleOrDefault( v => v.StudentNum == num);
            return result;
        }
        public List<Volunteer> FindVolunteer(string name)
        {
            var result = database.Volunteers.Where(v => v.Name == name).ToList();//返回一个集合
            return result;
        }
        //新建志愿者
        [AppAuthorize]
        public VolunteerResult AddVolunteer(Volunteer v)
        {
            if(v==null)
                return VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.NullVolunteerObject);
            if (v.StudentNum == 0)
                return VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.EmptyId);
            if (database.Volunteers.Where(o => o.StudentNum == v.StudentNum).Count() != 0)
                return VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.SameIdVolunteerExisted, v.StudentNum);

            else
            {
                v.Mobile = v.Mobile == null ? DEFAULTSTRING : v.Mobile;
                v.Room = v.Room == null ? DEFAULTSTRING : v.Room;
                v.Name = v.Name == null ? DEFAULTSTRING : v.Name;
                v.Class = v.Class == null ? DEFAULTSTRING : v.Class;
                v.Email = v.Email == null ? DEFAULTSTRING : v.Email;
                database.Volunteers.Add(v);
                Save();
                return VolunteerResult.Success();
            }
        }
        [AppAuthorize]
        public VolunteerResult AddVolunteer(int num,string name=DEFAULTSTRING,string c=DEFAULTSTRING,string mobile=DEFAULTSTRING,string room=DEFAULTSTRING,string email=DEFAULTSTRING)
        {
            if(num==0)
                return VolunteerResult.Error(VolunteerResult.EditVolunteerErrorEnum.EmptyId);
            var v = new Volunteer
            {
                StudentNum = num,
                Name = name,
                Class = c,
                Mobile = mobile,
                Room = room,
                Email = email
            };
            return AddVolunteer(v);
        }
        //编辑志愿者
        [AppAuthorize]
        public VolunteerResult EditVolunteer(Volunteer a,Volunteer b)
        {
            if (a == null || a.StudentNum == 0)
                return VolunteerResult.Error(VolunteerResult.EditVolunteerErrorEnum.NonExistingVolunteer);
            var v = database.Volunteers.SingleOrDefault(o => o.StudentNum == a.StudentNum);
            v.StudentNum=b.StudentNum;
            v.Name = b.Name;
            v.Class = b.Class;
            v.Mobile = b.Mobile;
            v.Room = b.Room;
            v.Email = b.Email;
            Save();
            return VolunteerResult.Success();
        }
        [AppAuthorize]
        public VolunteerResult EditVolunteer(Volunteer a, int num, string name = DEFAULTSTRING,string c= DEFAULTSTRING, string mobile = DEFAULTSTRING, string room = DEFAULTSTRING, string email = DEFAULTSTRING)
        {
            if (num == 0)
                return VolunteerResult.Error(VolunteerResult.EditVolunteerErrorEnum.EmptyId);
            var v = database.Volunteers.SingleOrDefault(o => o.StudentNum == a.StudentNum);
            v.StudentNum = num;
            v.Name = name==DEFAULTSTRING?v.Name:DEFAULTSTRING;
            v.Class = c==DEFAULTSTRING?v.Class:DEFAULTSTRING;
            v.Mobile = mobile==DEFAULTSTRING?v.Mobile:DEFAULTSTRING;
            v.Room = room==DEFAULTSTRING?v.Room:DEFAULTSTRING;
            v.Email = email==DEFAULTSTRING?v.Email:DEFAULTSTRING;
            Save();
            return VolunteerResult.Success();
        }
        //删除志愿者
        [AppAuthorize]
        public VolunteerResult DeleteVolunteer(Volunteer a)
        {
            if (a != null)
            {
                database.Volunteers.Remove(a);
                Save();
                return VolunteerResult.Success();
            }
            else
            {
                return VolunteerResult.Error(VolunteerResult.DeleteVolunteerErrorEnum.NonExistingVolunteer);
            }
        }
        [AppAuthorize]
        public VolunteerResult DeleteVolunteer(int num)
        {
            var v = database.Volunteers.SingleOrDefault(o => o.StudentNum == num);
            if (v != null)
            {
                database.Volunteers.Remove(v);
                Save();
                return VolunteerResult.Success();
            }
            else
            {
                return VolunteerResult.Error(VolunteerResult.DeleteVolunteerErrorEnum.NonExistingVolunteer);
            }
        }

        #region Save
        private void Save()
        {
            bool flag = false;
            do
            {
                try
                {
                    database.SaveChanges();
                    flag = false;
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
