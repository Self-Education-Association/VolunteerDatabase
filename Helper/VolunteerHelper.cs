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
        
        public Volunteer FindVolunteer(int id)
        {
            var result = database.Volunteers.SingleOrDefault( v => v.Id == id);
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
            if (v.Id == 0)
                return VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.EmptyId);
            if (database.Volunteers.Where(o => o.Id == v.Id).Count() != 0)
                return VolunteerResult.Error(VolunteerResult.AddVolunteerErrorEnum.SameIdVolunteerExisted);
            database.Volunteers.Add(v);
            Save();
            return VolunteerResult.Success();
        }
        [AppAuthorize]
        public VolunteerResult AddVolunteer(int id,string name="",string c="",string mobile="",string room="",string email="")
        {
            if(id==0)
                return VolunteerResult.Error(VolunteerResult.EditVolunteerErrorEnum.EmptyId);
            var v = new Volunteer
            {
                Id = id,
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
            if (a == null || a.Id == 0)
                return VolunteerResult.Error(VolunteerResult.EditVolunteerErrorEnum.NonExistingVolunteer);
            var v = database.Volunteers.SingleOrDefault(o => o.Id == a.Id);
            v.Id=b.Id;
            v.Name = b.Name;
            v.Class = b.Class;
            v.Mobile = b.Mobile;
            v.Room = b.Room;
            v.Email = b.Email;
            Save();
            return VolunteerResult.Success();
        }
        [AppAuthorize]
        public VolunteerResult EditVolunteer(Volunteer a, int id, string name = "",string c="", string mobile = "", string room = "", string email = "")
        {
            if (id == 0)
                return VolunteerResult.Error(VolunteerResult.EditVolunteerErrorEnum.NonExistingVolunteer);
            var v = database.Volunteers.SingleOrDefault(o => o.Id == a.Id);
            v.Id = id;
            v.Name = name;
            v.Class = c;
            v.Mobile = mobile;
            v.Room = room;
            v.Email = email;
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
        public VolunteerResult DeleteVolunteer(int id)
        {
            var v = database.Volunteers.SingleOrDefault(o => o.Id == id);
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
