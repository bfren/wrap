# Wrap: Functional Monads for .NET

![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/bfren/wrap?include_prereleases&label=Release) [![Nuget](https://img.shields.io/nuget/dt/Wrap?label=Downloads)](https://www.nuget.org/packages/wrap/) [![GitHub](https://img.shields.io/github/license/bfren/wrap?label=Licence)](https://mit.bfren.dev/2019)<br/>[![Test](https://github.com/bfren/wrap/actions/workflows/test.yml/badge.svg)](https://github.com/bfren/wrap/actions/workflows/test.yml) [![Publish](https://github.com/bfren/wrap/actions/workflows/publish.yml/badge.svg)](https://github.com/bfren/wrap/actions/workflows/publish.yml) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/6194f797b6ff44d68d3ea6624d0a36aa)](https://app.codacy.com/gh/bfren/wrap/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)

Functional monads for C#, to handle null values and failures without exceptions. Chain operations with method calls or LINQ query syntax (railway-oriented programming), with async support throughout.

## Types

- `Maybe<T>`: either `Some<T>` or `None`. Use it instead of `null`.
- `Result<T>`: either `Ok<T>` or `Failure`. Use it instead of throwing exceptions.
- `Either<TLeft, TRight>`: either `Left<TLeft>` or `Right<TRight>`.
- `Id<TId, TValue>`: strongly typed IDs backed by `int`, `uint`, `long`, `ulong` or `Guid`.

`Maybe<T>`, `Result<T>` and `Either<TLeft, TRight>` are C# 15 union types. Each is a struct, so:

- `switch` and `is` patterns work on the cases, and the compiler checks that a `switch` covers them all.
- Wrapping a value in `Some<T>` or `Ok<T>` does not allocate.
- They are never `null`. `default(Maybe<T>)` is `None`, and `default(Result<T>)` is a `Failure`.

## Example

```csharp
using Wrap;
using Wrap.Extensions;

static Result<int> ParseAge(string input) =>
    int.TryParse(input, out var age) && age >= 0
        ? age
        : R.Fail("Age must be a whole number of 0 or more.");

static string Describe(Result<int> age) =>
    age.Map(x => x + 1) switch
    {
        Ok<int>(var next) => $"Next year you'll be {next}.",
        Failure f => f.Message
    };

static string Greet(string? name) =>
    M.Wrap(name) // null becomes None
        .Map(x => $"Hello, {x}!")
        .Match(fNone: () => "Hello, stranger!", fSome: x => x);
```

View the [Wiki](https://github.com/bfren/wrap/wiki) for full documentation.

## Packages

| Package | Contents |
|---|---|
| [`Wrap`](https://www.nuget.org/packages/wrap/) | Everything: the types plus all extension methods (`Bind`, `Map`, `Match`, LINQ, async, JSON converters). Start here. |
| `Wrap.Mvc` | ASP.NET Core MVC model binding for Wrap types. |
| `Wrap.Caching` | Thread-safe caching that uses Wrap types. |
| `Wrap.Id.Dapper` | Dapper type handlers for strongly typed IDs. |
| `Wrap.Testing` | Helpers for unit tests (`AssertSome`, `AssertOk`, `AssertFailure`, value generators). |

The core types are also published on their own: `Wrap.Common`, `Wrap.Maybe`, `Wrap.Result`, `Wrap.Either` and `Wrap.Id`, each with a matching `.Testing` package.

## Requirements

Wrap 2 targets **.NET 11** and needs C# 15, which comes with the .NET 11 SDK.

Wrap 1.x supports .NET 8, 9 and 10. It is in maintenance and gets fixes only.

## History

Wrap brings together projects I have been working on since 2019. It started with a `Result<T>` implementation, which became Maybe (v10 in November 2025), and I decided it was best to have both.

At the same time I was maintaining StrongId (v8.5 in November 2024) so entity IDs didn't have to be primitives. I realised how much `Result<T>`, `Maybe<T>` and `Id<T>` had in common and brought them together in one library: Wrap.

Version 2 rebuilds the core types on the union types introduced in C# 15.

## Licence

> [MIT](https://mit.bfren.dev/2019)

## Copyright

> Copyright (c) 2019-2026 [bfren](https://bfren.dev) (unless otherwise stated)
