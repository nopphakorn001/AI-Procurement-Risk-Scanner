# Project State

## M1 local standalone and AICompanyOS onboarding — 2026-09-06

- Canonical repository: `D:\Product\AI-Procurement-Risk-Scanner`.
- Branch: `codex/aicompanyos-onboarding`.
- Verified pushed commit: `987cdca`.
- Product identity: `AIProcurementRiskScanner`; product type `SOFTWARE`.
- Local UI: `http://127.0.0.1:8783/`; API: `http://127.0.0.1:8784/`.
- SQLite development storage, supplier CRUD, explicit score/rationale recording, health and read-only control status are ready.
- Paid AI and n8n remain `NOT_CONNECTED`. No supplier outreach, purchasing, contract decision, money movement or external action is automated.
- Unit tests, backend build, frontend build/audit, UI E2E and restart/recovery pass.

## Maturity

`M1_LOCAL_STANDALONE_READY`

The product is usable as a local evidence-recording workspace. It is not yet an autonomous AI procurement decision system and has no proven economic outcome.

## Core Image POC Batch 1 — 2026-09-06

- Core POC screens 01–08 are complete and `READY_FOR_DESIGN_REVIEW` under `docs/poc/core-v1`.
- A generated visual-direction reference is preserved separately from deterministic screen artifacts; it is not evidence or product data.
- Desktop render 8/8, route navigation 8/8, console 0 errors/warnings, and mobile 390 × 844 smoke test pass.
- The POC explicitly preserves `UNKNOWN`, `NO_DATA`, `PARTIAL`, `NOT_CONNECTED` and `COLLECT_EVIDENCE` states and prevents a misleading aggregate score when evidence is incomplete.
- No functional runtime, backend, provider connection, procurement action, supplier outreach or money flow changed.
- Product maturity remains `M1_LOCAL_STANDALONE_READY`; design maturity is `CORE_IMAGE_POC_BATCH_1_READY_FOR_REVIEW`.

## M2 evidence provenance — 2026-09-06

- Structured supplier evidence now persists source type/reference, observed time, reviewer, confidence, risk value, summary and freshness across five weighted risk factors.
- Deterministic calculation is centralized: missing or stale evidence keeps aggregate risk `UNKNOWN`; 100% fresh factor coverage is required before a provisional score can exist.
- Recommendations are limited to `COLLECT_EVIDENCE` or `OWNER_REVIEW`; the runtime does not approve or reject suppliers.
- Legacy manual score writes are blocked with `409 MANUAL_SCORE_DISABLED_USE_EVIDENCE`.
- API contract `0.2.0`, evidence endpoints, additive SQL Server migration, backward-compatible local SQLite schema upgrade, and evidence-governed UI are verified.
- Domain tests 8/8, solution build, frontend build, API integration, UI E2E, responsive smoke and restart/recovery pass. Synthetic test data was removed.
- Maturity: `M2_EVIDENCE_PROVENANCE_READY`. AI and automation remain `NOT_CONNECTED`.
