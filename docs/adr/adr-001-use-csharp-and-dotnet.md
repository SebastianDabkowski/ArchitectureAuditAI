# ADR-001: Use C# and .NET as the Primary Technology

## Status

Accepted

## Context

ArchitectureAuditAI needs a technology stack for building a cross-platform CLI tool that
scans codebases, applies architecture rules, and generates HTML reports. Key requirements
include strong file-system APIs, cross-platform support, a mature testing ecosystem, and
straightforward extensibility for new rules.

## Decision

We will use **C#** with the **.NET** platform (currently .NET 10) as the primary technology.

## Consequences

- **Cross-platform** – .NET runs on Windows, macOS, and Linux, making the tool usable across
  environments and CI/CD systems.
- **Strong type system** – interfaces such as `IArchitectureRule` enable a clean, discoverable
  extension point for new audit rules.
- **Rich standard library** – `System.IO` provides robust file and directory scanning without
  third-party dependencies.
- **Testing** – xUnit integrates seamlessly for unit and integration testing.
- **Tooling** – the `dotnet` CLI simplifies building, running, and publishing the application.
- **Familiarity** – C# is widely known, which lowers the barrier for contributors.
