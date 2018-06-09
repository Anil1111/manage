using Manage.Core.Pageing;
using Manage.Data;
using Manage.Data.Domain;
using System.Collections.Generic;

namespace Manage.Service
{
    public interface IModuleService
    {
        Page<Sys_Module> FindPage(ModuleVM form);

        List<Sys_Module> GetModuleList();

        int Insert(ModuleVM form);

        Sys_Module GetModule(ModuleVM form);

        int Update(ModuleVM form);

        void Delete(ModuleVM form);
    }
}
