using System;
using System.Collections.Generic;

namespace Manage.Core.Pageing
{
    [Serializable]
    public class Page<T> : BasePage
    {
        public Page() { }

        public List<T> ResultList { get; set; }
    }
}
