namespace Manage.Data.Data
{
    /// <summary>
    /// EF上下文
    /// </summary>
    public class ContextDB
    {
        public static readonly ManageDBEntities managerDBContext = new ManageDBEntities();
        public static readonly SMS_DBEntities smsDBContext = new SMS_DBEntities();
    }
}
