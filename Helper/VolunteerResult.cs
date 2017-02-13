using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper
{
    public class VolunteerResult:IResult
    {
        private bool _succeeded;
        private string[] _errors;
        private int _errorvolunteernum;
        private string _errorstring;
        public bool Succeeded { get { return _succeeded; } }
        public string[] Errors { get { return _errors; } }
        public string ErrorString { get { return _errorstring; } }
        public int ErrorVolunteerNum { get { return _errorvolunteernum; } }
        public static VolunteerResult Error(params string[] errors)
        {
            if (errors.Count() == 0)
            {
                errors = new string[] { "未提供错误信息"};
            }
            var result = new VolunteerResult
            {
                _succeeded = false,
                _errors = errors,
                _errorstring = errors.ToString()
            };
            return result;
        }
        public static VolunteerResult Error(string errors,int num)
        {
            
            if (errors.Count() == 0)
            {
                errors =  "未提供错误信息" ;
            }
            var result = new VolunteerResult
            {
                _succeeded = false,
                _errors = new string[] { errors },
                _errorvolunteernum = num,
                _errorstring = errors.ToString()
            };
            return result;
        }
        public static VolunteerResult Error(AddVolunteerErrorEnum err,int num=0)
        {
            VolunteerResult result;
            switch (err)
            {
                case (AddVolunteerErrorEnum.NullVolunteerObject):
                    result = Error("欲创建的志愿者对象为null!");
                    break;
                case (AddVolunteerErrorEnum.EmptyId):
                    result = Error("一个或多个志愿者学号未填写，请完整填写表单。");
                    break;
                case (AddVolunteerErrorEnum.SameIdVolunteerExisted):
                    if(num!=0)
                       result = Error("存在学号相同的志愿者，请确认输入是否有误。数据库中已存在的学号：" + num, num);
                    else
                       result = Error("存在学号相同的志愿者，请确认输入是否有误。");
                    break;
                default:
                    result = Error();
                    break;
            }
            return result;
        }

        public static VolunteerResult Error(EditVolunteerErrorEnum err)
        {
            VolunteerResult result;
            switch (err)
            {
                case (EditVolunteerErrorEnum.NonExistingVolunteer):
                    result = Error("欲修改的志愿者记录不存在，请重新查询再试。");
                    break;
                case (EditVolunteerErrorEnum.EmptyId):
                    result = Error("一个或多个志愿者学号未填写，请完整填写表单。");
                    break;
                default:
                    result = Error();
                    break;
            }
            return result;
        }
        public static VolunteerResult Error(DeleteVolunteerErrorEnum err)
        {
            VolunteerResult result;
            switch (err)
            {
                case (DeleteVolunteerErrorEnum.NonExistingVolunteer):
                    result = Error("欲删除的志愿者记录不存在，请重新查询再试。");
                    break;
                default:
                    result = Error();
                    break;
            }
            return result;
        }

        public static bool AreSame(VolunteerResult a,VolunteerResult b)
        {
            if (a == null && b == null)
                return true;
            if ((a == null && b != null) || (a != null && b == null))
                return false;
            else if (a.ErrorString == b.ErrorString && a.Succeeded == b.Succeeded)
            { 
                if(a.ErrorVolunteerNum==b.ErrorVolunteerNum)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public static List<int> CreateNumList(params int[] nums)
        {
            List<int> list = new List<int>();
            foreach (int n in nums)
            {
                list.Add(n);
            }
            return list;
        }
        public static VolunteerResult Success()
        {
            var result = new VolunteerResult
            {
                _succeeded = true,
                _errors = { },
                _errorstring = ""
            };
            return result;
        }
        #region 枚举类型的错误信息
        public enum AddVolunteerErrorEnum
        {
            NullVolunteerObject,
            EmptyId,
            SameIdVolunteerExisted
        }
        public enum EditVolunteerErrorEnum
        {
            NonExistingVolunteer,
            EmptyId
        }
        public enum DeleteVolunteerErrorEnum
        {
            NonExistingVolunteer
        }
        #endregion

        
    }
}
