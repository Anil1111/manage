using Manage.Core.Caching;
using Manage.Core.Infrastructure;
using System.Configuration;

namespace Manage.Core.Utility
{
    public class ConfigUtil
    {
        public static string GetValue(string key)
        {
            ICacheManager cacheManager = ServiceContainer.Resolve<ICacheManager>();

            string cacheKey = "AppSettings-" + key;
            if (!cacheManager.Contains(cacheKey))
            {
                cacheManager.Set(cacheKey, ConfigurationManager.AppSettings[key]);
            }

            return cacheManager.Get<object>(cacheKey).ToString();
        }
    }
}
