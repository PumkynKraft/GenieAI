# <img src="https://github.com/pumkynkraft/GenieAI/blob/master/images/GenieAI.png" alt="simple-ai-icon" style="width:60px;"/> GenieAI

**An intelligent AI framework that makes your applications truly smart through prompt-based actioning and autonomous reasoning.**

GenieAI transforms ordinary applications into intelligent systems by embedding Large Language Models (LLMs) as the thinking brain, enabling natural language processing, automated planning, and self-correcting execution workflows.

## 🧠 Core Concept

GenieAI operates on a simple yet powerful principle: **Semantic Goals → Intelligent Planning → Automated Execution**

Instead of hardcoded logic, your applications can now:
- Understand natural language inputs and intentions
- Create dynamic execution plans to achieve semantic goals
- Execute actions through both AI prompts and code functions
- Self-correct and adapt based on results and feedback

## 🎯 Key Features

### **Pluggable LLM Integration**
- **Universal LLM Support**: Works with OpenAI GPT, Anthropic Claude, local models, and any LLM API
- **Hot-swappable Models**: Change reasoning engines without code modifications
- **Model-agnostic Architecture**: Write once, run with any compatible LLM

### **Dual Action System**
- **Semantic Prompts**: Natural language processing actions for reasoning, analysis, and text generation
- **Code Actions**: Direct function calls for database operations, API integrations, file processing, and system interactions
- **Hybrid Execution**: Seamlessly combine AI reasoning with traditional code execution

### **Intelligent Planning Engine**
- **Goal-oriented Reasoning**: Set high-level semantic goals and let GenieAI determine the execution path
- **Dynamic Plan Generation**: Creates step-by-step execution plans based on available actions and context
- **Adaptive Execution**: Modifies plans in real-time based on intermediate results and changing conditions

### **Flexible Memory System**
- **Short-term Memory**: Scoped to current session/prompt for immediate context and working data
- **Long-term Memory**: Persistent storage for learning, experience accumulation, and knowledge retention
- **Pluggable Memory Providers**: Support for in-memory, database, vector stores, and custom memory backends

### **Self-Correction & Learning**
- **Result Evaluation**: Automatically assess execution outcomes against intended goals
- **Error Recovery**: Detect failures and generate alternative execution strategies
- **Continuous Learning**: Improve future performance based on past executions and outcomes

## 🚀 Quick Start

### Installation

```bash
# .NET CLI
dotnet add package GenieAI

# Package Manager
Install-Package GenieAI

# PackageReference
<PackageReference Include="GenieAI" Version="1.0.0" />
```

### Basic Setup

```csharp
using GenieAI;

// Initialize the framework
var fx = new GenieAIEngine()
    .WithLLM(new OpenAIProvider("your-api-key"))
    .WithMemory(new InMemoryProvider())
    .WithActions(builder => 
    {
        // Register semantic prompts
        builder.AddSemanticAction("analyze_sentiment", "Analyze the emotional tone of the given text");
        builder.AddSemanticAction("summarize_content", "Create a concise summary of the provided content");
        
        // Register code actions
        builder.AddCodeAction("fetch_user", async (userId) => await userService.GetUserAsync(userId));
        builder.AddCodeAction("send_email", async (to, subject, body) => await emailService.SendAsync(to, subject, body));
    });

// Execute a semantic goal
var result = await fx.ExecuteGoalAsync(
    "Analyze the customer feedback and send a personalized response email",
    context: new { 
        feedback = "The product is okay but shipping was really slow",
        customerEmail = "customer@example.com"
    }
);
```

### Advanced Configuration

```csharp
var fx = new GenieAIEngine()
    .WithLLM(new AnthropicProvider("your-api-key"))
    .WithMemory(memory => memory
        .UseShortTerm(new RedisProvider("connection-string"))
        .UseLongTerm(new PostgreSQLProvider("connection-string")))
    .WithPlanning(planning => planning
        .SetMaxPlanSteps(10)
        .EnableSelfCorrection()
        .SetRetryAttempts(3))
    .WithActions(builder => 
    {
        // Semantic actions for AI processing
        builder.AddSemanticPrompt("customer_intent", @"
            Analyze the customer message and determine their primary intent.
            Categories: complaint, inquiry, compliment, request, other
        ");
        
        // Code actions for system integration
        builder.AddCodeAction("query_database", async (sql, parameters) => 
            await dbContext.ExecuteQueryAsync(sql, parameters));
        
        builder.AddCodeAction("call_api", async (endpoint, data) => 
            await httpClient.PostAsync(endpoint, JsonContent.Create(data)));
    });
```

## 📋 Example Use Cases

### Customer Service Automation
```csharp
await fx.ExecuteGoalAsync(
    "Process customer inquiry and provide helpful response",
    context: new { 
        customerMessage = "I can't find my order #12345",
        customerAccountId = "cust_789"
    }
);
// GenieAI will: analyze intent → query order database → generate personalized response
```

### Data Analysis Pipeline
```csharp
await fx.ExecuteGoalAsync(
    "Analyze sales data trends and generate executive summary report",
    context: new { 
        dataSource = "sales_2024",
        reportFormat = "executive_summary"
    }
);
// GenieAI will: fetch data → perform analysis → identify trends → generate formatted report
```

### Content Management
```csharp
await fx.ExecuteGoalAsync(
    "Review and categorize incoming blog submissions for publication",
    context: new { 
        submissions = blogSubmissions,
        categories = availableCategories
    }
);
// GenieAI will: analyze content → check quality → assign categories → schedule publication
```

## 🔧 Architecture

```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│   Application   │───▶│    GenieAI      │───▶│   LLM Provider  │
│                 │    │     Engine       │    │  (OpenAI/etc)   │
└─────────────────┘    └──────────────────┘    └─────────────────┘
                                │
                                ▼
                    ┌──────────────────────┐
                    │   Planning Engine    │
                    │ ┌──────────────────┐ │
                    │ │  Goal Analysis   │ │
                    │ │  Plan Generation │ │
                    │ │  Self-Correction │ │
                    │ └──────────────────┘ │
                    └──────────────────────┘
                                │
                    ┌───────────┴────────────┐
                    ▼                        ▼
            ┌───────────────┐      ┌─────────────────┐
            │ Semantic      │      │ Code Actions    │
            │ Prompts       │      │ (DB, API, etc)  │
            └───────────────┘      └─────────────────┘
                    │                        │
                    └───────────┬────────────┘
                                ▼
                    ┌──────────────────────┐
                    │   Memory System      │
                    │ ┌─────────────────┐  │
                    │ │  Short-term     │  │
                    │ │  Long-term      │  │
                    │ │  Learning       │  │
                    │ └─────────────────┘  │
                    └──────────────────────┘
```

## 📚 Documentation

- **[Getting Started Guide](./docs/getting-started.md)** - Complete setup and first steps
- **[LLM Providers](./docs/llm-providers.md)** - Supported models and configuration
- **[Action System](./docs/actions.md)** - Creating semantic prompts and code actions
- **[Memory System](./docs/memory.md)** - Short-term and long-term memory configuration
- **[Planning Engine](./docs/planning.md)** - Goal setting and execution planning
- **[API Reference](./docs/api-reference.md)** - Complete API documentation

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guide](./CONTRIBUTING.md) for details.

- 🐛 **Bug Reports**: [Issues](https://github.com/pumkynfactory/simplefx/issues)
- 💡 **Feature Requests**: [Discussions](https://github.com/pumkynfactory/simplefx/discussions)
- 📖 **Documentation**: Help improve our docs and examples

## 📄 License

GenieAI is licensed under the [MIT License](./LICENSE).

---

**Made with ❤️ by [The Pumkyn Factory](https://github.com/pumkynfactory)**

*Transform your applications from reactive to intelligent with GenieAI.*
