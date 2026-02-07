using ArchitectureAuditAI.Engine;
using ArchitectureAuditAI.Models;
using ArchitectureAuditAI.Rules;

namespace ArchitectureAuditAI.Tests;

public class RuleEngineTests : IDisposable
{
    private readonly string _tempDir;

    public RuleEngineTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), $"audit-test-{Guid.NewGuid()}");
        Directory.CreateDirectory(_tempDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, true);
    }

    [Fact]
    public void Run_WithNoRules_ReturnsEmptyReport()
    {
        var engine = new RuleEngine();
        var report = engine.Run(_tempDir);

        Assert.Equal(0, report.TotalRules);
        Assert.Equal(0, report.PassedRules);
        Assert.Equal(0, report.FailedRules);
    }

    [Fact]
    public void Run_WithRegisteredRules_ExecutesAllRules()
    {
        var engine = new RuleEngine();
        engine.RegisterRule(new AdrExistsRule());
        engine.RegisterRule(new AiReadyArchitectureRule());

        var report = engine.Run(_tempDir);

        Assert.Equal(2, report.TotalRules);
    }

    [Fact]
    public void RegisterRule_ReturnsSameEngineForChaining()
    {
        var engine = new RuleEngine();
        var returned = engine.RegisterRule(new AdrExistsRule());

        Assert.Same(engine, returned);
    }

    [Fact]
    public void RegisterRules_AddsMultipleRules()
    {
        var engine = new RuleEngine();
        engine.RegisterRules(new IArchitectureRule[] { new AdrExistsRule(), new AiReadyArchitectureRule() });

        Assert.Equal(2, engine.Rules.Count);
    }

    [Fact]
    public void Run_SetsTargetPathOnReport()
    {
        var engine = new RuleEngine();
        var report = engine.Run(_tempDir);

        Assert.Equal(_tempDir, report.TargetPath);
    }
}
