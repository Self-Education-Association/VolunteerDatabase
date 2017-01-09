using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper
{
    public sealed class IdentityHelper
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
            database = DatabaseContext.GetInstance();
        }

        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password, AppRoleEnum roleEnum, OrganizationEnum orgEnum)
        {
            var result = Task.Run(() => CreateUser(user: user, password: password, roleEnum: roleEnum, orgEnum: orgEnum));

            return await result;
        }

        public IdentityResult CreateUser(AppUser user, string password, AppRoleEnum roleEnum, OrganizationEnum orgEnum)
        {
            if (database.Users.Where(u => u.AccountName == user.AccountName) != null && database.Users.Where(u => u.AccountName == user.AccountName).Count() != 0)
            {
                return IdentityResult.Error("该用户名已被使用。");
            }
            var org = CreateOrFindOrganization(orgEnum);
            user.Salt = SecurityHelper.GetSalt();
            user.HashedPassword = SecurityHelper.Hash(password, user.Salt);
            user.Organization = org;
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
            if (user.Roles == null)
            {
                user.Roles = new List<AppRole>();
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
            var role = CreateOrFindRole(roleEnum);
            return AddToRole(userId: userId, roleName: roleEnum.ToString());
        }

        public async Task<IdentityResult> AddToRoleAsync(int userId, AppRoleEnum roleEnum)
        {
            var result = Task.Run(() => AddToRole(userId: userId, roleEnum: roleEnum));

            return await result;
        }

        public AppRole CreateOrFindRole(AppRoleEnum roleEnum)
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

        public Organization CreateOrFindOrganization(OrganizationEnum orgEnum)
        {
            var org = database.Organizations.SingleOrDefault(r => r.OrganizationEnum == orgEnum);
            if (org == null)
            {
                lock (locker)
                {
                    org = database.Organizations.SingleOrDefault(r => r.OrganizationEnum == orgEnum);
                    if (org == null)
                    {
                        var newOrg = new Organization
                        {
                            Name = orgEnum.ToString(),
                            OrganizationEnum = orgEnum
                        };
                        database.Organizations.Add(newOrg);
                        Save();
                        org = database.Organizations.SingleOrDefault(r => r.OrganizationEnum == orgEnum);
                    }
                }
            }
            return org;
        }

        public AppUserIdentityClaims CreateClaims(string userName, string password, string holderName = "")
        {
            var user = database.Users.SingleOrDefault(u => u.AccountName == userName);
            if (SecurityHelper.CheckPassword(password: password, salt: user?.Salt, hashedPassword: user?.HashedPassword))
            {
                var holder = database.Users.SingleOrDefault(u => u.AccountName == holderName);
                if (holder != null)
                {
                    return AppUserIdentityClaims.Create(user, holder);
                }
                return AppUserIdentityClaims.Create(user, user); //若Holder不为有效值则Holder被赋值为User
            }
            return AppUserIdentityClaims.Create(null, user);
        }

        public Task<AppUserIdentityClaims> CreateClaimsAsync(string userName, string password, string holderName = "")
        {
            return Task.Run(() => CreateClaims(userName, password, holderName));
        }

        public IdentityResult ChangePassword(string userName, string currentPassword, string newPassword)
        {
            IdentityResult result;
            var claims = CreateClaims(userName: userName, password: currentPassword);
            if (claims.IsAuthenticated)
            {
                var user = database.Users.SingleOrDefault(u => u.AccountName == claims.User.AccountName);
                if (user == null)
                {
                    return IdentityResult.Error("找不到用户。");
                }
                user.Salt = SecurityHelper.GetSalt();
                user.HashedPassword = SecurityHelper.Hash(password: newPassword, salt: user.Salt);
                Save();
                result = IdentityResult.Success();
            }
            else
            {
                result = IdentityResult.Error("用户验证失败。");
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
