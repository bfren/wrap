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

**Verify** (all three must be clean)
1. `dotnet build <project> --no-restore`: no CS1572–CS1574, CS1580, CS1587 or CS1591 warnings.
2. In `bin/Debug/net11.0/Wrap.<Project>.xml`, every `cref="…"` must resolve to the intended symbol prefix (`T:` type, `P:` property, `M:` method). For example, `grep -oE 'cref="[^"]*None[^"]*"'` should show `T:Wrap.None`, not `P:Wrap.M.None`, wherever you meant the type.
3. `dotnet format style -v d Wrap.slnx --no-restore --verify-no-changes --include src/<Project>`. Without `--verify-no-changes` and `--include`, it rewrites files in other projects. Fix what it reports in your project only, and leave warnings elsewhere alone. Apply IDE0001 (simplify name) fixes in crefs. Do **not** auto-apply IDE0004 (redundant cast) anywhere near `Maybe`/`Result`/`Either`. A cast such as `(Maybe<T>)M.None` passed to an `object` parameter changes what gets boxed, so prefer `default(Maybe<T>)` there.

Files use CRLF line endings (`.editorconfig`). If you edit with a tool that writes LF, run `dotnet format whitespace Wrap.slnx` afterwards.

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
