using Manage.Core.Json;
using Manage.Data;
using Manage.Service;
using Manage.Web.Core.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manage.Web.Areas.Common.Controllers
{
    public class SelectPageController : Controller
    {
        private readonly IUserService _userService;
        public SelectPageController(IUserService _userService)
        {
            this._userService = _userService;
        }

        [CustomExceptionFilterAttribute()]
        public string GetSelectPage(int flag)
        {
            string json = "";
            switch (flag)
            {
                case 1:
                    List<Sys_User> list = this._userService.Entities();
                    List<List> listResult = new List<List>();
                    foreach (Sys_User item in list)
                    {
                        listResult.Add(new List
                        {
                            id = item.Id,
                            desc = item.UserName
                        });
                    }
                    json = ResponseJson.Success(JsonUtil.SerializerObject(listResult), "SUCCESS");
                    break;
                default:
                    break;
            }
            return json;
        }
    }

    public class List
    {
        public string desc { get; set; }
        public int id { get; set; }
    }
}