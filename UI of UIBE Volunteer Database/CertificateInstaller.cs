using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Desktop
{
    public class CertificateInstaller
    {
        string certificateName = "Self Education Association";
        string certificateStored = "SelfEducationAssociation.cer";
        public bool? InstallCertificate()
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.MaxAllowed);
            X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectName, certificateName, false);
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
                X509Certificate2 certificate = new X509Certificate2(Environment.CurrentDirectory + "\\" + certificateStored);
                X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                store.Add(certificate);
                store.Close();
            }
            catch (SecurityException)
            {
                return false;
            }
            catch (CryptographicException)
            {
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
