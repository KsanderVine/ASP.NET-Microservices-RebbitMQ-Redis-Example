using KinoSearch.Configuration.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace KinoSearch.Configuration.Extensions
{
    public static class ConfigurationProcessorExtensions
    {
        public static IServiceCollection AddConfigurationProcessors(this IServiceCollection serviceCollection)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> types = GetAllConfigurationProcessors(assemblies);

            foreach (var type in types)
            {
                serviceCollection.AddTransient(typeof(IConfigurationProcessor), type);
            }

            return serviceCollection;

            static IEnumerable<Type> GetAllConfigurationProcessors(Assembly[] assemblies)
            {
                foreach (var assembly in assemblies)
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.IsAbstract)
                            continue;

                        if (typeof(IConfigurationProcessor).IsAssignableFrom(type))
                        {
                            yield return type;
                        }
                    }
                }
            }
        }
    }
}