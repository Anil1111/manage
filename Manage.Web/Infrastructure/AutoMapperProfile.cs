using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace Manage.Web.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            var viewModelTypes = Assembly.Load("Manage.Data").GetTypes().Where(t => t.IsClass && t.Name.IndexOf("VM") != -1);
            foreach (Type viewModelType in viewModelTypes)
            {
                var modelTypes = Assembly.Load("Manage.Core").GetTypes().Where(t => t.IsClass && t.Name == viewModelType.Name.Replace("VM", ""));
                foreach (Type modelType in modelTypes)
                {
                    this.CreateMap(modelType, viewModelType);
                }
            }
        }
    }
}