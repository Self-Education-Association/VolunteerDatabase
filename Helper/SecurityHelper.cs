using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace VolunteerDatabase.Helper
{
    public static class SecurityHelper
    {
        private static SHA256Managed sha = new SHA256Managed();

        private static UTF8Encoding encoding = new UTF8Encoding();

        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        public static byte[] Hash(byte[] input)
        {
            sha.Initialize();
            return sha.ComputeHash(input);
        }

        public static string Hash(string password)
        {
            byte[] passwordBytes = encoding.GetBytes(password);
            return encoding.GetString(Hash(passwordBytes));
        }

        public static string Hash(string password, string salt)
        {
            return Hash(password + salt);
        }

        public static string GetSalt(int length = 256)
        {
            byte[] salt = new byte[length];
            rng.GetBytes(salt);
            return encoding.GetString(salt);
        }

        public static bool CheckPassword(string password, string salt, string hashedPassword)
        {
            if (Hash(password: password, salt: salt) == hashedPassword)
            {
                return true;
            }
            return false;
        }
    }
}
