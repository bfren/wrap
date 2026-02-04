# Wrap Monads (C#)

![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/bfren/wrap?include_prereleases&label=Release) [![Nuget](https://img.shields.io/nuget/dt/Wrap?label=Downloads)](https://www.nuget.org/packages/wrap/) [![GitHub](https://img.shields.io/github/license/bfren/wrap?label=Licence)](https://mit.bfren.dev/2019)<br/>[![Test](https://github.com/bfren/wrap/actions/workflows/test.yml/badge.svg)](https://github.com/bfren/wrap/actions/workflows/test.yml) [![Publish](https://github.com/bfren/wrap/actions/workflows/publish.yml/badge.svg)](https://github.com/bfren/wrap/actions/workflows/publish.yml) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/6194f797b6ff44d68d3ea6624d0a36aa)](https://app.codacy.com/gh/bfren/wrap/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)

Various monads (including Maybe - see [here](https://en.wikipedia.org/wiki/Monad_(functional_programming)#An_example:_Maybe)) for C# - to handle null values and exceptions better - including Linq support, chaining, and asynchronous programming.  Includes:

- `Either<TLeft, TRight>`
- `Monad<T>`
- `Id<T>` (support for `int`, `uint`, `long`, `ulong` and `Guid` value types)
- `Maybe<T>` (either: `Some<T>` or `None`)
- `Result<T>` (either: `Ok<T>` or `Failure`)

View the [Wiki](https://github.com/bfren/wrap/wiki) for documentation.

## History

Although this library is at v1, it brings together some mature projects I have been working on since 2019. It started with a Result<T> implementation which morphed into Maybe - which reached v10 in November 2025 - and I figured it would be best to have both.

At the same time I was maintaining a StrongId library - which reached v8.5 in November 2024 - to avoid using primitives for Entity IDs. I realised the similarities between Result<T>, Maybe<T> and Id<T> and decided to bring them all together in one library: Wrap.

## Licence

> [MIT](https://mit.bfren.dev/2019)

## Copyright

> Copyright (c) 2019-2026 [bfren](https://bfren.dev) (unless otherwise stated)
