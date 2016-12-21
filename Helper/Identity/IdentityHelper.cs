using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;

namespace VolunteerDatabase.Helper
{
    public class IdentityHelper
    {
        private static IdentityHelper helper;

        private static readonly object locker = new object();

        private Database database;

        public static IdentityHelper GetInstance()
        {
            if (helper == null)
            {
                lock (locker)
                {
                    if (helper == null)
                    {
                        helper = new IdentityHelper();
                    }
                }
            }
            return helper;
        }

        public static async Task<IdentityHelper> GetInstanceAsync()
        {
            Task<IdentityHelper> helper = Task.Run(() =>
            {
                return GetInstance();
            });
            return await helper;
        }

        private IdentityHelper()
        {
            database = new Database();
        }

        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
        {
            var result = Task.Run(() => CreateUser(user, password));

            return await result;
        }

        public IdentityResult CreateUser(AppUser user, string password)
        {
            user.Salt = SecurityHelper.GetSalt();
            user.HashedPassword = SecurityHelper.Hash(password, user.Salt);
            database.Users.Add(user);
            bool flag = false;
            do
            {
                try
                {
                    database.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    flag = true;
                }
            } while (flag);

            return IdentityResult.Success();
        }

        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password, AppRoleEnum roleName)
        {
            var role = GetRoleAsync(roleName);
            var result = await CreateUserAsync(user, password);
            if (result.Succeeded)
            {
                return await userManager.AddToRoleAsync(user.Id, (await role).Name);
            }
            return new IdentityResult(result.Errors);
        }

        public IdentityResult AddToRole(string userId, string role)
        {
            var user = database.Users.Find(userId);
            if (user == null)
            {
                return IdentityResult.Error("未找到用户。");
            }
            if (user.Role)
        }

        public async Task<IdentityResult> AddToRoleAsync(string userId, AppRoleEnum roleName)
        {
            var role = await GetRoleAsync(roleName);
            var result = await userManager.AddToRoleAsync(userId: userId, role: role.Name);
            return result;
        }

        public async Task<IdentityResult> AddToRoleAsync(string userId, IdentityRole role)
        {
            var result = await userManager.AddToRoleAsync(userId: userId, role: role.Name);
            return result;
        }

        public async Task<IdentityRole> GetRoleAsync(AppRoleEnum roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName.ToString());
            if (role == null)
            {
                lock (locker)
                {
                    role = roleManager.FindByName(roleName.ToString());
                    if (role == null)
                    {
                        var create = roleManager.Create(new IdentityRole(roleName.ToString()));
                        if (create.Succeeded)
                        {
                            role = new IdentityRole(roleName.ToString());
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            return role;
        }

        public async Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword)
        {
            var result = await userManager.ChangePasswordAsync(userId: user.Id, currentPassword: currentPassword, newPassword: newPassword);

            return result;
        }
    }
}
