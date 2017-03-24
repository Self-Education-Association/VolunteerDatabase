using System;
using System.Diagnostics;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;

namespace VolunteerDatabase.Desktop
{
    public class CertificateInstaller
    {
        private const string CertificateName = "Self Education Association";
        private const string CertificateStored = "SelfEducationAssociation.cer";
        public static bool? InstallCertificate()
        {
            var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.MaxAllowed);
            var certs = store.Certificates.Find(X509FindType.FindBySubjectName, CertificateName, false);
            if (certs.Count == 0)
            {
                return DoInstall();
            }
            var flag = false;
            foreach (var c in certs)
            {
                if (c.NotAfter > DateTime.Now)
                {
                    flag = true;
                }
            }
            if (!flag)
                return DoInstall();
            return null;
        }

        private static bool DoInstall()
        {
            if (!GetAdministratorPermission())
                return false;
            try
            {
                var certificate = new X509Certificate2(Environment.CurrentDirectory + "\\" + CertificateStored);
                var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                store.Add(certificate);
                store.Close();
            }
            catch (CryptographicException e)
            {
                ModernDialog.ShowMessage(e.Message, e.ToString(), MessageBoxButton.OK);
                return false;
            }
            return true;
        }

        private static bool GetAdministratorPermission()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                return true;
            }
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                Verb = "runas"
            };
            //设置启动动作,确保以管理员身份运行
            try
            {
                Process.Start(startInfo);
            }
            catch
            {
                Application.Current.Shutdown();
                return false;
            }
            Application.Current.Shutdown();
            return true;
        }
    }
}
