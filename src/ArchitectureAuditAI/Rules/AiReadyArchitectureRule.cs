using ArchitectureAuditAI.Models;

namespace ArchitectureAuditAI.Rules;

public class AiReadyArchitectureRule : IArchitectureRule
{
    public string Name => "AI-Ready Architecture";
    public string Description => "Checks whether the project has a README.md and an architecture.md file.";

    public RuleResult Evaluate(string rootPath)
    {
        bool hasReadme = FileExistsCaseInsensitive(rootPath, "readme.md");
        bool hasArchitecture = FileExistsCaseInsensitive(rootPath, "architecture.md");

        bool passed = hasReadme && hasArchitecture;
        string details = BuildDetails(hasReadme, hasArchitecture);

        return new RuleResult
        {
            RuleName = Name,
            Description = Description,
            Passed = passed,
            Details = details
        };
    }

    private static bool FileExistsCaseInsensitive(string rootPath, string fileName)
    {
        try
        {
            var files = Directory.GetFiles(rootPath, "*", SearchOption.TopDirectoryOnly);
            return files.Any(f => string.Equals(Path.GetFileName(f), fileName, StringComparison.OrdinalIgnoreCase));
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }

    private static string BuildDetails(bool hasReadme, bool hasArchitecture)
    {
        var missing = new List<string>();
        var found = new List<string>();

        if (hasReadme)
            found.Add("README.md");
        else
            missing.Add("README.md");

        if (hasArchitecture)
            found.Add("architecture.md");
        else
            missing.Add("architecture.md");

        var parts = new List<string>();

        if (found.Count > 0)
            parts.Add($"Found: {string.Join(", ", found)}.");

        if (missing.Count > 0)
            parts.Add($"Missing: {string.Join(", ", missing)}.");

        return string.Join(" ", parts);
    }
}
