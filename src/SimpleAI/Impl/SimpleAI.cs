using SimpleAI.Core;

namespace SimpleAI.Impl
{
    internal class SimpleAI : IEngine
    {
        private readonly IOrchestratorExecutor _executor;

        public SimpleAI(IOrchestratorExecutor executor)
        {
            _executor = executor;
        }

        public async Task<string> RunAsync(string input, IDictionary<string, object?> context)
        {
            return await _executor.Execute(input, context ?? new Dictionary<string, object?>());
        }
    }
}