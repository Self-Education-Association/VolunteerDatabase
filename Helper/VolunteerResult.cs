using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Helper
{
    public class VolunteerResult
    {
        private bool _succeeded;
        private string[] _errors;
        public static VolunteerResult Error(params string[] errors)
        {
            if (errors.Count() == 0)
            {
                errors = new string[] { "未提供错误信息"};
            }
            var result = new VolunteerResult
            {
                _succeeded = false,
                _errors = errors
            };
            return result;
        }

        public static VolunteerResult Success()
        {
            var result = new VolunteerResult
            {
                _succeeded = true,
                _errors = { }
            };
            return result;
        }
        
    }
}
