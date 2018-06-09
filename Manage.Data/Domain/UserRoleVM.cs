namespace Manage.Data.Domain
{
    public class UserRoleVM : BaseVM
    {
        public int Role_Id { get; set; }
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int OrderSort { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
