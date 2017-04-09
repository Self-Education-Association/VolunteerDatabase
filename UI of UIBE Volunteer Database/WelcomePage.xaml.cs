using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows.Controls;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Desktop
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Window
    {
        

        protected WelcomePage()
        {
            InitializeComponent();
        }

        

        public static bool LogOut()
        {
            _claimsStored = null;
            SendClaimsEvent = null;
            LogOutEvent?.Invoke();
            LogOutEvent = null;
            return true;
        }


        public static void GetClaims(SendClaimsDelegate sendClaims, LogOutDelegate logout)
        {
            if (_claimsStored?.IsAuthenticated == true)
            {
                sendClaims(_claimsStored);
                return;
            }

            WelcomePage loginWindow = GetWindow();
            loginWindow.Show();
            SendClaimsEvent += sendClaims;
            return;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_claimsStored == null)
                Application.Current.Shutdown();
            e.Cancel = true;
            Hide();
        }

        public delegate void SendClaimsDelegate(AppUserIdentityClaims claims);

        public delegate void LogOutDelegate();

        public static event SendClaimsDelegate SendClaimsEvent;

        public static event LogOutDelegate LogOutEvent;

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
