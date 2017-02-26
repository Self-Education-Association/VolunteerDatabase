﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using System.Runtime.CompilerServices;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper
{
    /// <summary>
    /// 所有helper的基类,不允许被实例化.
    /// </summary>
    public abstract class BaseHelper
    {
        protected static LogHelper logger;
        public delegate bool SuccessEventHandler(string content, bool ispublic = false,LogType type = LogType.Default, AppUser targetuser = null, Volunteer targetvolunteer = null, Project targetproject = null,  [CallerMemberName] string caller = "");
        public delegate bool FailureEventHandler(string content, bool ispublic = false,LogType type = LogType.Default, AppUser targetuser = null, Volunteer targetvolunteer = null, Project targetproject = null,  [CallerMemberName] string caller = "");
        public event SuccessEventHandler Success;
        public event FailureEventHandler Failure;
        public bool OperationSucceeded(string content, bool ispublic = false, LogType type = LogType.Default, AppUser targetuser = null, Volunteer targetvolunteer = null, Project targetproject = null, [CallerMemberName] string caller = "")
        {
            if (caller == "")
            {
                return false;
            }
            else
            {
                Success(content: content, ispublic: ispublic, type:type, targetproject: targetproject, targetuser: targetuser, targetvolunteer: targetvolunteer);
                return true;
            }
        }//用bool便于debug

        public bool OperationFailed(string content, bool ispublic =false, LogType type = LogType.Default, AppUser targetuser = null, Volunteer targetvolunteer = null, Project targetproject = null, [CallerMemberName] string caller = "")
        {
            if(caller =="")
            {
                return false;
            }
            else
            {
                Failure(content: content, ispublic: ispublic, targetproject: targetproject, targetuser: targetuser, targetvolunteer: targetvolunteer);
                return true;
            }
        }

        public bool VolunteerOperationSucceeded(string content, Volunteer targetvolunteer, LogType type,bool ispublic = false, [CallerMemberName]string caller = "")
        {
            if(caller == "")
            {
                return false;
            }
            else
            {
                Success(content, ispublic, type, null, targetvolunteer, null);
                return true;
            }
        }

        public bool VolunteerOperationFailed(string content, Volunteer targetvolunteer, LogType type, bool ispublic = false, [CallerMemberName]string caller = "")
        {
            if (caller == "")
            {
                return false;
            }
            else
            {
                Failure(content, ispublic, type, null, targetvolunteer, null);
                return true;
            }
        }
    }
}
