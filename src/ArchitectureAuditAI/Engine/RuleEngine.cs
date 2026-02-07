using ArchitectureAuditAI.Models;
using ArchitectureAuditAI.Rules;

namespace ArchitectureAuditAI.Engine;

public class RuleEngine
{
    private readonly List<IArchitectureRule> _rules = [];

    public IReadOnlyList<IArchitectureRule> Rules => _rules.AsReadOnly();

    public RuleEngine RegisterRule(IArchitectureRule rule)
    {
        _rules.Add(rule);
        return this;
    }

    public RuleEngine RegisterRules(IEnumerable<IArchitectureRule> rules)
    {
        _rules.AddRange(rules);
        return this;
    }

    public AuditReport Run(string rootPath)
    {
        var results = new List<RuleResult>();

        foreach (var rule in _rules)
        {
            results.Add(rule.Evaluate(rootPath));
        }

        return new AuditReport
        {
            TargetPath = rootPath,
            GeneratedAt = DateTime.UtcNow,
            Results = results.AsReadOnly()
        };
    }
}
