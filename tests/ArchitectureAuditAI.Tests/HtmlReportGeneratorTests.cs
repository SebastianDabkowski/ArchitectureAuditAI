using ArchitectureAuditAI.Models;
using ArchitectureAuditAI.Reporting;

namespace ArchitectureAuditAI.Tests;

public class HtmlReportGeneratorTests
{
    [Fact]
    public void Generate_ReturnsValidHtml()
    {
        var report = new AuditReport
        {
            TargetPath = "/test/path",
            Results = new List<RuleResult>
            {
                new() { RuleName = "Test Rule", Description = "A test rule", Passed = true, Details = "All good" },
                new() { RuleName = "Failing Rule", Description = "A failing rule", Passed = false, Details = "Not found" }
            }
        };

        var generator = new HtmlReportGenerator();
        var html = generator.Generate(report);

        Assert.Contains("<!DOCTYPE html>", html);
        Assert.Contains("Architecture Audit Report", html);
        Assert.Contains("Test Rule", html);
        Assert.Contains("Failing Rule", html);
        Assert.Contains("PASS", html);
        Assert.Contains("FAIL", html);
        Assert.Contains("/test/path", html);
    }

    [Fact]
    public void Generate_EscapesHtmlInValues()
    {
        var report = new AuditReport
        {
            TargetPath = "<script>alert('xss')</script>",
            Results = new List<RuleResult>
            {
                new() { RuleName = "<b>Bold</b>", Description = "Desc", Passed = true, Details = "Details" }
            }
        };

        var generator = new HtmlReportGenerator();
        var html = generator.Generate(report);

        Assert.DoesNotContain("<script>alert", html);
        Assert.DoesNotContain("<b>Bold</b>", html);
    }

    [Fact]
    public void GenerateToFile_WritesFile()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test-report-{Guid.NewGuid()}.html");

        try
        {
            var report = new AuditReport
            {
                TargetPath = "/test",
                Results = new List<RuleResult>()
            };

            var generator = new HtmlReportGenerator();
            generator.GenerateToFile(report, outputPath);

            Assert.True(File.Exists(outputPath));
            var content = File.ReadAllText(outputPath);
            Assert.Contains("<!DOCTYPE html>", content);
        }
        finally
        {
            if (File.Exists(outputPath))
                File.Delete(outputPath);
        }
    }
}
