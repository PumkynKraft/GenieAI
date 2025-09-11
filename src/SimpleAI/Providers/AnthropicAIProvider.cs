using Microsoft.SemanticKernel;
using SimpleAI.Core;

namespace SimpleAI.Providers
{
    public class AnthropicAIProvider : ILLMProvider
    {
        private readonly string _modelId;
        private readonly string _apiKey;

        public AnthropicAIProvider(string modelId, string apiKey)
        {
            _apiKey = apiKey;
            _modelId = modelId;
        }

        public Task Configure(IKernelBuilder builder)
        {
            builder.AddOpenAIChatCompletion(
            modelId: _modelId, // or "claude-3-sonnet-20240229"
            apiKey: _apiKey);
            return Task.CompletedTask;
        }
    }
}