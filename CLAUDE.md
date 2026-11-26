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
dotnet restore Wrap.slnx

# Build solution (-warnaserror fails on any warning, as CI does)
dotnet build Wrap.slnx --no-restore --configuration Debug -warnaserror

# Run all tests (Microsoft.Testing.Platform, set in global.json, needs --solution)
dotnet test --solution Wrap.slnx --no-restore --no-build --configuration Debug
```

These match the shared CI workflow.

### Supported .NET Versions

Wrap 2.x targets **.NET 11 only** (`net11.0`), because it is built on C# 15 union types. `global.json` pins the .NET 11 SDK.

Wrap 1.x (.NET 8, 9, 10 and 11) lives on a maintenance branch and gets fixes only.

Targeting `net10.0` as well was tried and not merged: it works with PolySharp, but consumers need the .NET 11 SDK and C# 15 on .NET 10, which Microsoft doesn't support, and .NET 10 and 11 both reach end of support in November 2028. The work is on branch `claude/optimistic-cray-bxlo36`. Point .NET 10 users to Wrap 1.x.

## Code Style & Quality

### EditorConfig Rules
The project uses [EditorConfig](https://editorconfig.org) for consistent formatting:
- **Indent**: tabs (size 4) in `.cs` files, 2 spaces in XML/JSON/YAML and project files (`*.csproj`, `*.props`, `*.slnx`), 4 spaces elsewhere
- **Charset**: UTF-8
- **Trailing whitespace**: Trimmed in every file type

`.editorconfig` only holds settings that differ from the .NET defaults (`dotnet new editorconfig`).

### Style Checking
The build is the only style gate. `AnalysisLevel` is `latest-recommended` and `TreatWarningsAsErrors` is on when `CI=true`, so any warning fails CI while a local build only shows it. Run `dotnet build -warnaserror` to see exactly what CI will reject.

Only a few rules are warnings: IDE0005 (unnecessary `using`), IDE0055 (formatting), IDE0060 (unused parameter), IDE1006 (naming), IDE0073 (file header, an error in `src` and `apps`), the CA rules in `latest-recommended`, and the compiler, nullability and XML doc warnings. Every other style preference is a suggestion: it shows in the editor and never fails anything. Use `dotnet format style` to apply fixes, not as a check. Add a pragma only for a single real exception.

### Layout
Put a field, constant or property initialiser on its own line after the `=`, indented one level, just as `=>` bodies are:
```csharp
internal const LogLevel DefaultFailureLevel =
	LogLevel.Warning;

public static readonly FailureKind NotFound =
	new(nameof(NotFound));
```
A field with no initialiser, such as `public static readonly FailureKind Unspecified;`, stays on one line.

### XML Doc Comments
Every project has `GenerateDocumentationFile` on, and the comments become the NuGet IntelliSense text, so they must be accurate. When writing or correcting them:

**Scope and safety**
- Change only `///` lines. Never change code, and never reorder or rename members while editing docs.
- Describe what the code does now. Read the member body before writing. If the code looks wrong, report it rather than documenting it.
- Some behaviour of the union types is intended, not a bug. Document it, don't report it:
  - `HasValue` is always `true`, so the compiler knows the union is never null (`default` is `None`).
  - `Value` boxes the current case and is used by reflection and serialisers.
  - `TryGetValue(out Some<T>)` fills `some` with `Some(default)` and returns `false` when the Maybe is `None`.
  - A `Some<T>` holding `null` becomes `None` when converted to `Maybe<T>`.

  The compiler calls `HasValue`, `Value` and `TryGetValue` for `is` patterns and `switch`, so say so in their docs.

**Content**
- `<summary>`: one sentence in the imperative for methods ("Wrap a value…", "Compare…") and "Gets…" for properties.
- `<param>`, `<typeparam>` and `<returns>`: one each, matching the signature. Properties get no `<returns>`.
- `<returns>` covers every outcome. For example, `!=` is also true when the Maybe is `None`, not only when the values differ.
- `<remarks>`: only for non-obvious behaviour such as null handling, allocation, culture or exhaustiveness. Wrap a code sample in `<code>` and escape generics in it as `&lt;T&gt;`.
- Recheck any remark that justifies a design choice against the language as it is now. For example, "a switch could miss a case, so use Match" is no longer true. A `switch` over a union is checked for exhaustiveness, so `Match` is now shorthand rather than a safety net.
- Write keywords as `<see langword="null"/>`, `true` and `false` the same way, and literals or expressions as `<c>…</c>`.
- Write "Some value type." for a `T` that is a Maybe's value. Name the cases `Some{T}`, `None`, `Ok{T}`, `Failure`, `Left{T}` and `Right{T}`, never `NoneImpl`/`FailureImpl`, which no longer exist.

**Reuse**
- Use `<inheritdoc/>` for interface or override members only after you have opened the base member and read its docs. If they are stale or describe another type, write explicit docs instead and report the stale base.
- Use `<inheritdoc cref="…"/>` for overload families such as `MatchAsync` or the `Parse*` methods. Point it at a member with the same parameter names, otherwise the `<param>` text is lost or wrong.

**crefs**
- Inside `static partial class M`, a bare `None` resolves to the `M.None` property. To mean the type, write `Wrap.None`. `dotnet format` simplifies `global::Wrap.None` to that, and `M.Wrap` doesn't interfere in a cref.
- A cref can also point at a private member (`<see cref="isSome"/>`) from another private member's docs.

**Verify** (both must be clean)
1. `dotnet build <project> --no-restore -warnaserror`: no warnings, including CS1572–CS1574, CS1580, CS1587 and CS1591.
2. In `bin/Debug/net11.0/Wrap.<Project>.xml`, every `cref="…"` must resolve to the intended symbol prefix (`T:` type, `P:` property, `M:` method). For example, `grep -oE 'cref="[^"]*None[^"]*"'` should show `T:Wrap.None`, not `P:Wrap.M.None`, wherever you meant the type.

Apply the IDE0001 (simplify name) suggestions in crefs. Do **not** apply IDE0004 (redundant cast) anywhere near `Maybe`/`Result`/`Either`. A cast such as `(Maybe<T>)M.None` passed to an `object` parameter changes what gets boxed, so prefer `default(Maybe<T>)` there.

Files use LF line endings (`.editorconfig` and `.gitattributes`), so files written by tools that save LF need no conversion. Don't convert files to CRLF.

## Project Structure

```
/src/
  ├── All/                 # Convenience package importing all types
  ├── All.Mvc/             # MVC integration
  ├── All.Testing/         # Testing utilities
  ├── Caching/             # Distributed caching support
  ├── Common/              # Shared utilities
  ├── Either/              # Either<TLeft, TRight> monad (Left/Right)
  ├── Either.Testing/      # Either testing utilities
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
  ├── Tests.Either/
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
- Use of Testing utilities (Either.Testing, Maybe.Testing, Result.Testing, Id.Testing) for creating test values
- Tests run on .NET 11 via GitHub Actions

## Guidelines for Changes

### When Adding Features
1. Add to appropriate sub-project (Maybe, Result, Either, Id, Caching, Common)
2. Add corresponding test project updates
3. Build with `-warnaserror` and fix every warning
4. Ensure all tests pass
5. Update related Testing projects if public APIs change

### When Fixing Bugs
1. Add a test case that reproduces the bug first
2. Fix the bug in the implementation
3. Verify the test passes
4. Run full test suite to check for regressions

### When Updating Dependencies
- Changes to Directory.Packages.props affect all projects
- Verify all tests pass after dependency updates

## License

MIT - Copyright (c) 2019-2026 bfren (unless otherwise stated)
