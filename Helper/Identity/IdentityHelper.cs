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

        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password, AppRoleEnum roleEnum)
        {
            var result = Task.Run(() => CreateUser(user: user, password: password, roleEnum: roleEnum));

            return await result;
        }

        public IdentityResult CreateUser(AppUser user, string password, AppRoleEnum roleEnum)
        {
            user.Salt = SecurityHelper.GetSalt();
            user.HashedPassword = SecurityHelper.Hash(password, user.Salt);
            database.Users.Add(user);
            Save();
            AddToRole(user.Id, roleEnum);

            return IdentityResult.Success();
        }

        private IdentityResult AddToRole(int userId, string roleName)
        {
            var user = database.Users.Find(userId);
            if (user == null)
            {
                return IdentityResult.Error("未找到用户。");
            }
            var role = database.Roles.SingleOrDefault(r => r.Name == roleName);
            if (role == null)
            {
                return IdentityResult.Error("无该用户组。");
            }
            if (user.Roles.Contains(role))
            {
                return IdentityResult.Error("该用户已存在于[" + roleName + "]用户组。");
            }
            user.Roles.Add(role);
            Save();

            return IdentityResult.Success();
        }

        public IdentityResult AddToRole(int userId, AppRoleEnum roleEnum)
        {
            var role = GetRole(roleEnum);
            return AddToRole(userId: userId, roleName: roleEnum.ToString());
        }

        public async Task<IdentityResult> AddToRoleAsync(int userId, AppRoleEnum roleEnum)
        {
            var result = Task.Run(()=>AddToRole(userId: userId, roleEnum: roleEnum));

            return await result;
        }

        public AppRole GetRole(AppRoleEnum roleEnum)
        {
            var role = database.Roles.SingleOrDefault(r => r.RoleEnum == roleEnum);
            if (role == null)
            {
                lock (locker)
                {
                    role = database.Roles.SingleOrDefault(r => r.RoleEnum == roleEnum);
                    if (role == null)
                    {
                        var newRole = new AppRole
                        {
                            Name = roleEnum.ToString(),
                            RoleEnum = roleEnum
                        };
                        database.Roles.Add(newRole);
                        Save();
                        role = database.Roles.SingleOrDefault(r => r.RoleEnum == roleEnum);
                    }
                }
            }
            return role;
        }

        public IdentityClaims CreateClaims(string userName, string password)
        {
            var user = database.Users.SingleOrDefault(u => u.Name == userName);
            if (SecurityHelper.CheckPassword(password: password, salt: user?.Salt, hashedPassword: user?.HashedPassword))
            {
                return IdentityClaims.Create(user);
            }
            return IdentityClaims.Create(null);
        }

        public IdentityResult ChangePassword(string userName, string currentPassword, string newPassword)
        {
            IdentityResult result;
            var claims = CreateClaims(userName: userName, password: currentPassword);
            if (claims.IsAuthenticated)
            {
                var user = claims.User;
                user.Salt = SecurityHelper.GetSalt();
                user.HashedPassword = SecurityHelper.Hash(password: newPassword, salt: user.Salt);
                Save();
                result = IdentityResult.Success();
            }
            else
            {
                result = IdentityResult.Error("密码验证失败。");
            }

            return result;
        }

        private void Save()
        {
            bool flag = false;
            do
            {
                try
                {
                    database.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    flag = true;
                    foreach (var entity in database.ChangeTracker.Entries())
                    {
                        database.Entry(entity).Reload();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            } while (flag);
        }
    }
}
