using System;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using VolunteerDatabase.Helper;

namespace VolunteerDatabase.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
#if !DEBUG
            bool? result = new CertificateInstaller().InstallCertificate();
            if (result == true)
            {
                MessageBox.Show("证书安装成功！");
            }
#endif
            InitializeComponent();
        }
        private AppUserIdentityClaims Claims { get; set; }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }

}
