using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.PromptTemplates.Liquid;
using SimpleAI.Core;

namespace SimpleAI.Impl
{
    internal class DefaultOrchestrator : IOrchestratorConfigure, IOrchestratorExecutor
    {
        private readonly IKernelBuilder builder;
        private readonly IMemoryCache MemoryCache;

        public DefaultOrchestrator()
        {
            builder = Kernel.CreateBuilder();
            MemoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public Task AddActions(Dictionary<string, IAction[]> actions)
        {
            var funtions = new Dictionary<string, KernelFunction[]>();

            foreach (var action in actions.Values.SelectMany(x => x))
            {
                if (action is SemanticPrompt prompt)
                {
                    var promptFunction = builder.Services.BuildServiceProvider()
                            .GetRequiredService<Kernel>()
                            .CreateFunctionFromPrompt(
                                promptTemplate: prompt.PromptTemplate,
                                functionName: prompt.Name,
                                description: prompt.Description,
                                templateFormat: "liquid",
                                promptTemplateFactory: new LiquidPromptTemplateFactory()
                                );

                    if (funtions.ContainsKey(prompt.Skill))
                        funtions[prompt.Skill] = funtions[prompt.Skill].Append(promptFunction).ToArray();
                    else
                        funtions[prompt.Skill] = new[] { promptFunction };
                }
                else if (action is NativeFunction nativeFunction)
                {
                    var nativeFunc = builder.Services.BuildServiceProvider()
                            .GetRequiredService<Kernel>()
                            .CreateFunctionFromMethod(nativeFunction.Invoke,
                            nativeFunction.Name,
                            nativeFunction.Description);
                    if (funtions.ContainsKey(nativeFunction.Skill))
                        funtions[nativeFunction.Skill] = funtions[nativeFunction.Skill].Append(nativeFunc).ToArray();
                    else
                        funtions[nativeFunction.Skill] = new[] { nativeFunc };
                }
            }

            foreach (var skill in funtions.Keys)
                builder.Plugins.AddFromFunctions(skill, funtions[skill]);

            return Task.CompletedTask;
        }

        public Task AddLLM(ILLMProvider provider)
        {
            return provider.Configure(builder);
        }

        public Task AddLLM(Action<IKernelBuilder> configure)
        {
            configure(builder);
            return Task.CompletedTask;
        }

        //public Task AddMemory(MemoryOptions options) => throw new NotImplementedException();

        public Task AddPlanning(PlanningOptions options)
        {
            MemoryCache.Set("PlanningOptions", options, TimeSpan.FromMinutes(60)); // cache for 30 minutes
            return Task.CompletedTask;
        }

        public async Task<string> Execute(string input, IDictionary<string, object?> dictionary)
        {
            var planningOptions = MemoryCache.Get<PlanningOptions>("PlanningOptions") ?? new PlanningOptions();
            var existingPlan = MemoryCache.Get<ChatHistory>("UserId");
            var kernel = builder.Build();

            PromptExecutionSettings executionSettings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };

            if (existingPlan == null)
            {
                // Create the prompt template using liquid format
                var templateFactory = new LiquidPromptTemplateFactory();
                var promptTemplateConfig = new PromptTemplateConfig()
                {
                    Template = planningOptions.Goal,
                    TemplateFormat = "liquid",
                    Name = "GoalPrompt",
                };
                var promptTemplate = templateFactory.Create(promptTemplateConfig);

                var arguments = new KernelArguments(dictionary, new Dictionary<string, PromptExecutionSettings> { { "default", executionSettings } });
                var renderedPrompt = await promptTemplate.RenderAsync(kernel, arguments);

                existingPlan = new ChatHistory(renderedPrompt);
            }

            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            existingPlan.AddUserMessage(input);

            var result = await chatCompletionService.GetChatMessageContentAsync(existingPlan, executionSettings, kernel);

            existingPlan.AddAssistantMessage(result.Content ?? string.Empty);

            MemoryCache.Set("UserId", existingPlan);

            return result.Content ?? string.Empty;
        }
    }
}