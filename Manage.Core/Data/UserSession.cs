using System;

namespace Manage.Core.Data
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public class UserSession
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string TokenId { get; set; }
    }
}
