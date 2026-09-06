param([switch]$Quiet)
$projectRoot = Split-Path -Parent $PSScriptRoot
$runtimeRoot = Join-Path $projectRoot '.runtime'
foreach ($name in @('api','ui')) {
    $pidPath = Join-Path $runtimeRoot "$name.pid"
    if (-not (Test-Path -LiteralPath $pidPath)) { continue }
    $processIds = @(Get-Content -LiteralPath $pidPath | ForEach-Object { if ($_ -match '^\d+$') { [int]$_ } })
    foreach ($processId in $processIds) {
        $process = Get-Process -Id $processId -ErrorAction SilentlyContinue
        if ($process) { Stop-Process -Id $processId -Force }
    }
    Remove-Item -LiteralPath $pidPath -Force
}
if (-not $Quiet) { Write-Output 'AIProcurementRiskScanner stopped.' }
