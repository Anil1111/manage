using Manage.Core.Caching;
using Manage.Core.Infrastructure;
using Manage.Core.Logging;
using Unity;

namespace Manage.Service
{
    public class InfrastructureRegister : IDependencyRegister
    {
        public void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ICacheManager, RedisCacheManager>();
            //container.RegisterType<ICacheManager, MemoryCacheManager>();
            container.RegisterType<ILogger, Log4netManager>();
        }
    }
}
