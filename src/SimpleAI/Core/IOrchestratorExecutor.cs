namespace SimpleAI.Core
{
    public interface IOrchestratorExecutor
    {
        Task<string> Execute(string input, IDictionary<string, object?> dictionary);
    }
}