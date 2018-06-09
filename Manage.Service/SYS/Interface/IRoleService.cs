using Manage.Core.Pageing;
using Manage.Data;
using Manage.Data.Domain;
using System.Collections.Generic;

namespace Manage.Service
{
    public interface IRoleService
    {
        Page<Sys_Role> FindPage(UserRoleVM form);

        int Insert(UserRoleVM form);

        Sys_Role GetRole(UserRoleVM form);

        int Delete(UserRoleVM form);

        List<UserRoleVM> Entities(int userId);

        List<UserRoleVM> EntitiesByGroupRole(int userId);

        List<Sys_PermissionRole> GetPermissionRoleList();

        List<Sys_Module> GetModuleList();

        List<Sys_Role> GetRoleList();

        void InsertUserGroupRole(List<Sys_UserGroupRole> list, int userGroupId);

        List<Sys_UserGroupRole> GetUserGroupRoleList();

        Root GetTree(int roleId);

        void InsertPermissionRole(List<Sys_PermissionRole> list);

        List<Sys_UserGroupUser> GetUserGroupUserList();

        void InsertUserGroupUser(List<Sys_UserGroupUser> list, int userId);

        List<Sys_RoleUser> GetRoleUserList();

        void InsertRoleUser(List<Sys_RoleUser> list, int userId);

        List<Sys_Permission> GetPermissionList();

        int Update(UserRoleVM form);
    }
}
