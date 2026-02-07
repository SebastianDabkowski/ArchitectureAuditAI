using ArchitectureAuditAI.Rules;

namespace ArchitectureAuditAI.Tests;

public class AdrExistsRuleTests : IDisposable
{
    private readonly string _tempDir;

    public AdrExistsRuleTests()
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
    public void Evaluate_WhenAdrDirectoryExists_ReturnsPass()
    {
        Directory.CreateDirectory(Path.Combine(_tempDir, "adr"));

        var rule = new AdrExistsRule();
        var result = rule.Evaluate(_tempDir);

        Assert.True(result.Passed);
        Assert.Contains("adr", result.Details);
    }

    [Fact]
    public void Evaluate_WhenDocsAdrDirectoryExists_ReturnsPass()
    {
        Directory.CreateDirectory(Path.Combine(_tempDir, "docs", "adr"));

        var rule = new AdrExistsRule();
        var result = rule.Evaluate(_tempDir);

        Assert.True(result.Passed);
        Assert.Contains("docs/adr", result.Details);
    }

    [Fact]
    public void Evaluate_WhenAdrFilesExist_ReturnsPass()
    {
        File.WriteAllText(Path.Combine(_tempDir, "adr-001-use-postgres.md"), "# ADR 001");

        var rule = new AdrExistsRule();
        var result = rule.Evaluate(_tempDir);

        Assert.True(result.Passed);
        Assert.Contains("adr-001", result.Details);
    }

    [Fact]
    public void Evaluate_WhenNoAdrPresent_ReturnsFail()
    {
        var rule = new AdrExistsRule();
        var result = rule.Evaluate(_tempDir);

        Assert.False(result.Passed);
        Assert.Contains("No ADR", result.Details);
    }

    [Fact]
    public void Name_ReturnsExpectedValue()
    {
        var rule = new AdrExistsRule();
        Assert.Equal("ADR Exists", rule.Name);
    }
}
