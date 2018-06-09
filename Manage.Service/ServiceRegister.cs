using Manage.Core.Infrastructure;
using System;
using System.Linq;
using System.Reflection;
using Unity;

namespace Manage.Service
{
    public class ServiceRegister : IDependencyRegister
    {
        public void RegisterTypes(IUnityContainer container)
        {
            var serviceTypes = Assembly.Load("Manage.Service").GetTypes().Where(t => t.IsClass && t.Name.EndsWith("Service"));
            foreach (Type serviceType in serviceTypes)
            {
                Type iServiceType = Assembly.Load("Manage.Service").GetTypes().Where(t => t.IsInterface && t.Name == "I" + serviceType.Name).FirstOrDefault();
                container.RegisterType(iServiceType, serviceType);
            }
        }
    }
}
