using Manage.Core.Infrastructure;
using Manage.Web.Core.Infrastructure;
using System;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace Manage.Api
{
    public static class UnityConfig
    {
        public static IUnityContainer GetConfiguredContainer()
        {
            RegisterComponents(ServiceContainer.Current);
            return ServiceContainer.Current;
        }

        public static void RegisterComponents(IUnityContainer container)
        {
            container.RegisterInstance(container);
            ITypeFinder typeFinder = new WebTypeFinder();

            var registerTypes = typeFinder.FindClassesOfType<IDependencyRegister>();
            foreach (Type registerType in registerTypes)
            {
                var register = (IDependencyRegister)Activator.CreateInstance(registerType);
                register.RegisterTypes(container);
            }

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}