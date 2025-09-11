namespace SimpleAI.Core
{
    public class PlanningBuilder
    {
        private readonly PlanningOptions _options = new();

        public PlanningBuilder SetMaxPlanSteps(int steps)
        {
            _options.MaxPlanSteps = steps;
            return this;
        }

        public PlanningBuilder EnableSelfCorrection()
        {
            _options.EnableSelfCorrection = true;
            return this;
        }

        public PlanningBuilder SetRetryAttempts(int attempts)
        {
            _options.RetryAttempts = attempts;
            return this;
        }

        public PlanningOptions Build() => _options;
    }
}