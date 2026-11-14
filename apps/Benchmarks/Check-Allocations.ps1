# Wrap: Test Apps
# Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

# Checks the bytes each v2 benchmark allocates per operation against allocations.json.
# Each benchmark runs in its own process with normal tiered compilation: a combined run, or
# DOTNET_TieredCompilation=0, reports different allocations from a real app (see README.md).
#
#   pwsh apps/Benchmarks/Check-Allocations.ps1          # fail if any benchmark allocates more
#   pwsh apps/Benchmarks/Check-Allocations.ps1 -Update  # rewrite allocations.json from this run

param(
	[switch] $Update,
	# BenchmarkDotNet divides total bytes by operations, so allow a little measurement noise
	[int] $Allowance = 8
)

$ErrorActionPreference = "Stop"
$project = Join-Path $PSScriptRoot ".." "AppBenchmarkv2"
$baselinePath = Join-Path $PSScriptRoot "allocations.json"
$artifactsRoot = Join-Path ([IO.Path]::GetTempPath()) "wrap-allocations"

dotnet build $project -c Release --nologo -v q
if ($LASTEXITCODE -ne 0) { throw "Build failed." }

$names = dotnet run --project $project -c Release --no-build -- --list flat | Where-Object { $_ -match '^Benchmarks\.' }
if ($LASTEXITCODE -ne 0 -or -not $names) { throw "Could not list benchmarks." }

$measured = [ordered]@{}
$names | Sort-Object | ForEach-Object {
	$artifacts = Join-Path $artifactsRoot $_
	Write-Host "Running $_"
	dotnet run --project $project -c Release --no-build -- --filter $_ --job short --exporters json --artifacts $artifacts | Out-Null
	if ($LASTEXITCODE -ne 0) { throw "Benchmark $_ failed." }

	$report = Get-ChildItem (Join-Path $artifacts "results") -Filter "*-report-full-compressed.json" | Get-Content -Raw | ConvertFrom-Json
	$measured[$_] = ($report.Benchmarks | Where-Object FullName -eq $_).Memory.BytesAllocatedPerOperation
}

if ($Update) {
	$measured | ConvertTo-Json | Set-Content $baselinePath
	Write-Host "Updated $baselinePath with $($measured.Count) benchmarks."
	exit 0
}

$baseline = Get-Content $baselinePath -Raw | ConvertFrom-Json -AsHashtable
$rows = @($baseline.Keys) + @($measured.Keys) | Sort-Object -Unique | ForEach-Object {
	$before = $baseline[$_]
	$after = $measured[$_]
	[pscustomobject]@{
		Benchmark = $_
		Baseline  = $before
		Measured  = $after
		Status    = if ($null -eq $after) { "missing" }
			elseif ($null -eq $before) { "new" }
			elseif ($after -gt $before + $Allowance) { "MORE" }
			elseif ($after -lt $before - $Allowance) { "less" }
			else { "ok" }
	}
}

$rows | Format-Table -AutoSize | Out-String | Write-Host

$failed = @($rows | Where-Object Status -in "MORE", "missing", "new")
if (@($rows | Where-Object Status -eq "less")) {
	Write-Host "Some benchmarks allocate less than the baseline: run with -Update and commit allocations.json."
}
if ($failed) {
	Write-Host "$($failed.Count) benchmark(s) allocate more than the baseline, or are missing from it or from this run."
	Write-Host "If the change is intended, run with -Update and commit allocations.json."
	exit 1
}
