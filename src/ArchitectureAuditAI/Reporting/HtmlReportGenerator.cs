using System.Text;
using System.Web;
using ArchitectureAuditAI.Models;

namespace ArchitectureAuditAI.Reporting;

public class HtmlReportGenerator
{
    public string Generate(AuditReport report)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang=\"en\">");
        sb.AppendLine("<head>");
        sb.AppendLine("  <meta charset=\"UTF-8\">");
        sb.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        sb.AppendLine("  <title>Architecture Audit Report</title>");
        sb.AppendLine("  <style>");
        sb.AppendLine("    body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; margin: 0; padding: 20px; background: #f5f5f5; }");
        sb.AppendLine("    .container { max-width: 900px; margin: 0 auto; }");
        sb.AppendLine("    h1 { color: #333; }");
        sb.AppendLine("    .summary { display: flex; gap: 16px; margin-bottom: 24px; }");
        sb.AppendLine("    .summary-card { padding: 16px 24px; border-radius: 8px; color: white; flex: 1; text-align: center; }");
        sb.AppendLine("    .summary-card.total { background: #5b6abf; }");
        sb.AppendLine("    .summary-card.passed { background: #2e7d32; }");
        sb.AppendLine("    .summary-card.failed { background: #c62828; }");
        sb.AppendLine("    .summary-card h2 { margin: 0 0 4px 0; font-size: 2rem; }");
        sb.AppendLine("    .summary-card p { margin: 0; font-size: 0.9rem; opacity: 0.9; }");
        sb.AppendLine("    .rule { background: white; border-radius: 8px; padding: 16px 20px; margin-bottom: 12px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); }");
        sb.AppendLine("    .rule-header { display: flex; align-items: center; gap: 10px; }");
        sb.AppendLine("    .badge { padding: 4px 12px; border-radius: 12px; font-size: 0.8rem; font-weight: 600; color: white; }");
        sb.AppendLine("    .badge.pass { background: #2e7d32; }");
        sb.AppendLine("    .badge.fail { background: #c62828; }");
        sb.AppendLine("    .rule h3 { margin: 0; color: #333; }");
        sb.AppendLine("    .rule .description { color: #666; margin: 8px 0 4px 0; font-size: 0.9rem; }");
        sb.AppendLine("    .rule .details { color: #444; margin: 4px 0 0 0; }");
        sb.AppendLine("    .meta { color: #999; font-size: 0.85rem; margin-bottom: 20px; }");
        sb.AppendLine("  </style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine("  <div class=\"container\">");
        sb.AppendLine("    <h1>Architecture Audit Report</h1>");
        sb.AppendLine($"    <p class=\"meta\">Target: {HttpUtility.HtmlEncode(report.TargetPath)} | Generated: {report.GeneratedAt:yyyy-MM-dd HH:mm:ss} UTC</p>");

        sb.AppendLine("    <div class=\"summary\">");
        sb.AppendLine($"      <div class=\"summary-card total\"><h2>{report.TotalRules}</h2><p>Total Rules</p></div>");
        sb.AppendLine($"      <div class=\"summary-card passed\"><h2>{report.PassedRules}</h2><p>Passed</p></div>");
        sb.AppendLine($"      <div class=\"summary-card failed\"><h2>{report.FailedRules}</h2><p>Failed</p></div>");
        sb.AppendLine("    </div>");

        foreach (var result in report.Results)
        {
            var badgeClass = result.Passed ? "pass" : "fail";
            var badgeText = result.Passed ? "PASS" : "FAIL";

            sb.AppendLine("    <div class=\"rule\">");
            sb.AppendLine("      <div class=\"rule-header\">");
            sb.AppendLine($"        <span class=\"badge {badgeClass}\">{badgeText}</span>");
            sb.AppendLine($"        <h3>{HttpUtility.HtmlEncode(result.RuleName)}</h3>");
            sb.AppendLine("      </div>");
            sb.AppendLine($"      <p class=\"description\">{HttpUtility.HtmlEncode(result.Description)}</p>");
            sb.AppendLine($"      <p class=\"details\">{HttpUtility.HtmlEncode(result.Details)}</p>");
            sb.AppendLine("    </div>");
        }

        sb.AppendLine("  </div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        return sb.ToString();
    }

    public void GenerateToFile(AuditReport report, string outputPath)
    {
        var html = Generate(report);
        File.WriteAllText(outputPath, html);
    }
}
