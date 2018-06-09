using Unity;

namespace Manage.Core.Infrastructure
{
    public interface IDependencyRegister
    {
        void RegisterTypes(IUnityContainer container);
    }
}
