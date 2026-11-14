// Wrap: Test Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using BenchmarkDotNet.Running;

// Shared by AppBenchmarkv1 (Wrap 1.x from NuGet) and AppBenchmarkv2 (local 2.x code).
// Run both in Release with the same arguments, e.g. dotnet run -c Release -- --filter *
// shortcut: BenchmarkDotNet 0.15.8 can't build a separate process for net11.0 (NotRecognized runtime),
// so benchmarks always run in-process; drop --inProcess once a BenchmarkDotNet release supports .NET 11
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
	.Run(args.Contains("--inProcess") ? args : [.. args, "--inProcess"]);
