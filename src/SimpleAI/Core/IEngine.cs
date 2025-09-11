namespace SimpleAI.Core
{
    public interface IEngine
    {
        Task<string> RunAsync(string input, IDictionary<string, object?> context);
    }
}