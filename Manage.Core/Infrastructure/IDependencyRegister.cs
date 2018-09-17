using Unity;

namespace Manage.Core.Infrastructure
{
    /// <summary>
    /// IOC注册
    /// </summary>
    public interface IDependencyRegister
    {
        void RegisterTypes(IUnityContainer container);
    }
}
