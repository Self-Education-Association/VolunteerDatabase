using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Desktop.ManagedControl
{
    class ControlManager : IControlManager
    {
        event ControlLogOutDelegate LogOutEvent;

        event ControlMessageDelegate MessageEvent;

        event ControlRefreshDelegate RefreshEvent;

        private readonly List<IManageableControl> _controls = new List<IManageableControl>();

        private static ControlManager _manager;

        private static readonly object ManagerLocker = new object();

        private ControlManager() { }

        public static ControlManager GetInstance()
        {
            if (_manager == null)
            {
                lock (ManagerLocker)
                {
                    if (_manager == null)
                    {
                        _manager = new ControlManager();
                    }
                }
            }

            return _manager;
        }

        public void LoadControl(IManageableControl control)
        {
            LogOutEvent += control.LogOutHandler;
            MessageEvent += control.MessageHandler;
            RefreshEvent += control.RefreshHandler;
            _controls.Add(control);
        }

        public void SendLogOutEvent()
        {
            LogOutEvent?.Invoke();
        }

        public void SendMessageEvent(string message)
        {
            MessageEvent?.Invoke(message);
        }

        public void SendRefreshEvent()
        {
            RefreshEvent?.Invoke();
        }

        public void UnloadAllControl(bool isClose)
        {
            foreach (var c in _controls)
            {
                UnloadControl(c);
            }
        }

        public void UnloadControl(Type type, bool isClose)
        {
            foreach (var c in _controls)
            {
                if (type.IsAssignableFrom(c.GetType()))
                {
                    UnloadControl(c);
                }
            }
        }

        public void UnloadControl(IManageableControl control)
        {
            if (_controls.Contains(control))
            {
                LogOutEvent -= control.LogOutHandler;
                MessageEvent -= control.MessageHandler;
                RefreshEvent -= control.RefreshHandler;
                _controls.Remove(control);
            }
        }
    }
}
