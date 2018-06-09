using System;
using System.Collections.Generic;
using Unity;

namespace Manage.Core.Infrastructure
{
    public static class ServiceContainer
    {
        static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() => new UnityContainer());
        public static IUnityContainer Current { get { return container.Value; } }

        public static T Resolve<T>() where T : class
        {
            return container.Value.Resolve<T>();
        }

        public static IEnumerable<T> ResolveAll<T>() where T : class
        {
            return container.Value.ResolveAll<T>();
        }
    }
}
