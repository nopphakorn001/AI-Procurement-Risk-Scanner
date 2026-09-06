# Core Image POC Contract — Batch 1

Status: `READY_FOR_DESIGN_REVIEW`

This batch deliberately validates the product's information architecture before functional development. All records shown are visibly marked as POC examples and are not supplier findings.

## Screens

1. Company Dashboard — action queue, evidence coverage, connector truth.
2. Supplier Directory — search, filtering and decision-readiness scan.
3. Add Supplier / Identity Resolution — duplicate detection and unresolved identity.
4. Supplier Overview — verified findings, missing evidence and next best action.
5. Supplier Evidence — source provenance, freshness, confidence and reviewer contract.
6. Evidence Inbox — checker queue separate from maker activity.
7. Risk Assessment Workspace — quality gates and constrained recommendation.
8. Explainable Score Breakdown — factor weights, evidence state and non-calculation of missing data.

## Invariants

- `UNKNOWN` is never converted to zero.
- `NO_DATA` and `PARTIAL` remain visible at the point of decision.
- No aggregate score is calculated until the configured evidence threshold passes.
- Every evidence item requires source/reference, observation time, reviewer, confidence and freshness.
- Maker cannot independently verify or accept their own work.
- Procurement approval, outreach, purchasing and money movement remain owner-controlled and outside this POC.
- AI and automation remain `NOT_CONNECTED`; the interface must not imply otherwise.

## Visual rules

- Dense, sober evidence workspace; no decorative hero content.
- No gradients, glow, glassmorphism, 3D decoration or meaningless charts.
- Cyan indicates navigation/information, amber indicates incomplete review, red indicates a blocking problem, and green is reserved for verified evidence.
- Status is communicated with words and color, never color alone.
