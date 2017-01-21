﻿using System;
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
        private object VolunteerLocker = new object();
        Database database;
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
        public VolunteerResult AddVolunteer(Volunteer v)
        {
            if(v==null)
                return VolunteerResult.Error("欲创建的志愿者对象为null!");
            if (v.Id == 0)
                return VolunteerResult.Error("一个或多个志愿者学号未填写，请完整填写表单。");
            if (database.Volunteers.Where(o => o.Id == v.Id).Count() != 0)
                return VolunteerResult.Error("存在学号相同的志愿者，请确认输入是否有误。");
            database.Volunteers.Add(v);
            Save();
            return VolunteerResult.Success();
        }
        public VolunteerResult AddVolunteer(int id,string name="",string c="",string mobile="",string room="",string email="")
        {
            if(id==0)
                return VolunteerResult.Error("一个或多个志愿者学号未填写，请完整填写表单。");
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
        public VolunteerResult EditVolunteer(Volunteer a,Volunteer b)
        {
            if (a == null || a.Id == 0)
                return VolunteerResult.Error("欲修改的志愿者记录不存在，请重新查询再试。");
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
        public VolunteerResult EditVolunteer(Volunteer a, int id, string name = "",string c="", string mobile = "", string room = "", string email = "")
        {
            if (id == 0)
                return VolunteerResult.Error("一个或多个志愿者学号未填写，请完整填写表单。");
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
                return VolunteerResult.Error("删除失败，待删除的志愿者记录不存在！");
            }
        }

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
                return VolunteerResult.Error("删除失败，待删除的志愿者记录不存在！");
            }
        }

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
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            } while (flag);
        }


        
       

    }
}