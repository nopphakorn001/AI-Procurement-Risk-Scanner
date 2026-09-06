# Session Handoff

- Start hidden local processes with `scripts\start-local.ps1`; stop only this product with `scripts\stop-local.ps1`.
- Open `http://127.0.0.1:8783/`. The status banner must show SQLite `READY`, AI `NOT_CONNECTED`, and Automation `NOT_CONNECTED`.
- Do not present manually recorded scores as AI-generated. Every score requires an evidence/rationale entry.
- The optional `n8n/workflow.json` calls an external provider and must not be imported or activated without credentials, budget and owner approval.
- Next milestone is M2 source-evidence provenance, structured risk factors and deterministic explainable scoring.
- Core Image POC Batch 1 is available at `docs/poc/core-v1/index.html`; serve it locally and review hashes `#dashboard` through `#score`.
- Review images are in `docs/poc/core-v1/screens`; the visual-direction reference is in `docs/poc/visual-direction` and must not be treated as supplier evidence.
- POC acceptance is currently `READY_FOR_DESIGN_REVIEW`, not functional acceptance. After owner design review, continue the remaining Image POC batches before implementing M2 UI contracts.
