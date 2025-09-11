//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.SemanticKernel;
//using Microsoft.SemanticKernel.ChatCompletion;
//using Microsoft.SemanticKernel.Connectors.Anthropic;
//using Microsoft.SemanticKernel.Connectors.Redis;
//using Microsoft.SemanticKernel.Memory;
//using Microsoft.SemanticKernel.Planners;

//namespace SimpleAI;

//// --- Main Program ---
//public class Program
//{
//    public static async Task Main(string[] args)
//    {
//        // Initialize HTTP client
//        var httpClient = new HttpClient();

//        // Build Semantic Kernel
//        var builder = Kernel.CreateBuilder();

//        // === LLM: Anthropic ===
//        builder.AddOpenAIChatCompletion(
//            modelId: "claude-3-opus-20240229", // or "claude-3-sonnet-20240229"
//            apiKey: "your-api-key");

//        // === Memory: Redis (Short Term) + PostgreSQL (Long Term) ===
//        // Redis for short-term
//        builder.Services.AddKeyedSingleton<IMemoryStore>("short-term", (sp, key) =>
//            new RedisMemoryStore("connection-string"));

//        // PostgreSQL for long-term (mocked)
//        builder.Services.AddKeyedSingleton<IMemoryStore>("long-term", (sp, key) =>
//            new PostgreSQLMemoryStore());

//        // Default memory store (you can choose which to use at runtime)
//        builder.Services.AddSingleton<IMemoryStore>(sp =>
//            sp.GetRequiredKeyedService<IMemoryStore>("short-term"));

//        // === Plugins (Actions) ===
//        var dbContext = new DbContext();
//        builder.Plugins.AddFromObject(new SystemPlugin(httpClient, dbContext), "system");

//        // === Semantic Function: Customer Intent ===
//        const string CustomerIntentPrompt = @"
//            Analyze the customer message and determine their primary intent.
//            Categories: complaint, inquiry, compliment, request, other

//            Customer Message: {{$input}}

//            Intent:";

//        var customerIntentFunction = builder.Services.BuildServiceProvider()
//            .GetRequiredService<Kernel>()
//            .CreateFunctionFromPrompt(
//                promptTemplate: CustomerIntentPrompt,
//                functionName: "CustomerIntent",
//                description: "Classify customer message intent",
//                executionSettings: new PromptExecutionSettings { MaxTokens = 100 });

//        builder.Plugins.AddFromFunctions("CustomerService", new[] { customerIntentFunction });

//        // Build Kernel
//        var kernel = builder.Build();

//        // === Planning (Stepwise Planner) ===
//        var planner = new StepwisePlanner(kernel, new()
//        {
//            MaxIterations = 10, // SetMaxPlanSteps(10)
//            MaxTokens = 4000,
//        });

//        // Enable self-correction? → Handled by planner retry logic or custom loop
//        // SetRetryAttempts(3) → Wrap execution in retry policy

//        // ======== DEMO USAGE ========

//        // 1. Run semantic function directly
//        var intentResult = await kernel.InvokeAsync(
//            customerIntentFunction,
//            new() { ["input"] = "I want to return my order." });

//        Console.WriteLine($"🎯 Customer Intent: {intentResult}");

//        // 2. Run code function (plugin)
//        var dbResult = await kernel.InvokeAsync(
//            "system",
//            "QueryDatabaseAsync",
//            new()
//            {
//                ["sql"] = "SELECT * FROM Orders WHERE Id = @id",
//                ["parameters"] = "{ \"id\": 123 }"
//            });

//        Console.WriteLine($"🗃️ DB Result: {dbResult}");

//        // 3. Use planner (optional)
//        try
//        {
//            var plan = await planner.CreatePlanAsync("Find customer order #123 and email them a return label.");
//            Console.WriteLine("📋 Generated Plan:");
//            Console.WriteLine(plan.ToString());

//            // Execute plan
//            var planResult = await kernel.InvokeAsync(plan);
//            Console.WriteLine($"✅ Plan Result: {planResult}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"❌ Planning failed: {ex.Message}");
//        }

//        Console.WriteLine("✅ Semantic Kernel configured and tested.");
//    }
//}