using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Desktop
{
    public class AppWindowsManager : IWindowsManager
    {
        private event WindowLogOutDelegate LogOutEvent;

        private event WindowRefreshDelegate RefreshEvent;

        private event WindowMessageDelegate MessageEvent;

        private readonly List<IManageableWindow> _windows = new List<IManageableWindow>();

        public void LoadForms(IManageableWindow window)
        {
            if (window is Window)
            {
                LogOutEvent += window.LogOutHandler;
                RefreshEvent += window.RefreshHandler;
                MessageEvent += window.MessageHandler;
                _windows.Add(window);
            }
            else
            {
                throw new InvalidOperationException($"{window.ToString()} is not a Window or Form");
            }
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

        public void UnloadAllForms(bool isClose = false)
        {
            foreach (var window in _windows)
            {
                UnloadForm(window);
                if (isClose)
                    CloseForm(window);
            }
        }

        public void UnloadForms(Type type, bool isClose = false)
        {
            foreach (var window in _windows)
            {
                if (!type.IsInstanceOfType(window)) continue;
                UnloadForm(window);
                if (isClose)
                    CloseForm(window);
            }
        }

        private void UnloadForm(IManageableWindow window)
        {
            LogOutEvent -= window.LogOutHandler;
            RefreshEvent -= window.RefreshHandler;
            MessageEvent -= window.MessageHandler;
            _windows.Remove(window);
        }

        private void CloseForm(IManageableWindow window)
        {
            if (window is Window)
                ((Window)window).Close();
        }
    }
}
