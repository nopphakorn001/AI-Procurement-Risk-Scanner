# AI Procurement Risk Scanner

Standalone local supplier-risk workspace. It stores supplier records, allows an owner to record an evidence-backed risk score and rationale, and exposes a small read-only control contract for AICompanyOS.

## Current maturity

`M1_LOCAL_STANDALONE_READY`

- Local SQLite is the default development database; SQL Server remains available for Docker deployments.
- Supplier CRUD and explicit assessment recording are available.
- `/health` and `/api/status` expose runtime truth.
- Paid AI and n8n are `NOT_CONNECTED` by default. The product does not imply that an unconfigured connector produced a score.
- No purchasing, supplier outreach, contract decision, or money movement is automated.

## Run locally

Requirements: .NET 8 SDK and Node.js.

```powershell
cd D:\Product\AI-Procurement-Risk-Scanner
npm --prefix frontend install
.\scripts\start-local.ps1
```

Open `http://127.0.0.1:8783/`. Swagger is at `http://127.0.0.1:8784/swagger`.

Stop the runtime without closing unrelated processes:

```powershell
.\scripts\stop-local.ps1
```

Both processes start hidden and write diagnostics under `.runtime/`, preventing command windows from repeatedly appearing.

## Verify

```powershell
dotnet test tests\ProcurementRisk.Domain.Tests\ProcurementRisk.Domain.Tests.csproj
dotnet build src\ProcurementRisk.sln --configuration Release
npm --prefix frontend run build
```

## Optional integrations

Docker Compose provides SQL Server, API and frontend containers. Set `SA_PASSWORD` in a local `.env`; never commit it. The sample `n8n/workflow.json` is optional and requires a separately configured provider key. Importing or activating that workflow can call a paid external API and is therefore outside the default local run.
