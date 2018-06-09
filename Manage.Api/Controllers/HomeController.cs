using Manage.Data;
using Manage.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Manage.Api.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")] 跨越
    public class HomeController : BaseController
    {
        private readonly IModuleService _moduleService;
        public HomeController(IModuleService moduleService)
        {
            this._moduleService = moduleService;
        }

        public HttpResponseMessage GetUsers()
        {
            List<Sys_Module> list = this._moduleService.GetModuleList();

            return null;
        }
    }
}
