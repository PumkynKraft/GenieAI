using Microsoft.Extensions.DependencyInjection;

namespace SimpleAI.Core
{
    public static class EngineConfigurationExtension
    {
        public static void AddSimpleAI(this IServiceCollection services, IEngineConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null");

            var executor = configuration.Configure(services);
            var serviceCollection = services.AddSingleton<IEngine>(new Impl.SimpleAI(executor));
        }

        public static void AddSimpleAI(this IServiceCollection services)
        {
            var configurations = typeof(IEngineConfiguration).GetImplementations();

            if (!configurations.Any())
                throw new InvalidOperationException("No IEngineConfiguration implementations found. Please ensure at least one implementation is registered.");

            if (configurations.Count() > 1)
                throw new InvalidOperationException("Multiple IEngineConfiguration implementations found. Please ensure only one implementation is registered.");

            var configuration = configurations.First();

            if (Activator.CreateInstance(configuration) is not IEngineConfiguration instance)
                throw new InvalidOperationException($"Failed to create an instance of {configuration.FullName}, Implement default constructor.");

            var executor = instance.Configure(services);
            services.AddSingleton<IEngine>(new Impl.SimpleAI(executor));
        }
    }
}