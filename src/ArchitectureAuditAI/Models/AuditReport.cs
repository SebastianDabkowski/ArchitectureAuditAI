namespace ArchitectureAuditAI.Models;

public class AuditReport
{
    public string TargetPath { get; init; } = string.Empty;
    public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;
    public IReadOnlyList<RuleResult> Results { get; init; } = [];
    public int TotalRules => Results.Count;
    public int PassedRules => Results.Count(r => r.Passed);
    public int FailedRules => Results.Count(r => !r.Passed);
}
