using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Desktop
{
    public class AppManageableFormFactory<T> : IManageableWindowFactory<T> where T : Window, IManageableWindow, new()
    {
        private IWindowsManager _manager;

        public AppManageableFormFactory(IWindowsManager manager)
        {
            _manager = manager;
        }

        T IManageableWindowFactory<T>.GetInstance(IWindowsManager manager)
        {
            var form = new T();
            
            manager.LoadForms(form);
            return new T();
        }
    }
}
