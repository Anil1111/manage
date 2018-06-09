using AutoMapper;
using Manage.Core.Infrastructure;
using System;
using System.Linq;
using System.Reflection;
using Unity;

namespace Manage.Web.Infrastructure
{
    public class AutoMapperRegister : IDependencyRegister
    {
        public void RegisterTypes(IUnityContainer container)
        {
            var profileTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && t.IsSubclassOf(typeof(Profile)));
            var profileInstances = profileTypes.Select(t => Activator.CreateInstance(t)).Cast<Profile>();
            var config = new MapperConfiguration(cfg => profileInstances.ToList().ForEach(p => cfg.AddProfile(p)));
            container.RegisterInstance(config);
            container.RegisterInstance(config.CreateMapper());
        }
    }
}