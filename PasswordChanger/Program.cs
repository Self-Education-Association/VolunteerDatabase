using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Helper;

namespace PasswordChanger
{
    class Program
    {
        public static string ReadPassword()
        {
            char[] revisekeys = new char[3];
            revisekeys[0] = (char)0x08;
            revisekeys[1] = (char)0x20;
            revisekeys[2] = (char)0x08;

            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo kinfo = Console.ReadKey(true);

                if (kinfo.Key == ConsoleKey.Enter)
                {
                    break;
                }

                if (kinfo.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length != 0)
                    {
                        int rIndex = sb.Length - 1;
                        sb.Remove(rIndex, 1);
                        Console.Write(revisekeys);
                    }
                    continue;
                }
                sb.Append(Convert.ToString(kinfo.KeyChar));
                Console.Write("*");
            }
            return sb.ToString();
        }
        static void Main(string[] args)
        {
            IdentityResult result = IdentityResult.Success();
            do
            {
                Console.WriteLine("UserName:");
                string username = Console.ReadLine();
                IdentityHelper ih = IdentityHelper.GetInstance();
                Console.WriteLine("Password:");
                string password = ReadPassword();
                result = ih.ChangePassword(username, "", password, true);
                if (!result.Succeeded)
                {
                    Console.WriteLine(string.Join(",", result.Errors));
                }
                else
                {
                    Console.WriteLine("修改成功。");
                }
            } while (!result.Succeeded);
            Console.ReadKey();
        }
    }

    
}
