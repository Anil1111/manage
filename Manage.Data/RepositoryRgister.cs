using Manage.Core.Data;
using Manage.Core.Infrastructure;
using Manage.Data.Data;
using Unity;

namespace Manage.Data
{
    public class RepositoryRgister : IDependencyRegister
    {
        public void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType(typeof(IRepository<>), typeof(EfRepository<>));
        }
    }
}
