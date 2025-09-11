using SimpleAI.Impl;

namespace SimpleAI.Core
{
    public class OrchestratorFactory
    {
        public static IOrchestratorConfigure Create()
        {
            return new DefaultOrchestrator();
        }
    }
}