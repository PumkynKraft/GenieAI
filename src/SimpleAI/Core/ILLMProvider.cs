using Microsoft.SemanticKernel;

namespace SimpleAI.Core
{
    // --- Interfaces & Models ---
    public interface ILLMProvider
    {
        Task Configure(IKernelBuilder builder);
    }
}