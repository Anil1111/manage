using System;

namespace Manage.Core.Data
{
    [Serializable]
    public class UserSession
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string TokenId { get; set; }
    }
}
