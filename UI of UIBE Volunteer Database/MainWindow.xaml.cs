using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        string certificateStored = "SelfEducationAssociation.cer";
        public MainWindow()
        {
#if !DEBUG
            bool? result = new CertificateInstaller().InstallCertificate();
            if (result == true)
            {
                ModernDialog.ShowMessage("证书安装成功","提示信息",MessageBoxButton.OK);
            }
#endif
            InitializeComponent();
        }
        private AppUserIdentityClaims Claims { get; set; }

    }

}
