using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;

namespace VolunteerDatabase.Helper
{
    public class IdentityResult
    {
        private string[] _errors;

        private bool _succeeded;

        public string[] Errors { get { return _errors; } }

        public bool Succeeded { get { return _succeeded; } }

        public static IdentityResult Error(params string[] errors)
        {
            if (errors.Count() == 0)
            {
                errors = new string[] { "未提供错误信息。" };
            }
            var result = new IdentityResult
            {
                _succeeded = false,
                _errors = errors
            };
            return result;
        }

        public static IdentityResult Success()
        {
            var result = new IdentityResult
            {
                _succeeded = true,
                _errors = { }
            };
            return result;
        }

        public bool AreSameWith(IdentityResult b)
        {
            if (this.Succeeded == b.Succeeded && this.Errors == b.Errors)
            {
                return true;
            }
            return false;
        }

        private IdentityResult() { }
    }
}
