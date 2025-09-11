using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SimpleAI.Core
{
    internal static class IoCExtensions
    {
        internal static IServiceCollection AddAsImplementationsOfInterface<TInterface>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var interfaceType = typeof(TInterface);

            var implementationTypes = GetImplementations(interfaceType);

            foreach (var implType in implementationTypes)
            {
                services.TryAddEnumerable(ServiceDescriptor.Describe(interfaceType, implType, lifetime));

                var interfaces = implType.GetInterfaces().Where(p => p.AssemblyQualifiedName != null && !p.AssemblyQualifiedName.Equals(interfaceType.AssemblyQualifiedName));

                foreach (var intrface in interfaces)
                {
                    services.TryAddEnumerable(ServiceDescriptor.Describe(intrface, implType, lifetime));
                }
            }

            return services;
        }

        internal static IEnumerable<Type> GetImplementations(this Type interfaceType)
        {
            var assemblies = AppDomain.CurrentDomain
                            .GetAssemblies()
                            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location));

            var implementationTypes = assemblies
                .SelectMany(a =>
                {
                    try
                    { return a.GetTypes(); }
                    catch { return Array.Empty<Type>(); } // Prevent ReflectionTypeLoadException
                })
                .Where(t =>
                    interfaceType.IsAssignableFrom(t) &&
                    t.IsClass && !t.IsAbstract && !t.IsGenericType);
            return implementationTypes;
        }
    }
}