using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using VolunteerDatabase.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VolunteerDatabase.Helper
{
    public class IdentityHelper
    {
        private static IdentityHelper helper;

        private static readonly object locker = new object();

        private Database database;

        private UserManager<AppUser> userManager;

        private RoleManager<IdentityRole> roleManager;

        public IdentityHelper GetInstance()
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

        private IdentityHelper()
        {
            database = new Database();
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(database));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(database));
        }

        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
        {
            var result = await userManager.CreateAsync(user: user, password: password);

            return result;
        }

        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password, AppUserRoleName roleName)
        {
            var role = GetRoleAsync(roleName);
            var result = await CreateUserAsync(user, password);
            if (result.Succeeded)
            {
                return await userManager.AddToRoleAsync(user.Id, (await role).Name);
            }
            return new IdentityResult(result.Errors);
        }

        public async Task<IdentityResult> AddToRoleAsync(string userId, AppUserRoleName roleName)
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

        public async Task<IdentityRole> GetRoleAsync(AppUserRoleName roleName)
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

    public enum AppUserRoleName
    {
        Administrator,
        OrgnizationAdministrator,
        OrgnizationMember
    }
}
