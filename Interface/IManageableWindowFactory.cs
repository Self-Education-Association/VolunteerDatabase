using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VolunteerDatabase.Interface
{
    public interface IManageableWindowFactory<T> where T : Window, IManageableWindow
    {
        T GetInstance(IWindowsManager manager);
    }
}
