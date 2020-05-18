using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Dvor.Web.Infrastructure.Modules
{
    public static class MapperModule
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingConfiguration));
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingConfiguration()); });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}