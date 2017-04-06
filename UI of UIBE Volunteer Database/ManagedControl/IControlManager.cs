using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Desktop.ManagedControl
{
    interface IControlManager
    {
        void SendLogOutEvent();

        void SendRefreshEvent();

        void SendMessageEvent(string message);

        void LoadControl(IManageableControl control);

        void UnloadControl(Type type, bool isClose);

        void UnloadAllControl(bool isClose);
    }

    public delegate void ControlLogOutDelegate();

    public delegate void ControlRefreshDelegate();

    public delegate void ControlMessageDelegate(string message);
}
