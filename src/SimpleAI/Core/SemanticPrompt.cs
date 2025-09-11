using Microsoft.SemanticKernel;

namespace SimpleAI.Core
{
    public abstract class SemanticPrompt : IAction
    {
        public string Skill { get; }
        public string Name { get; }
        public string Description { get; }
        public string PromptTemplate { get; }

        public SemanticPrompt(string skill, string name, string description)
        {
            Skill = skill;
            Name = name;
            Description = description;
            PromptTemplate = GetPromptTemplate();
        }

        protected abstract string GetPromptTemplate();
    }

    public class SemanticPlugin
    {
        public string Name { get; }
        public string Description { get; }
        public List<SemanticPrompt> Prompts { get; }

        public SemanticPlugin(string name, string description)
        {
            Name = name;
            Description = description;
            Prompts = new List<SemanticPrompt>();
        }

        public void AddPrompt(SemanticPrompt prompt)
        {
            Prompts.Add(prompt);
        }
    }

    public abstract class NativeFunction : IAction
    {
        public string Skill { get; }
        public string Description { get; }
        public string Name { get; }

        protected NativeFunction(string skill, string name, string description)
        {
            Skill = skill;
            Name = name;
            Description = description;
        }

        public abstract void Invoke(params object[] obj);
    }
}