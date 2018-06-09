using Manage.Core.Pageing;
using Manage.Data;
using Manage.Data.Domain;
using System.Collections.Generic;

namespace Manage.Service
{
    public interface IUserService
    {
        Page<Sys_User> FindPage(UserVM form);

        int Insert(UserVM form);

        int Insert(List<Sys_User> entities);

        Sys_User GetUser(UserVM form);

        int Delete(Sys_User user);

        int Update(UserVM form);

        List<Sys_User> Entities(string username);

        void Login(UserVM form);

        int UserResetPwd(UserVM form);

        int UpdatePwd(string oldPassword, string password, int userId);
    }
}
