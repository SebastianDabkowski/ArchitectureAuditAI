using ArchitectureAuditAI.Rules;

namespace ArchitectureAuditAI.Tests;

public class AiReadyArchitectureRuleTests : IDisposable
{
    private readonly string _tempDir;

    public AiReadyArchitectureRuleTests()
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
    public void Evaluate_WhenBothFilesExist_ReturnsPass()
    {
        File.WriteAllText(Path.Combine(_tempDir, "README.md"), "# Project");
        File.WriteAllText(Path.Combine(_tempDir, "architecture.md"), "# Architecture");

        var rule = new AiReadyArchitectureRule();
        var result = rule.Evaluate(_tempDir);

        Assert.True(result.Passed);
        Assert.Contains("README.md", result.Details);
        Assert.Contains("architecture.md", result.Details);
    }

    [Fact]
    public void Evaluate_WhenOnlyReadmeExists_ReturnsFail()
    {
        File.WriteAllText(Path.Combine(_tempDir, "README.md"), "# Project");

        var rule = new AiReadyArchitectureRule();
        var result = rule.Evaluate(_tempDir);

        Assert.False(result.Passed);
        Assert.Contains("Missing", result.Details);
        Assert.Contains("architecture.md", result.Details);
    }

    [Fact]
    public void Evaluate_WhenOnlyArchitectureExists_ReturnsFail()
    {
        File.WriteAllText(Path.Combine(_tempDir, "architecture.md"), "# Architecture");

        var rule = new AiReadyArchitectureRule();
        var result = rule.Evaluate(_tempDir);

        Assert.False(result.Passed);
        Assert.Contains("Missing", result.Details);
        Assert.Contains("README.md", result.Details);
    }

    [Fact]
    public void Evaluate_WhenNoFilesExist_ReturnsFail()
    {
        var rule = new AiReadyArchitectureRule();
        var result = rule.Evaluate(_tempDir);

        Assert.False(result.Passed);
        Assert.Contains("README.md", result.Details);
        Assert.Contains("architecture.md", result.Details);
    }

    [Fact]
    public void Name_ReturnsExpectedValue()
    {
        var rule = new AiReadyArchitectureRule();
        Assert.Equal("AI-Ready Architecture", rule.Name);
    }
}
