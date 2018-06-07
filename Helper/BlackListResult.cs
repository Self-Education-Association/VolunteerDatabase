using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper
{
    public class BlackListResult:IResult
    {
        private bool _succeed = false;
        private string[] _errors;
        private string _errorstring;
        public bool Succeeded { get { return _succeed; } }
        public string[] Errors { get { return _errors; } }
        public string ErrorString { get { return _errorstring; } }
        public static BlackListResult Success()
        {
            var result = new BlackListResult
            {
                _succeed = true,
                _errors = {},
            };
            return result;
        }

        public static BlackListResult Error(params string[] errors)
        {
            if (errors.Count() == 0)
            {
                errors = new string[] { "未提供错误信息。" };
            }
            var result = new BlackListResult
            {
                _succeed = false,
                _errors = errors,
                  _errorstring = errors.ToString()
            };
            return result;
        }
        public static BlackListResult Error(long id ,params string[] errors)
        {
            if (errors.Count() == 0)
            {
                errors = new string[] { "未提供错误信息。" };
            }
            var result = new BlackListResult
            {
                _succeed = false,
                _errors = errors,
                _errorstring = errors.ToString()
            };
            return result;
        }
        public static BlackListResult Error(AddBlackListRecordErrorEnum err)
        {
            BlackListResult result;
            switch (err)
            {
                case (AddBlackListRecordErrorEnum.NullRecord):
                    result = Error("欲添加的记录为空，请重新输入。");
                    break;
                case (AddBlackListRecordErrorEnum.ExistingRecord):
                    result = Error("已存在相同记录，请勿重复添加。");
                    break;
                case (AddBlackListRecordErrorEnum.WrongTime):
                    result= Error("填写时间不正确，请检查并稍后重试！");
                    break;
                default:
                    result = Error();
                    break;
            }
            return result;
        }
        public static BlackListResult Error(EditBlackListRecordErrorEnum err)
        {
          BlackListResult result;
            switch (err)
            {
                case (EditBlackListRecordErrorEnum.EmptyId):
                    result = Error("请输入Id！");
                    break;
                case (EditBlackListRecordErrorEnum.NoExistingRecord):
                    result = Error("欲编辑的志愿者记录不存在，请重新查询再试。");
                    break;
                default:
                    result = Error();
                    break;
            }
            return result;
        }
        public static BlackListResult Error(DeleteBlackListRecordErrorEnum err)
        {
            BlackListResult result;
            switch (err)
            {
                case (DeleteBlackListRecordErrorEnum.NonExistingRecord):
                    result = Error("欲删除的记录不存在！");
                    break;
                default:
                    result = Error();
                    break;
            }
            return result;
        }
        public static bool AreSame(BlackListResult a,BlackListResult b)
        {
            if (a == null && b == null)
                return true;
            if ((a == null && b != null) || (a != null && b == null))
                return false;
            else if (a.ErrorString == b.ErrorString && a.Succeeded == b.Succeeded)
            {
                    return true;
            }
            else
                return false;
        }
        public static List<long> CreateNumList(params long[] nums)
        {
            List<long> list = new List<long>();
            foreach (long n in nums)
            {
                list.Add(n);
            }
            return list;
        }
        #region 枚举类型的错误信息
        public enum AddBlackListRecordErrorEnum
        {
            NullRecord,
            ExistingRecord,
            WrongTime,
            NotAuthorized
        }
        public enum EditBlackListRecordErrorEnum
        {
            EmptyId,
            NoExistingRecord
        }
        public enum DeleteBlackListRecordErrorEnum
        {
            NonExistingRecord
        }
        #endregion
    }
}
