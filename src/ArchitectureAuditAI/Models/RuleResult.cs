namespace ArchitectureAuditAI.Models;

public class RuleResult
{
    public string RuleName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool Passed { get; init; }
    public string Details { get; init; } = string.Empty;
}
