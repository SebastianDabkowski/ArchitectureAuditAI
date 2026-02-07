using ArchitectureAuditAI.Engine;
using ArchitectureAuditAI.Reporting;
using ArchitectureAuditAI.Rules;

string targetPath = args.Length > 0 ? args[0] : Directory.GetCurrentDirectory();
string outputPath = args.Length > 1 ? args[1] : Path.Combine(Directory.GetCurrentDirectory(), "architecture-audit-report.html");

if (!Directory.Exists(targetPath))
{
    Console.Error.WriteLine($"Error: Directory '{targetPath}' does not exist.");
    return 1;
}

Console.WriteLine($"Architecture Audit - Scanning: {targetPath}");
Console.WriteLine();

var engine = new RuleEngine();
engine.RegisterRule(new AdrExistsRule());
engine.RegisterRule(new AiReadyArchitectureRule());

var report = engine.Run(targetPath);

foreach (var result in report.Results)
{
    var status = result.Passed ? "PASS" : "FAIL";
    Console.WriteLine($"  [{status}] {result.RuleName}");
    Console.WriteLine($"         {result.Details}");
    Console.WriteLine();
}

Console.WriteLine($"Results: {report.PassedRules}/{report.TotalRules} rules passed.");
Console.WriteLine();

var generator = new HtmlReportGenerator();
generator.GenerateToFile(report, outputPath);
Console.WriteLine($"HTML report generated: {outputPath}");

return report.FailedRules > 0 ? 1 : 0;
