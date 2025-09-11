using System.Net.Http.Json;

namespace SimpleAI.Core
{
    public class Program
    {
        //private static readonly HttpClient httpClient = new();
        //private static readonly DbContext dbContext = new();

        public static Task Main(string[] args)
        {
            //            var fx = new EngineBuilder()
            //                .WithLLM(new OpenAIProvider("your-api-key"))
            //                .WithMemory(memory => memory
            //                    .UseShortTerm(new RedisProvider("connection-string"))
            //                    .UseLongTerm(new PostgreSQLProvider("connection-string")))
            //                .WithPlanning(planning => planning
            //                    .SetMaxPlanSteps(10)
            //                    .EnableSelfCorrection()
            //                    .SetRetryAttempts(3))
            //                .WithActions(builder =>
            //                {
            //                    // Semantic actions for AI processing
            //                    builder.AddSemanticPrompt("customer_intent", @"
            //                    Analyze the customer message and determine their primary intent.
            //                    Categories: complaint, inquiry, compliment, request, other
            //                ");

            //                    // Code actions for system integration
            //                    builder.AddCodeAction("query_database", async (string sql, object parameters) =>
            //                        await dbContext.ExecuteQueryAsync(sql, parameters));

            //                    builder.AddCodeAction("call_api", async (string endpoint, object data) =>
            //                        await httpClient.PostAsync(endpoint, JsonContent.Create(data)));
            //                });

            //            // Example usage
            //            var intentResult = await fx.RunSemanticActionAsync("customer_intent", new Dictionary<string, object>
            //        {
            //            { "message", "I want to return my order." }
            //        });
            //            Console.WriteLine($"Intent Analysis: {intentResult}");

            //            var dbResult = await fx.RunSemanticActionAsync("query_database", new Dictionary<string, object>
            //        {
            //            { "param1", "SELECT * FROM Orders WHERE Id = @id" },
            //            { "param2", new { id = 123 } }
            //        });
            //            Console.WriteLine($"DB Result: {dbResult}");

            //            Console.WriteLine("✅ Engine configured and tested.");
            return Task.CompletedTask;
        }
    }
}