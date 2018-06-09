using Manage.Core.Pageing;
using Manage.Data;
using Manage.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service
{
    public interface IPermissionService
    {
        Page<Sys_Permission> FindPage(PermissionVM form);

        List<Sys_Permission> GetPermissionList();

        int Insert(PermissionVM form);

        Sys_Permission GetPermission(PermissionVM form);

        int Update(PermissionVM form);

        void Delete(PermissionVM form);

        IEnumerable<ModulePermissionVM> GetModulePermissionList();
    }
}
