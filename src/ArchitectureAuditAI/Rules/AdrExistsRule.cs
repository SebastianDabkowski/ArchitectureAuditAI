using ArchitectureAuditAI.Models;

namespace ArchitectureAuditAI.Rules;

public class AdrExistsRule : IArchitectureRule
{
    public string Name => "ADR Exists";
    public string Description => "Checks whether Architecture Decision Records (ADR) are present in the codebase.";

    public RuleResult Evaluate(string rootPath)
    {
        var adrDirectory = FindAdrDirectory(rootPath);
        var adrFiles = FindAdrFiles(rootPath);

        bool passed = adrDirectory is not null || adrFiles.Count > 0;
        string details = BuildDetails(adrDirectory, adrFiles);

        return new RuleResult
        {
            RuleName = Name,
            Description = Description,
            Passed = passed,
            Details = details
        };
    }

    private static string? FindAdrDirectory(string rootPath)
    {
        var candidates = new[] { "adr", "ADR", "docs/adr", "docs/ADR", "doc/adr", "doc/ADR" };

        foreach (var candidate in candidates)
        {
            var fullPath = Path.Combine(rootPath, candidate);
            if (Directory.Exists(fullPath))
                return candidate;
        }

        return null;
    }

    private static List<string> FindAdrFiles(string rootPath)
    {
        var results = new List<string>();

        try
        {
            var files = Directory.GetFiles(rootPath, "*.md", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).ToLowerInvariant();
                if (fileName.StartsWith("adr-") || fileName.StartsWith("adr_")
                    || fileName.Contains("architecture-decision"))
                {
                    results.Add(Path.GetRelativePath(rootPath, file));
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Skip directories we cannot access
        }

        return results;
    }

    private static string BuildDetails(string? adrDirectory, List<string> adrFiles)
    {
        if (adrDirectory is null && adrFiles.Count == 0)
            return "No ADR directory or ADR files found. Consider adding an 'adr/' directory with architecture decision records.";

        var parts = new List<string>();

        if (adrDirectory is not null)
            parts.Add($"Found ADR directory: '{adrDirectory}'.");

        if (adrFiles.Count > 0)
            parts.Add($"Found {adrFiles.Count} ADR file(s): {string.Join(", ", adrFiles)}.");

        return string.Join(" ", parts);
    }
}
