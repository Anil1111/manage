using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Manage.Api.Controllers
{
    public class BaseController : ApiController
    {
        public HttpResponseMessage ToJson(string content)
        {
            HttpResponseMessage result = new HttpResponseMessage
            {
                Content = new StringContent(content, Encoding.GetEncoding("UTF-8"), "application/json")
            };

            return result;
        }
    }
}
