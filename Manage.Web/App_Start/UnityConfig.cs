using Manage.Core.Infrastructure;
using Manage.Web.Core.Infrastructure;
using System;
using System.Collections.Generic;
using Unity;

namespace Manage.Web
{
    public static class UnityConfig
    {
        public static IUnityContainer GetConfiguredContainer()
        {
            RegisterTypes(ServiceContainer.Current);

            return ServiceContainer.Current;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterInstance(container);
            ITypeFinder typeFinder = new WebTypeFinder();

            IEnumerable<Type> registerTypes = typeFinder.FindClassesOfType<IDependencyRegister>();
            foreach (Type registerType in registerTypes)
            {
                IDependencyRegister register = (IDependencyRegister)Activator.CreateInstance(registerType);
                register.RegisterTypes(container);
            }
        }
    }
}