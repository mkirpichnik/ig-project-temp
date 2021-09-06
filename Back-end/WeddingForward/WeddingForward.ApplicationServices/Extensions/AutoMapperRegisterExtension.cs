using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace WeddingForward.ApplicationServices.Extensions
{
    public static class AutoMapperRegisterExtension
    {
        public static IServiceCollection RegisterMapper(this IServiceCollection container)
        {
            return container.AddSingleton(MapperConfigurationFactory)
                .AddSingleton<IConfigurationProvider>(provider => provider.GetService<MapperConfiguration>())
                .AddTransient<IMapper>(provider => provider.GetService<MapperConfiguration>().CreateMapper());
        }

        private static MapperConfiguration MapperConfigurationFactory(IServiceProvider unityContainer)
        {
            return new MapperConfiguration(configuration =>
            {
                configuration.ConstructServicesUsing(unityContainer.GetService);

                foreach (Profile profile in unityContainer.GetServices<Profile>())
                {
                    configuration.AddProfile(profile);
                }
            });
        }
    }
}
