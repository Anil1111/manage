namespace Manage.Data.Domain
{
    public class UserVM : Sys_User
    {
        public int UserId { get; set; }

        public string CheckCode { get; set; }

        //SelectPage begin
        public int HidSelectPage { get; set; }
        public string SelectUserNameKey { get; set; }
        //SelectPage end
    }
}
