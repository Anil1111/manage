using System.Collections.Generic;

namespace Manage.Data.Domain
{
    public class ModuleVM : BaseVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LinkUrl { get; set; }

        public int Code { get; set; }

        public string Icon { get; set; }

        public int ParentId { get; set; }

        public bool Enabled { get; set; }

        public string EnabledStr { get; set; }

        public List<Sys_Module> ChildModules { get; set; }
    }
}
