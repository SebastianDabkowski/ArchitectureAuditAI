using ArchitectureAuditAI.Models;

namespace ArchitectureAuditAI.Rules;

public interface IArchitectureRule
{
    string Name { get; }
    string Description { get; }
    RuleResult Evaluate(string rootPath);
}
