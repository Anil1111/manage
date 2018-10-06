using Manage.Data;
using Manage.Data.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.QuartzJobs
{
    public class UserServiceJob
    {
        private static ManageDBEntities context;
        public UserServiceJob()
        {
            context = new ManageDBEntities();
        }

        public List<Sys_User> Entities(string userName)
        {
            string sql = "select * from Sys_User a where a.UserName = @0";
            SqlParameter[] parameter = new SqlParameter[] {
                new SqlParameter("@0", SqlDbType.VarChar, 50)
            };
            parameter[0].Value = userName;

            return context.Database.SqlQuery<Sys_User>(sql, parameter).ToList();
        }
    }
}
