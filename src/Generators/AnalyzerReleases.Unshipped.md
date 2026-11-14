; Unshipped analyzer release
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
WRAP001 | Wrap.Failure | Error | Method must be static partial and return Failure
WRAP002 | Wrap.Failure | Error | Failure message must not be empty
WRAP003 | Wrap.Failure | Error | Placeholder count must match parameter count
WRAP004 | Wrap.Failure | Warning | Placeholder name should match the parameter at the same position
