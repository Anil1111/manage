using Manage.Core.Pageing;

namespace Manage.Data
{
    public class BaseVM : BasePage
    {
        public string ParentName { get; set; }

        public bool Check { get; set; }

        public string ModuleName { get; set; }

        public string BeginDate { get; set; }

        public string EndDate { get; set; }
    }
}
