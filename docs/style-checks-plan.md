# Style checks plan

Plan, 23 November 2026, to replace Wrap's two style gates with one. Steps 1 to 4 and 6 are done (step 3 in bfren/dotnet#3). Step 5 is done locally (clean CI-strict build, 0 warnings, 1651 tests pass); a passing Test run on GitHub is left.

**The problem:** the Style step in CI keeps failing on things that aren't bugs, and each fix (a pragma, a suppression, a severity tweak) adds config that drifts. Of the last 100 Test runs, 16 failed and 15 of those failed at Style:

| Rule | Failed runs | What it was |
| --- | --- | --- |
| IDE0001 (name can be simplified) | 5 | Mostly crefs in doc comments |
| IDE0060 (unused parameter) | 4 | False positive on `[Failure]` methods: the generator isn't built when Style runs |
| IDE0250 (struct can be `readonly`) | 4 | One issue on `InUnion`, re-run four times |
| IDE0004 (redundant cast) | 2 | A rule CLAUDE.md already says is unsafe to apply near `Maybe`/`Result` |
| IDE0290 (use primary constructor) | 1 | |
| IDE0005 (unnecessary `using`) | 1 | |

**Why it goes round in circles:** there are two gates checking the same rules on different inputs.

- The build runs the style rules (`EnforceCodeStyleInBuild` in `src` and `tests`), but only CS8509 and CS8655 are errors, so style warnings never fail it. Locally they're easy to miss.
- `dotnet format style --verify-no-changes` runs the same rules again, before the build, and fails CI. It can't see generated code, so it disagrees with the build (IDE0060).
- `.editorconfig` line 72, `dotnet_analyzer_diagnostic.severity = warning`, turns every analyzer rule into a warning, preferences included. About 75 more lines then set individual rules or options to `warning`, and others carve exceptions back out (IDE0045, IDE0046, IDE0058, IDE0059, IDE0063 to `suggestion`; CA1822 and IDE0072 to `none`). `tests/.editorconfig` turns off 13 more.

**The aim:** one gate, the build. A warning means "this could be a bug or a broken API", and it fails CI. Everything else is a suggestion that shows in the editor and never fails anything. Pragmas and suppressions are left only for real exceptions.

## 1. Make the build the only gate

- [x] Done

**Where:** `src/Directory.Build.props`, `tests/Directory.Build.props`, `apps/Directory.Build.props`

- Add to all three (there is no root `Directory.Build.props`, and each folder's file is used on its own):
  ```xml
  <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
  <AnalysisLevel>latest-recommended</AnalysisLevel>
  <TreatWarningsAsErrors Condition="'$(CI)' == 'true'">true</TreatWarningsAsErrors>
  ```
- `apps` doesn't enforce style in its build today, so this is the first time the build checks it.
- GitHub Actions sets `CI=true`, so CI is strict and local builds only warn. To see exactly what CI will reject, run `dotnet build -warnaserror`.
- Keep the existing `WarningsAsErrors` for CS8509 and CS8655, so a non-exhaustive union switch fails every build, local ones included.
- `TreatWarningsAsErrors` also makes compiler, nullability, XML doc and NuGet warnings fail CI. That suits a library. The full solution builds with 0 warnings today, so nothing should change on day one. Confirm that in step 5.

## 2. Rewrite `.editorconfig`

- [x] Done

**Where:** `.editorconfig`, `src/.editorconfig`, `tests/.editorconfig`

The root `.editorconfig` is RehanSaeed's EditorConfig template (v4.1.0, 2021), written to mirror StyleCop, with Wrap's overrides added at the end. StyleCop.Analyzers isn't referenced anywhere, so the `SA1636` lines do nothing. The "StyleCop" naming block is just built-in IDE1006 naming rules. Of its 104 C# options, 89 have the same value as the .NET default (`dotnet new editorconfig`). Rewrite it as a short file (about 50 lines) holding only what differs from the default:

- File settings: `root = true`, UTF-8, LF line endings, final newline, trimmed whitespace, tabs (size 4) for `.cs`, 2 spaces for XML/JSON/YAML/project files, CRLF for `.cmd`/`.bat`.
- The 15 options where Wrap differs from the default:
  - `var` everywhere: `csharp_style_var_for_built_in_types`, `_when_type_is_apparent`, `_elsewhere` = `true`
  - Expression bodies for methods, constructors, operators and local functions = `true`
  - `dotnet_style_prefer_conditional_expression_over_assignment` and `_over_return` = `false`
  - `dotnet_style_operator_placement_when_wrapping = end_of_line`
  - `dotnet_separate_import_directive_groups = false`
  - `csharp_indent_labels = no_change`, `csharp_indent_case_contents_when_block = false`
  - `csharp_preserve_single_line_statements = false`
  - `csharp_preferred_modifier_order`: take the current default order (it adds `file`, `const` and `required`) rather than the template's stale one
- Settings that the overrides at the end of today's file change, which must stay: no `this.` qualifiers, accessibility modifiers `for_non_interface_members`, `using` directives outside the namespace, file-scoped namespaces.
- Naming rules worth keeping: no non-private fields except `const` and `static readonly` (PascalCase), private fields camelCase, interfaces `I`-prefixed, type parameters `T`-prefixed. Drop the rest of the naming block, the sanity-check rule, the `SA1636` lines, the VB settings and the template's licence block.
- No `dotnet_analyzer_diagnostic.severity = warning` (today's line 72). `latest-recommended` from step 1 becomes the baseline.
- Every style preference is `:suggestion`. The IDE still uses them for formatting and fixes, whatever their severity.
- Keep these as warnings (fail CI), each because it can catch a bug or keep the API clean:
  - IDE0060 unused parameters (`dotnet_code_quality_unused_parameters = all:warning`)
  - IDE0005 unnecessary `using` (cheap, and keeps the IDE0005 build check honest)
  - IDE0055 formatting, so whitespace and line endings stay enforced without a separate `dotnet format whitespace` step
  - IDE0073 file header (`src/.editorconfig`, already `error`)
  - CS1591 and the other XML doc warnings in `src`, which become NuGet IntelliSense text
- Leave out the carve-outs that only existed to soften line 72: the `suggestion` lines for IDE0045, IDE0046, IDE0058, IDE0059, IDE0063 and IDE0130, and CA1822 and IDE0072 `none`, unless the build in step 5 shows one is still needed.
- `tests/.editorconfig`: delete each `none` entry, build, and put back only the ones that `latest-recommended` still raises in test code (CA1707 for underscores in test names is likely to stay).
- Decide: IDE0004 (redundant cast) goes to `suggestion` with the rest. CLAUDE.md already warns that its fix can change what gets boxed, so it should never fail a build.

## 3. Stop CI running `dotnet format`

- [x] Done

**Where:** `bfren/dotnet` `.github/workflows/test.yml` (the shared workflow), Wrap's `.github/workflows/test.yml`

- Add an input to the shared workflow, for example `style: true` by default, and run the Style step only when it is `true`. Other repos keep today's behaviour until each one is moved over.
- In Wrap's `test.yml`, pass `style: false`.
- Once every repo using the shared workflow has moved over, remove the Style step and the input.
- Not checked: which other repos use the shared workflow, and whether `prepare.yml` (used by publish) runs `dotnet format` too.

## 4. Remove the pragmas and suppressions the old setup needed

- [x] Done

- `tests/Tests.Generators/Failure/Failures.cs` and `apps/AppMvc/FailureMessage.cs`: delete `#pragma warning disable IDE0060` and its comment. The build runs the generator before the analyzers, so `[Failure]` parameters are used.
- `tests/Tests.Id.Dapper/_/IntegerIdValue/Convert_Tests.cs`: delete `#pragma warning disable IDE0004` once IDE0004 is a suggestion.
- Re-check the rest after step 5 and keep only those a `latest-recommended` build still raises:
  - `src/Common/Monad.cs`, CA1000 (static member on a generic type)
  - `src/Common/IWithException.cs`, CA1711 (name ends in "Exception")
  - `tests/Tests.Generators/Failure/Failures.cs`, IDE1006 and CA1716 (keyword names, tested on purpose)
  - `tests/Tests.Generators/Failure/GlobalFailures.cs`, CA1050 (global namespace, tested on purpose)
  - `tests/Tests.Id.Dapper/_/GuidIdTypeHandler/Parse_Tests.cs`, xUnit1026

## 5. Verify

- [ ] Done

- From a clean clone (no `bin` or `obj`): `dotnet restore Wrap.slnx`, then `CI=true dotnet build Wrap.slnx --no-restore`, then the tests. A clean clone matters, because the IDE0060 problem only showed up there.
- List any new warnings from `latest-recommended`. For each, fix the code, or set the rule to `suggestion` in `.editorconfig` with a one-line reason. Don't add a pragma unless it's a single real exception.
- Push a branch and check that the Test workflow passes without the Style step.
- Check one deliberate preference break (for example a block-bodied method) shows as a suggestion and doesn't fail CI, and one deliberate warning (an unused `using`) fails CI but not a local build.

## 6. Update the docs

- [x] Done

**Where:** `CLAUDE.md`

- "Building & Testing": drop `dotnet format style` as a check. Say the build is the gate, run `dotnet build -warnaserror` to reproduce CI, and use `dotnet format style` only to apply fixes.
- "Style Checking" and the XML doc "Verify" list: replace the `--verify-no-changes` step with "the build shows no warnings".
- "EditorConfig Rules": `.cs` files are indented with tabs (size 4), not 4 spaces. Today's `.editorconfig` override already sets tabs, so CLAUDE.md is wrong.
- Keep the "Layout" section. It describes how code should look, which still matters, it just no longer fails CI.

## Decisions for you

- Is the list of rules kept as warnings in step 2 right? Everything else becomes a suggestion.
- Should test code be as strict as `src`, or keep a short list of relaxed rules in `tests/.editorconfig`?
- In the shared workflow, should the Style step become opt-in now (default `false`), or stay on for other repos until each one is moved?

## Not checked

- Other repos that use the shared `bfren/dotnet` workflows.
- What `latest-recommended` raises on this code: the build has not been run with it.
- `prepare.yml` and `publish.yml` in `bfren/dotnet`.
- Severities in today's `.editorconfig` against the defaults (only values were compared), and `src/`, `tests/` and `apps/.editorconfig` against the defaults.
- Whether the rewritten `.editorconfig` changes how `dotnet format` formats any existing file. Run `dotnet format whitespace --verify-no-changes` before and after the rewrite.
