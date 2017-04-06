using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Desktop.ManagedControl
{
    interface IManageableControl
    {
        void LogOutHandler();

        void RefreshHandler();

        void MessageHandler(string message);
    }
}
