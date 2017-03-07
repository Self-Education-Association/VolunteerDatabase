using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Interface
{
    public interface ILog<T> where T : IUser
    {
        T Adder { get; set; }
        DateTime AddTime { get; set; }
        string LogContent { get; set; }

    }
    public enum LogType
    {
        Default,
        #region 登陆认证模块
        Register,
        Login,
        Authorize,
        
        AddToRole,
        CreateClaims,
        ChangePassword,
        Organizationole,
        CreateOrganization,
        #endregion

        #region 账号管理模块
        Accept,
        Deny,
        EditInfo,
        #endregion

        #region 志愿者信息模块
        AddVolunteer,
        DeleteVolunteer,
        EditVolunteer,
        EditContact,
        LastContactEdit,
        #endregion

        #region 黑名单模块
        AddBlackList,
        DeleteBlackList,
        EnableBlackList,
        DisableBlackList,
        EditBlackList,
        #endregion

        #region 项目模块
        CreateProject,
        EditProject,
        DeleteProject,
        FinishProject,
        ScoreVolunteer,
        PortInOriginalVolunteers,
        PortInSelectedVolunteers,
        FinishSelectVolunteers,
        DesignateManager
        #endregion
    }
}
