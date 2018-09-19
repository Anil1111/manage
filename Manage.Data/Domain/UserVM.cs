namespace Manage.Data.Domain
{
    public class UserVM : Sys_User
    {
        public int UserId { get; set; }

        public string CheckCode { get; set; }
    }
}
