$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$runtimeRoot = Join-Path $projectRoot '.runtime'
New-Item -ItemType Directory -Force -Path $runtimeRoot | Out-Null

& (Join-Path $PSScriptRoot 'stop-local.ps1') -Quiet

foreach ($port in @(8783, 8784)) {
    if (Get-NetTCPConnection -State Listen -LocalPort $port -ErrorAction SilentlyContinue) {
        throw "LOCAL_PORT_IN_USE:$port"
    }
}

$env:ASPNETCORE_ENVIRONMENT = 'Development'
$api = Start-Process -FilePath 'dotnet' -ArgumentList @('run','--project',(Join-Path $projectRoot 'src\ProcurementRisk.API\ProcurementRisk.API.csproj'),'--urls','http://127.0.0.1:8784') -WorkingDirectory $projectRoot -WindowStyle Hidden -PassThru -RedirectStandardOutput (Join-Path $runtimeRoot 'api.out.log') -RedirectStandardError (Join-Path $runtimeRoot 'api.err.log')
$env:VITE_API_URL = 'http://127.0.0.1:8784'
$ui = Start-Process -FilePath 'npm.cmd' -ArgumentList @('run','dev','--','--host','127.0.0.1','--port','8783','--strictPort') -WorkingDirectory (Join-Path $projectRoot 'frontend') -WindowStyle Hidden -PassThru -RedirectStandardOutput (Join-Path $runtimeRoot 'ui.out.log') -RedirectStandardError (Join-Path $runtimeRoot 'ui.err.log')

Set-Content -LiteralPath (Join-Path $runtimeRoot 'api.pid') -Value $api.Id
Set-Content -LiteralPath (Join-Path $runtimeRoot 'ui.pid') -Value $ui.Id

$ready = $false
for ($attempt = 0; $attempt -lt 40; $attempt++) {
    try {
        $health = Invoke-RestMethod -Uri 'http://127.0.0.1:8784/api/status' -TimeoutSec 2
        $page = Invoke-WebRequest -UseBasicParsing -Uri 'http://127.0.0.1:8783/' -TimeoutSec 2
        if ($health.productId -eq 'AIProcurementRiskScanner' -and $page.StatusCode -eq 200 -and $page.Content -like '*Procurement Risk Scanner*') { $ready = $true; break }
    } catch { Start-Sleep -Milliseconds 500 }
}

if (-not $ready) { throw "LOCAL_RUNTIME_START_FAILED: inspect $runtimeRoot" }
$apiListener = (Get-NetTCPConnection -State Listen -LocalPort 8784 -ErrorAction Stop | Select-Object -First 1).OwningProcess
$uiListener = (Get-NetTCPConnection -State Listen -LocalPort 8783 -ErrorAction Stop | Select-Object -First 1).OwningProcess
Set-Content -LiteralPath (Join-Path $runtimeRoot 'api.pid') -Value @($api.Id, $apiListener | Select-Object -Unique)
Set-Content -LiteralPath (Join-Path $runtimeRoot 'ui.pid') -Value @($ui.Id, $uiListener | Select-Object -Unique)
Write-Output 'AIProcurementRiskScanner READY'
Write-Output 'UI: http://127.0.0.1:8783/'
Write-Output 'API: http://127.0.0.1:8784/swagger'
