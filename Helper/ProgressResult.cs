using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Helper
{
    public class ProgressResult
    {
        private string[] _errors;

        private bool _succeeded;

        public string[] Errors { get { return _errors; } }

        public bool Succeeded { get { return _succeeded; } }

        public static ProgressResult Error(params string[] errors)
        {
            if (errors.Count() == 0)
            {
                errors = new string[] { "未提供错误信息。" };
            }
            var result = new ProgressResult
            {
                _succeeded = false,
                _errors = errors
            };
            return result;
        }

        public static ProgressResult Error(ProgressErrorEnum error)
        {
            if(error==ProgressErrorEnum.BeyondMaxium)
            {
                return Error("已达项目人数上限，添加失败");
            }
            else
            {
                return Error();
            }
        }

        public static ProgressResult Success()
        {
            var result = new ProgressResult
            {
                _succeeded = true,
                _errors = { }
            };
            return result;
        }

        private ProgressResult() { }

        public enum ProgressErrorEnum
        {
            BeyondMaxium
        }
    }
}
