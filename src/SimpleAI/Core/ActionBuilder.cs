namespace SimpleAI.Core
{
    // --- Action Builders ---
    public class ActionBuilder
    {
        private readonly Dictionary<string, IAction[]> _skills = new();

        public void AddSemanticPrompt<T>(T prompt) where T : SemanticPrompt
        {
            if (_skills.ContainsKey(prompt.Skill))
                _skills[prompt.Skill] = _skills[prompt.Skill].Append(prompt).ToArray();

            _skills[prompt.Skill] = [prompt];
        }

        public void AddCodeAction<T>(T action) where T : NativeFunction
        {
            if (_skills.ContainsKey(action.Skill))
                _skills[action.Skill] = _skills[action.Skill].Append(action).ToArray();

            _skills[action.Skill] = [action];
        }

        internal IReadOnlyDictionary<string, IAction[]> Build() => _skills;
    }
}