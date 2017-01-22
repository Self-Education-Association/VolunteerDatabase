using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Helper
{
    public class BlackListResult
    {
        private bool _succeed = false;
        private string[] _errors;
        public bool Succeed { get { return _succeed; } }
        public string[] Errors { get { return _errors; } }
        
        public static BlackListResult Success()
        {
            var result = new BlackListResult
            {
                _succeed = true,
                _errors = {}
                 
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
    }
}
