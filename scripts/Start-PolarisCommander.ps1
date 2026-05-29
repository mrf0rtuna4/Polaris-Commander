<#
.SYNOPSIS
    Safely restores, builds, and launches Polaris Commander with package identity.

.DESCRIPTION
    This helper wraps the known-good WinUI/WinAppSDK launch flow so developers do
    not have to type restore/clean/build/run manually. It detects the current CPU
    architecture, uses the matching MSBuild Platform, and launches the packaged
    app through dotnet run (which invokes the winapp CLI integration from the app
    project).
#>
[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug',

    [switch]$SkipClean,

    [switch]$NoLaunch,

    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$AppArguments
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Resolve-RepositoryRoot {
    $scriptDirectory = Split-Path -Parent $PSCommandPath
    return (Resolve-Path (Join-Path $scriptDirectory '..')).Path
}

function Resolve-Platform {
    switch ($env:PROCESSOR_ARCHITECTURE) {
        'AMD64' { return 'x64' }
        'ARM64' { return 'ARM64' }
        'x86' {
            throw 'x86 is not configured for PolarisCommander.App. Use an x64 or ARM64 PowerShell session.'
        }
        default {
            throw "Unsupported PROCESSOR_ARCHITECTURE '$($env:PROCESSOR_ARCHITECTURE)'. Supported values: AMD64 and ARM64."
        }
    }
}

$repositoryRoot = Resolve-RepositoryRoot
$appProject = Join-Path $repositoryRoot 'src/PolarisCommander.App/PolarisCommander.App.csproj'
$platform = Resolve-Platform

Push-Location $repositoryRoot
try {
    Write-Host "==> Repository: $repositoryRoot"
    Write-Host "==> Configuration: $Configuration"
    Write-Host "==> Platform: $platform"

    dotnet restore $appProject -p:Platform=$platform

    if (-not $SkipClean) {
        dotnet clean $appProject -c $Configuration -p:Platform=$platform --no-restore
    }

    dotnet build $appProject -c $Configuration -p:Platform=$platform --no-restore

    if ($NoLaunch) {
        Write-Host '==> Build completed. Launch skipped because -NoLaunch was specified.'
        return
    }

    $runProperties = @(
        "-p:Platform=$platform"
    )

    if ($AppArguments -and $AppArguments.Count -gt 0) {
        $runProperties += "-p:WinAppLaunchArgs=$($AppArguments -join ' ')"
    }

    Write-Host '==> Launching Polaris Commander with package identity...'
    dotnet run --project $appProject -c $Configuration --no-build @runProperties
}
finally {
    Pop-Location
}