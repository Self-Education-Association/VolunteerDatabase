using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Linq;
using System.Net.NetworkInformation;
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
            try
            {
               // Ping ping = new Ping();
               // PingReply pingr = ping.Send("10.1.1.68");
                //if(pingr.Status!=IPStatus.Success)
               // {
               //     ModernDialog.ShowMessage("您的网络存在问题，请检查网络状况，确保在校园网环境下使用", "网络问题", MessageBoxButton.OK);
               //     Application.Current.Shutdown();
               // }           
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
