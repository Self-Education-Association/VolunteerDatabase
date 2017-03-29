using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Interface
{
    public interface IWindowsManager
    {
        void SendLogOutEvent();

        void SendRefreshEvent();

        void SendMessageEvent(string message);

        void LoadForms(IManageableWindow form);

        void UnloadForms(Type type, bool isClose);

        void UnloadAllForms(bool isClose);
    }

    public delegate void WindowLogOutDelegate();

    public delegate void WindowRefreshDelegate();

    public delegate void WindowMessageDelegate(string message);
}
