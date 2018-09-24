using System;

namespace Manage.Core.Data
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public class UserSession
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        public string TokenId { get; set; }
    }
}
