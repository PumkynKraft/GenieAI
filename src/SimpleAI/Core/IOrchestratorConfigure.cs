using Microsoft.SemanticKernel;

namespace SimpleAI.Core
{
    // --- Engine  ---
    public interface IOrchestratorConfigure
    {
        Task AddLLM(ILLMProvider llm);

        Task AddLLM(Action<IKernelBuilder> configure);

        //Task AddMemory(MemoryOptions options);

        Task AddPlanning(PlanningOptions options);

        Task AddActions(Dictionary<string, IAction[]> actions);
    }
}