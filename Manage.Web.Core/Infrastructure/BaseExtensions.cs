using Manage.Core.Caching;
using Manage.Core.Data;
using Manage.Core.Infrastructure;
using Manage.Core.Utility;

namespace Manage.Web.Core.Infrastructure
{
    public class BaseExtensions
    {
        public static UserSession UserSession()
        {
            string token = CookiesUtil.GetCookiesValue(SuperConstants.COOKIESID);
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            else
            {
                ICacheManager cacheManager = ServiceContainer.Resolve<ICacheManager>();
                UserSession userSession = cacheManager.Get<UserSession>(token);
                if (userSession == null)
                    return null;
                else
                    return userSession;
            }
        }
    }
}
