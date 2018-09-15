namespace Manage.Data.Data
{
    public class ContextDB
    {
        public static readonly ManageDBEntities managerDBContext = new ManageDBEntities();
        public static readonly SMS_DBEntities smsDBContext = new SMS_DBEntities();
    }
}
