using Manage.Core.Infrastructure;
using System;
using System.Linq;
using System.Reflection;
using Unity;

namespace Manage.Service
{
    /// <summary>
    /// Service注册
    /// </summary>
    public class ServiceRegister : IDependencyRegister
    {
        public void RegisterTypes(IUnityContainer container)
        {
            Assembly assembly = Assembly.Load("Manage.Service");
            var serviceTypes = assembly.GetTypes().Where(t => t.IsClass && t.Name.EndsWith("Service"));
            foreach (Type serviceType in serviceTypes)
            {
                Type iServiceType = assembly.GetTypes().Where(t => t.IsInterface && t.Name == "I" + serviceType.Name).FirstOrDefault();
                container.RegisterType(iServiceType, serviceType);
            }
        }
    }
}
