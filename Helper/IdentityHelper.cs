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

        private static readonly object helperLocker = new object();
        private static readonly object roleLocker = new object();
        private static readonly object orgLocker = new object();

        private Database database;

        public static IdentityHelper GetInstance()
        {
            if (helper == null)
            {
                lock (helperLocker)
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

        public OrganizationEnum Matching(string str)
        {
            OrganizationEnum org = new OrganizationEnum();
            if (OrganizationEnum.中国语言文学学院.ToString() == str)
            { org = OrganizationEnum.中国语言文学学院; }
            if (OrganizationEnum.SEA团队.ToString() == str)
            { org = OrganizationEnum.SEA团队; }
            if (OrganizationEnum.TestOnly.ToString() == str)
            { org = OrganizationEnum.TestOnly; }
            if (OrganizationEnum.保险学院.ToString() == str)
            { org = OrganizationEnum.保险学院; }
            if (OrganizationEnum.信息学院.ToString() == str)
            { org = OrganizationEnum.信息学院; }
            if (OrganizationEnum.公共管理学院.ToString() == str)
            { org = OrganizationEnum.公共管理学院; }
            if (OrganizationEnum.国际商学院.ToString() == str)
            { org = OrganizationEnum.国际商学院; }
            if (OrganizationEnum.国际经济贸易学院.ToString() == str)
            { org = OrganizationEnum.国际经济贸易学院; }
            if (OrganizationEnum.校志愿服务中心.ToString() == str)
            { org = OrganizationEnum.校志愿服务中心; }
            if (OrganizationEnum.法学院.ToString() == str)
            { org = OrganizationEnum.法学院; }
            if (OrganizationEnum.统计学院.ToString() == str)
            { org = OrganizationEnum.统计学院; }
            if (OrganizationEnum.金融学院.ToString() == str)
            { org = OrganizationEnum.金融学院; }
            return org;
        }


        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password, AppRoleEnum roleEnum, OrganizationEnum orgEnum)
        {
            var result = Task.Run(() => CreateUser(user: user, password: password, roleEnum: roleEnum, orgEnum: orgEnum));

            return await result;
        }

        public IdentityResult CreateUser(AppUser user, string password, AppRoleEnum roleEnum = AppRoleEnum.OrgnizationMember, OrganizationEnum orgEnum = OrganizationEnum.TestOnly)
        {
            lock (database)
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
            }

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
                lock (roleLocker)
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
                lock (orgLocker)
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

        //显示所有通过审批的成员
        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public List<AppUser> ShowApprovedMembers(Organization org)
        {
            return org.Members.Where(m => m.Status == AppUserStatus.Enabled).ToList();
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public List<AppUser> ShowNotApprovedMembers(Organization org)
        {
            return org.Members.Where(m => m.Status == AppUserStatus.NotApproved).ToList();
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public IdentityResult ApproveRegisterRequest(AppUser user)
        {
            if (user == null)
            {
                return IdentityResult.Error("待审批的用户为空,请查询后重试!");
            }
            try
            {
                user.Status = AppUserStatus.Enabled;
                Save();
                return IdentityResult.Success();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public IdentityResult ApproveRegisterRequest(List<AppUser> users)
        {
            if (users == null)
            {
                return IdentityResult.Error("生成的用户列表为空,请查询后重试!");
            }
            try
            {
                foreach (var user in users)
                {
                    if (user == null)
                    {
                        return IdentityResult.Error("待审批的某个用户为空,请查询后重试!");
                    }
                    user.Status = AppUserStatus.Enabled;
                }
                Save();
                return IdentityResult.Success();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public IdentityResult RejectRegisterRequest(AppUser user, string note = "无")
        {
            if (user == null)
            {
                return IdentityResult.Error("待拒绝的用户为空,用户实体非法,请查询后重试!");
            }
            if (user.Id == 0)
            {
                return IdentityResult.Error("用户实体非法,请查询后重试!");
            }
            try
            {
                var approvalrecord = database.ApprovalRecords.SingleOrDefault(r => r.User.Id == user.Id);
                if (approvalrecord == null)
                {
                    return IdentityResult.Error("没有找到对应用户的审批记录!");
                }
                approvalrecord.IsApproved = false;
                approvalrecord.ExpireTime = DateTime.Now.AddDays(3);//最近的审批记录
                approvalrecord.Note = "拒绝学号为[" + approvalrecord.User.StudentNum.ToString() + "]用户名为[" + approvalrecord.User.AccountName + "]的账号注册请求.";
                database.ApprovalRecords.Add(approvalrecord);
                approvalrecord.User = null;
                if (database.Entry(user).State == System.Data.Entity.EntityState.Detached)
                {
                    database.Users.Attach(user);
                }
                database.Users.Remove(user);
                Save();
                return IdentityResult.Success();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public ApprovalRecord GetApprovalRecordByRequestedUser(AppUser user)
        {
            var result = database.ApprovalRecords.SingleOrDefault(r => r.User.Id == user.Id);
            if (result == null)
            {
                return null;
            }
            try
            {
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public List<ApprovalRecord> GetApprovalRecordByOrganization(Organization org)
        {
            List<ApprovalRecord> result = database.ApprovalRecords.Where(r => r.Organization.Id == org.Id).ToList();
            return result;
        }

        [AppAuthorize(AppRoleEnum.Administrator)]
        [AppAuthorize(AppRoleEnum.OrgnizationAdministrator)]
        public List<AppUser> GetUnderlingsList(AppUser superior)
        {
            if (superior == null)
            {
                return null;
            }
            try
            {
                return superior.Underlings;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Save()
        {
            bool flag = false;
            do
            {
                try
                {
                    database.SaveChanges();
                    flag = false;
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
