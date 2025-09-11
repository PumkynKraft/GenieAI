using Microsoft.Extensions.DependencyInjection;

namespace SimpleAI.Core
{
    public interface IEngineConfiguration
    {
        IOrchestratorExecutor Configure(IServiceCollection services);
    }
}