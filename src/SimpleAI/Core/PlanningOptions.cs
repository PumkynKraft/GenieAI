namespace SimpleAI.Core
{
    // --- Planning Configuration ---
    public class PlanningOptions
    {
        public int MaxPlanSteps { get; set; } = 5;
        public bool EnableSelfCorrection { get; set; } = false;
        public int RetryAttempts { get; set; } = 1;

        public string Goal { get; set; } = string.Empty;
    }
}