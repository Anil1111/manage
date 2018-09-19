namespace Manage.Data.Data
{
    /// <summary>
    /// EF上下文
    /// </summary>
    public class ContextDB
    {
        /// <summary>
        /// 管理数据库上下午
        /// </summary>
        public static readonly ManageDBEntities managerDBContext = new ManageDBEntities();

        /// <summary>
        /// 手机消息数据库上下午
        /// </summary>
        public static readonly SMS_DBEntities smsDBContext = new SMS_DBEntities();
    }
}
