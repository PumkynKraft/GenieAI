namespace SimpleAI.Core
{
    public interface IMemoryProvider
    {
        Task StoreAsync(string key, string value);

        Task<string> RetrieveAsync(string key);
    }
}