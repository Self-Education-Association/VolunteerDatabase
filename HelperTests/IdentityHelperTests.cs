using Microsoft.VisualStudio.TestTools.UnitTesting;
using VolunteerDatabase.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Helper.Tests
{
    [TestClass()]
    public class IdentityHelperTests
    {
        IdentityHelper helper = IdentityHelper.GetInstance();
        Database database = DatabaseContext.GetInstance();

        [TestMethod()]
        public void GetInstanceTest()
        {
            Assert.IsInstanceOfType(helper, typeof(IdentityHelper));
        }

        [TestMethod()]
        public void CreateUserTest()
        {
            string accountName = "X!@#$%^&*()_+-=1234567890qwertyuiop[]\\{}|asdfghjkl;':\"zxcvbnm,./<>?";
            AppUser dbUser = database.Users.SingleOrDefault(u => u.AccountName == accountName);
            if (dbUser != null)
            {
                database.Users.Remove(dbUser);
                database.SaveChanges();
            }

            AppUser user = new AppUser
            {
                Name = "张三李四·.",
                AccountName = accountName,
                Email = "123@abc.com",
                Mobile = "18888888888"
            };
            string password = "!@#$%^&*()_+-=1234567890qwertyuiop[]\\{}|asdfghjkl;':\"zxcvbnm,./<>?";
            IdentityResult result = helper.CreateUser(user, password, AppRoleEnum.OrgnizationMember, OrganizationEnum.SEA团队);
            if (!result.Succeeded)
            {
                Assert.Fail();
            }

            dbUser = database.Users.SingleOrDefault(u => u.AccountName == user.AccountName);
            if (dbUser == null)
            {
                Assert.Fail();
            }
            if (dbUser.Name != user.Name || dbUser.AccountName != user.AccountName || dbUser.Email != user.Email || dbUser.Mobile != user.Mobile)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void AddToRoleTest()
        {
            string accountName = "!@#$%^&*()_+-=1234567890qwertyuiop[]\\{}|asdfghjkl;':\"zxcvbnm,./<>?";
            AppUser dbUser = database.Users.SingleOrDefault(u => u.AccountName == accountName);
            if (dbUser != null)
            {
                database.Users.Remove(dbUser);
                database.SaveChanges();
            }

            AppUser user = new AppUser
            {
                Name = "张三李四·.",
                AccountName = accountName,
                Email = "123@abc.com",
                Mobile = "18888888888"
            };
            string password = "!@#$%^&*()_+-=1234567890qwertyuiop[]\\{}|asdfghjkl;':\"zxcvbnm,./<>?";
            IdentityResult result = helper.CreateUser(user, password, AppRoleEnum.OrgnizationMember, OrganizationEnum.SEA团队);
            if (!result.Succeeded)
            {
                Assert.Inconclusive("CreateUser方法运行失败，本测试无法运行。");
            }
            dbUser = database.Users.SingleOrDefault(u => u.AccountName == user.AccountName);
            helper.AddToRole(dbUser.Id, AppRoleEnum.LogViewer);
            AppUserIdentityClaims claims = helper.CreateClaims(user.AccountName, password);
            if (!claims.IsAuthenticated)
            {
                Assert.Inconclusive("CreateClaims方法运行失败，未取得IdentityClaims。");
            }
            if (!claims.IsInRole(AppRoleEnum.LogViewer))
            {
                Assert.Fail("角色添加失败！");
            }
            database.Users.Remove(dbUser);
            database.SaveChanges();
        }

        [TestMethod()]
        public void CreateOrFindRoleTest()
        {
            var role = database.Roles.SingleOrDefault(r => r.RoleEnum == AppRoleEnum.TestOnly);
            if (role != null)
            {
                database.Roles.Remove(role);
                database.SaveChanges();
            }
            var helperRole = helper.CreateOrFindRole(AppRoleEnum.TestOnly);
            if (helperRole == null)
            {
                Assert.Fail("未返回Role对象。");
            }
            if (helperRole.RoleEnum != AppRoleEnum.TestOnly || helperRole.Name != AppRoleEnum.TestOnly.ToString())
            {
                Assert.Fail("返回的Role对象不符合标准。");
            }
            role = database.Roles.SingleOrDefault(r => r.RoleEnum == AppRoleEnum.TestOnly);
            if (role == null)
            {
                Assert.Fail("Role对象未插入数据库。");
            }
            database.Roles.Remove(role);
            database.SaveChanges();
        }

        [TestMethod()]
        public void CreateOrFindOrganizationTest()
        {
            var org = database.Organizations.SingleOrDefault(o => o.OrganizationEnum == OrganizationEnum.TestOnly);
            if (org != null)
            {
                database.Organizations.Remove(org);
                database.SaveChanges();
            }
            var helperOrg = helper.CreateOrFindOrganization(OrganizationEnum.TestOnly);
            if (helperOrg == null)
            {
                Assert.Fail("未返回Organization对象。");
            }
            if (helperOrg.OrganizationEnum != OrganizationEnum.TestOnly || helperOrg.Name != OrganizationEnum.TestOnly.ToString())
            {
                Assert.Fail("返回的Organization对象不符合标准。");
            }
            org = database.Organizations.SingleOrDefault(o => o.OrganizationEnum == OrganizationEnum.TestOnly);
            if (org == null)
            {
                Assert.Fail("Organization对象未插入数据库。");
            }
            database.Organizations.Remove(org);
            database.SaveChanges();
        }

        [TestMethod()]
        public void CreateClaimsTest()
        {
            string accountName = "~!@#$%^&*()_+-=1234567890qwertyuiop[]\\{}|asdfghjkl;':\"zxcvbnm,./<>?";
            AppUser dbUser = database.Users.SingleOrDefault(u => u.AccountName == accountName);
            if (dbUser != null)
            {
                database.Users.Remove(dbUser);
                database.SaveChanges();
            }

            AppUser user = new AppUser
            {
                Name = "张三李四·.",
                AccountName = accountName,
                Email = "123@abc.com",
                Mobile = "18888888888"
            };
            string password = "!@#$%^&*()_+-=1234567890qwertyuiop[]\\{}|asdfghjkl;':\"zxcvbnm,./<>?";
            IdentityResult result = helper.CreateUser(user, password, AppRoleEnum.OrgnizationMember, OrganizationEnum.SEA团队);
            if (!result.Succeeded)
            {
                Assert.Inconclusive("CreateUser方法运行失败，本测试无法运行。");
            }

            AppUserIdentityClaims claims = helper.CreateClaims(user.AccountName, password);

            dbUser = database.Users.SingleOrDefault(u => u.AccountName == accountName);
            database.Users.Remove(dbUser);
            database.SaveChanges();
            Assert.IsTrue(claims.IsAuthenticated);
        }

        [TestMethod()]
        public void ChangePasswordTest()
        {
            string accountName = "!!@#$%^&*()_+-=1234567890qwertyuiop[]\\{}|asdfghjkl;':\"zxcvbnm,./<>?";
            AppUser dbUser = database.Users.SingleOrDefault(u => u.AccountName == accountName);
            if (dbUser != null)
            {
                database.Users.Remove(dbUser);
                database.SaveChanges();
            }

            AppUser user = new AppUser
            {
                Name = "张三李四·.",
                AccountName = accountName,
                Email = "123@abc.com",
                Mobile = "18888888888"
            };
            string password = "!@#$%^&*()_+-=1234567890qwertyuiop[]\\{}|asdfghjkl;':\"zxcvbnm,./<>?";
            string newPassword = "@!@#$%^&*()_+-=1234567890qwertyuiop[]\\{}|asdfghjkl;':\"zxcvbnm,./<>?";
            IdentityResult result = helper.CreateUser(user, password, AppRoleEnum.OrgnizationMember, OrganizationEnum.SEA团队);
            if (!result.Succeeded)
            {
                Assert.Inconclusive("CreateUser方法运行失败，本测试无法运行。");
            }

            var oldPasswordClaims = helper.CreateClaims(accountName, password);
            if (!oldPasswordClaims.IsAuthenticated)
            {
                Assert.Inconclusive("CreateClaims方法运行失败，本测试无法运行。");
            }
            result = helper.ChangePassword(accountName, password, newPassword);
            if (!result.Succeeded)
            {
                Assert.Fail("修改密码失败。");
            }
            var passwordNotChanged = helper.CreateClaims(accountName, password);
            if (passwordNotChanged.IsAuthenticated)
            {
                Assert.Fail("原密码仍能登入");
            }
            var newPasswordClaims = helper.CreateClaims(accountName, newPassword);
            if (!newPasswordClaims.IsAuthenticated)
            {
                Assert.Fail("新密码无法登入。");
            }

            dbUser = database.Users.SingleOrDefault(u => u.AccountName == accountName);
            database.Users.Remove(dbUser);
            database.SaveChanges();
        }
    }
}