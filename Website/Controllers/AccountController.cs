using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VolunteerDatabase.Helper;
using Website.Models;
using Website.Models.ViewModels;

namespace Website.Controllers
{
    [WebsiteAuthorize]
    public class AccountController : Controller
    {
        private IdentityHelper helper = IdentityHelper.GetInstance();

        // GET: Account/Logoff
        public ActionResult Logoff()
        {
            HttpContext.Session?.Clear();
            Request.Cookies.Clear();
            return new HttpStatusCodeResult(200);
        }

        // GET: Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            var cookie = Request.Cookies["RememberMe"];
            if (cookie != null)
            {
                return Login(new LoginViewModel()
                {
                    Name = DecryptCookie(cookie["Name"]),
                    Password = DecryptCookie(cookie["Password"]),
                    RememberMe = true
                });
            }
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            //模型不符合要求，返回
            if (!ModelState.IsValid)
                return View();
            
            var claims = helper.CreateClaims(model.Name, model.Password);

            //登录失败，返回
            if (!claims.IsAuthenticated)
            {
                return View();
            }

            //登陆成功，判断是否需要记录Cookies
            if (model.RememberMe)
            {
                Request.Cookies.Add(new HttpCookie("RememberMe")
                {
                    ["Name"] = EncryptCookie(model.Name),
                    ["Password"] = EncryptCookie(model.Password),
                    Expires = DateTime.Now.AddDays(1)
                });
            }
            else
            {
                var cookie = Request.Cookies["RememberMe"];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Request.Cookies.Add(cookie);
                }
            }

            //添加登录态
            HttpContext.Session["Claim"] = claims.User;

            return new HttpStatusCodeResult(200);
        }

        private string EncryptCookie(string input, string certificate = "VolunteerDatabase")
        {
            //包含公钥文件的rsa算法初始化
            string pubKeyFile = Server.MapPath($"~/Certificate/{certificate}.cer");

            if (!System.IO.File.Exists(pubKeyFile))
                throw new FileNotFoundException();

            X509Certificate2 cer = new X509Certificate2(pubKeyFile);
            RSACryptoServiceProvider rsaProvider = (RSACryptoServiceProvider)cer.PublicKey.Key;

            //公钥加密
            byte[] plainBytes = Encoding.UTF8.GetBytes(input);
            byte[] encryptedBytes = rsaProvider.Encrypt(plainBytes, false);

            return Encoding.UTF8.GetString(encryptedBytes);

        }

        private string DecryptCookie(string input, string certificate = "VolunteerDatabase")
        {
            //包含私钥文件的rsa算法初始化
            string priKeyFile = Server.MapPath($"~/Certificate/{certificate}.pfx");

            if (!System.IO.File.Exists(priKeyFile))
                throw new FileNotFoundException();

            X509Certificate2 cert = new X509Certificate2(priKeyFile, "volcer");
            RSACryptoServiceProvider rsaProvider = (RSACryptoServiceProvider)cert.PrivateKey;

            //私钥解密
            byte[] encryptedBytes = Encoding.UTF8.GetBytes(input);
            byte[] decryptedBytes = rsaProvider.Decrypt(encryptedBytes, false);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}