using Manage.Core.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Web.Core.Enums
{
    /// <summary>
    /// 用户状态枚举
    /// </summary>
    public enum UserState
    {
        /// <summary>
        /// 正常
        /// </summary>
        [RemarkAttribute("正常")]
        Normal = 0,
        /// <summary>
        /// 冻结
        /// </summary>
        [RemarkAttribute("冻结")]
        Frozen = 1,
        /// <summary>
        /// 删除
        /// </summary>
        //[Remark("删除")]
        Deleted = 2
    }
}
