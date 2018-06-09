using Manage.Core.Pageing;
using Manage.Data;
using Manage.Data.Domain;
using System.Collections.Generic;

namespace Manage.Service
{
    public interface IUserGroupService
    {
        Page<Sys_UserGroup> FindPage(UserGroupVM form);

        List<Sys_UserGroup> GetUserGroupList();

        int Insert(UserGroupVM form);

        Sys_UserGroup GetUserGroup(UserGroupVM form);

        int Update(UserGroupVM form);

        int Delete(UserGroupVM form);
    }
}
