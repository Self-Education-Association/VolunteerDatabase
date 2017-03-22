using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using VolunteerDatabase.Entity;

namespace VolunteerDatabase.Desktop
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var database = DatabaseContext.GetInstance();
            database.Users.ToList();
        }
    }
}
