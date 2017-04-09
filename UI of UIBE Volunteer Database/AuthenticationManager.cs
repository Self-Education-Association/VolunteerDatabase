using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Desktop.Pages;

namespace VolunteerDatabase.Desktop
{
    class AuthenticationManager
    {
        private static IPrincipal _principal;

        public static IPrincipal Principal
        {
            get
            {
                if (DateTime.Now > _expireTime)
                {
                    LogOut();
                    Login.GetWindow();
                }
                return _principal;
            }
        }

        private static DateTime _authenticateTime;

        private static DateTime _expireTime;

        public static void SignIn(IIdentity identity)
        {
            SignIn(new AuthenticationProperties { ExpireTimeSpan = AuthenticationProperties.DefaultExpireTimeSpan }, identity);
        }

        public static void SignIn(AuthenticationProperties properties, IIdentity identity)
        {
            _principal = new AppPrincipal(identity);
            _authenticateTime = DateTime.Now;
            _expireTime = _authenticateTime.Add(properties.ExpireTimeSpan);
            SendPrincipalEvent?.Invoke(Principal);
        }

        public static bool LogOut()
        {
            _principal = null;
            SendPrincipalEvent = null;
            LogOutEvent?.Invoke();
            LogOutEvent = null;
            return true;
        }

        public static void GetClaims(SendPrincipalDelegate sendClaims, LogOutDelegate logout)
        {
            if (_principal?.Identity.IsAuthenticated == true)
            {
                sendClaims(_principal);
                return;
            }
            SendPrincipalEvent += sendClaims;
        }

        public delegate void SendPrincipalDelegate(IPrincipal principal);

        public delegate void LogOutDelegate();

        public static event SendPrincipalDelegate SendPrincipalEvent;

        public static event LogOutDelegate LogOutEvent;
    }

    class AuthenticationProperties
    {
        public static readonly TimeSpan DefaultExpireTimeSpan = new TimeSpan(1, 0, 0, 0);

        private TimeSpan _expireTimeSpan = DefaultExpireTimeSpan;

        public TimeSpan ExpireTimeSpan
        {
            get => IsPersistent ? TimeSpan.MaxValue : _expireTimeSpan;
            set => _expireTimeSpan = value;
        }

        public bool IsPersistent;
    }
}
