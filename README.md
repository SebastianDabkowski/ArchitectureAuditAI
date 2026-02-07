# ArchitectureAuditAI

A .NET console application that audits software project architectures against predefined quality rules.
It scans a target directory, evaluates the project against a set of architecture rules, and generates
both console output and an HTML report with the results.

## Features

- **Pluggable rule engine** – register any rule that implements `IArchitectureRule`
- **Built-in rules**
  - *ADR Exists* – verifies that Architecture Decision Records are present (`adr/`, `docs/adr/`, or ADR-named markdown files)
  - *AI-Ready Architecture* – checks for `README.md` and `architecture.md`
- **HTML reporting** – produces a styled HTML report with pass/fail summaries and details
- **CLI interface** – easy to integrate into CI/CD pipelines

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/)

### Build

```bash
dotnet build
```

### Run

```bash
dotnet run --project src/ArchitectureAuditAI [targetPath] [outputPath]
```

| Argument | Default | Description |
|---|---|---|
| `targetPath` | current directory | Directory to audit |
| `outputPath` | `architecture-audit-report.html` | Path for the generated HTML report |

### Test

```bash
dotnet test
```

## Project Structure

```
src/ArchitectureAuditAI/
├── Program.cs                 # CLI entry point
├── Engine/RuleEngine.cs       # Runs registered rules and produces a report
├── Rules/
│   ├── IArchitectureRule.cs   # Rule interface
│   ├── AdrExistsRule.cs       # ADR detection rule
│   └── AiReadyArchitectureRule.cs  # README & architecture.md rule
├── Models/
│   ├── AuditReport.cs         # Aggregated audit report
│   └── RuleResult.cs          # Individual rule result
└── Reporting/
    └── HtmlReportGenerator.cs # HTML report output
```

## Extending

Add a new rule by implementing `IArchitectureRule` and registering it in `Program.cs`:

```csharp
engine.RegisterRule(new MyCustomRule());
```