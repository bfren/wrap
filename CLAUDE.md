# Wrap - Development Guidelines for Claude Code

## Project Overview

**Wrap** is a functional monad library for .NET that brings together several mature projects:
- **Result<T>** - for handling success/failure outcomes
- **Maybe<T>** - for handling null/empty values (Some/None)
- **Either<TLeft, TRight>** - for railway-oriented programming
- **Id<T>** - strong type IDs (int, uint, long, ulong, Guid)
- **Caching** - distributed caching support
- **Common** - shared utilities

Published on [NuGet](https://www.nuget.org/packages/wrap/).

## Development Workflow

### Building & Testing

```bash
# Restore dependencies
dotnet restore Test.csproj

# Check code style (using EditorConfig rules)
dotnet format style -v d Wrap.slnx --no-restore

# Build solution
dotnet build Test.csproj --no-restore --configuration Debug

# Run all tests
dotnet test Test.csproj --no-restore --no-build --configuration Debug
```

### Supported .NET Versions

The library is tested against:
- .NET 8.0.x
- .NET 9.0.x
- .NET 10.0.x

## Code Style & Quality

### EditorConfig Rules
The project uses [EditorConfig](https://editorconfig.org) for consistent formatting:
- **Indent**: 4 spaces (except XML/JSON/YAML: 2 spaces)
- **Charset**: UTF-8
- **Trailing whitespace**: Trimmed
- Project files (*.csproj) use 2-space indentation

### Style Checking
Code style is enforced via `dotnet format style` in the test workflow. Always run this before committing:
```bash
dotnet format style -v d Wrap.slnx --no-restore
```

## Project Structure

```
/src/
  ├── All/                 # Convenience package importing all types
  ├── All.Mvc/             # MVC integration
  ├── All.Testing/         # Testing utilities
  ├── Caching/             # Distributed caching support
  ├── Common/              # Shared utilities
  ├── Either/              # Either<TLeft, TRight> monad
  ├── Id/                  # Strong type IDs
  ├── Id.Dapper/           # Dapper ORM integration for IDs
  ├── Id.Testing/          # ID testing utilities
  ├── Maybe/               # Maybe<T> monad (Some/None)
  ├── Maybe.Testing/       # Maybe testing utilities
  ├── Result/              # Result<T> monad (Ok/Failure)
  └── Result.Testing/      # Result testing utilities

/tests/
  ├── Tests.All/
  ├── Tests.All.Mvc/
  ├── Tests.Caching/
  ├── Tests.Common/
  ├── Tests.Id/
  ├── Tests.Id.Dapper/
  ├── Tests.Maybe/
  └── Tests.Result/
```

## Key Implementation Notes

### Functional Programming Patterns
- **Railway-Oriented Programming**: LINQ extensions and method chaining for error handling
- **Async Support**: Full async/await support throughout
- **Monad Composition**: All types support monadic composition patterns

### Testing
- Each sub-project has corresponding test project
- Use of Testing utilities (Maybe.Testing, Result.Testing, Id.Testing) for creating test values
- Tests run on multiple .NET versions via GitHub Actions

## Guidelines for Changes

### When Adding Features
1. Add to appropriate sub-project (Maybe, Result, Either, Id, Caching, Common)
2. Add corresponding test project updates
3. Run `dotnet format style` to ensure consistent formatting
4. Ensure tests pass on all supported .NET versions
5. Update related Testing projects if public APIs change

### When Fixing Bugs
1. Add a test case that reproduces the bug first
2. Fix the bug in the implementation
3. Verify the test passes
4. Run full test suite to check for regressions

### When Updating Dependencies
- Changes to Directory.Packages.props affect all projects
- Verify tests pass on all .NET versions after dependency updates

## License

MIT - Copyright (c) 2019-2026 bfren (unless otherwise stated)
