# Session Handoff

- Start hidden local processes with `scripts\start-local.ps1`; stop only this product with `scripts\stop-local.ps1`.
- Open `http://127.0.0.1:8783/`. The status banner must show SQLite `READY`, AI `NOT_CONNECTED`, and Automation `NOT_CONNECTED`.
- Do not present manually recorded scores as AI-generated. Every score requires an evidence/rationale entry.
- The optional `n8n/workflow.json` calls an external provider and must not be imported or activated without credentials, budget and owner approval.
- Next milestone is M2 source-evidence provenance, structured risk factors and deterministic explainable scoring.
