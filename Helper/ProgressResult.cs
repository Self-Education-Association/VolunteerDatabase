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

        public string[] Errors => _errors;

        public bool Succeeded => _succeeded;

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
    }
}
