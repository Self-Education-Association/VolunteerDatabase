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
        const string CertificateName = "Self Education Association";
        const string CertificateStored = "SelfEducationAssociation.cer";
        public bool? InstallCertificate()
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.MaxAllowed);
            X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectName, CertificateName, false);
            if (certs.Count == 0)
            {
                return doInstall();
            }
            bool flag = false;
            foreach (var c in certs)
            {
                if (c.NotAfter > DateTime.Now)
                {
                    flag = true;
                }
            }
            if (!flag)
                return doInstall();
            return null;
        }

        bool doInstall()
        {
            if (!getAdministratorPermission())
                return false;
            try
            {
                X509Certificate2 certificate = new X509Certificate2(Environment.CurrentDirectory + "\\" + CertificateStored);
                X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
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

        bool getAdministratorPermission()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                return true;
            }
            else
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
                //设置启动动作,确保以管理员身份运行
                startInfo.Verb = "runas";
                try
                {
                    Process.Start(startInfo);
                }
                catch
                {
                    Application.Current.Shutdown();
                    return false;
                }
            }
            Application.Current.Shutdown();
            return true;
        }
    }
}
