using System.Reflection;
//using Microsoft.Extensions.DependencyInjection;

namespace Engine.Presentation;

public static class ConventionServiceCollectionExtensions
{
    public static IServiceCollection AddServicesByConvention(this IServiceCollection services, Assembly assembly)
    {
        var types = assembly.GetExportedTypes();

        foreach (var type in types)
        {
            if (type.IsAbstract || type.IsInterface || !type.IsClass)
            {
                continue;
            }

            if (type.Name.EndsWith("UseCase"))
            {
                services.AddScoped(type);
                continue;
            }

            if (type.Name.EndsWith("Service") || type.Name.EndsWith("Repository"))
            {
                var serviceInterface = type.GetInterfaces()
                                           .FirstOrDefault(i => i.Name == $"I{type.Name}");

                if (serviceInterface != null)
                {
                    services.AddScoped(serviceInterface, type);
                }
            }
        }

        return services;
    }
}
