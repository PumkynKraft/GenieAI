using Microsoft.SemanticKernel;

namespace SimpleAI.Core
{
    public class EngineBuilder
    {
        private readonly IOrchestratorConfigure orchestrator = OrchestratorFactory.Create();

        public async Task<EngineBuilder> WithLLM(ILLMProvider llm)
        {
            await orchestrator.AddLLM(llm);
            return this;
        }

        public async Task<EngineBuilder> WithLLM(Action<IKernelBuilder> configure)
        {
            await orchestrator.AddLLM(configure);
            return this;
        }

        //public EngineBuilder WithMemory(Action<MemoryBuilder> configure)
        //{
        //    var builder = new MemoryBuilder();
        //    configure(builder);
        //    var memory = builder.Build();
        //    orchestrator.AddMemory(memory);
        //    return this;
        //}

        public async Task<EngineBuilder> WithPlanning(Action<PlanningBuilder> configure)
        {
            var builder = new PlanningBuilder();
            configure(builder);
            await orchestrator.AddPlanning(builder.Build());
            return this;
        }

        public async Task<EngineBuilder> WithActions(Action<ActionBuilder> configure)
        {
            var builder = new ActionBuilder();
            configure(builder);
            await orchestrator.AddActions(new Dictionary<string, IAction[]>(builder.Build()));
            return this;
        }
    }
}