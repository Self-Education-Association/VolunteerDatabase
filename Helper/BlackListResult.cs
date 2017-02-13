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
        private int _errorvolunteerid;
        public bool Succeeded { get { return _succeed; } }
        public string[] Errors { get { return _errors; } }
        public int ErrorVolunteerId { get { return _errorvolunteerid; } }
        public static BlackListResult Success()
        {
            var result = new BlackListResult
            {
                _succeed = true,
                _errors = {},
                _errorvolunteerid = 0
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
                _errors = errors
            };
            return result;
        }
        public static BlackListResult Error(int id ,params string[] errors)
        {
            if (errors.Count() == 0)
            {
                errors = new string[] { "未提供错误信息。" };
            }
            var result = new BlackListResult
            {
                _succeed = false,
                _errors = errors,
                _errorvolunteerid = id
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
        #region 枚举类型的错误信息
        public enum AddBlackListRecordErrorEnum
        {
            NullRecord,
            ExistingRecord,
            WrongTime
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
