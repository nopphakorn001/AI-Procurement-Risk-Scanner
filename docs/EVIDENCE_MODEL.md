# M2 Evidence Provenance Model

Every risk observation is a `SupplierEvidence` record bound to one supplier and one of five weighted factors:

| Factor | Weight |
|---|---:|
| Identity & ownership | 20% |
| Financial stability | 25% |
| Operational capacity | 20% |
| Compliance | 20% |
| Supply continuity | 15% |

Required provenance fields are source type, source reference, observation time, reviewer, confidence, risk value and summary. Evidence older than 90 days is `STALE` and contributes no coverage.

## Calculation invariant

The newest fresh record per factor is used. Coverage is the sum of weights with fresh evidence. The aggregate score remains `null` / `UNKNOWN` until coverage is exactly 100%. With complete coverage, the score is the deterministic weighted sum of factor risk values.

- Incomplete evidence → `COLLECT_EVIDENCE`.
- Complete evidence → provisional score + `OWNER_REVIEW`.
- Neither state approves, rejects, contacts or purchases from a supplier.
- Manual score writes through the legacy API return `409 MANUAL_SCORE_DISABLED_USE_EVIDENCE`.

AI and automation are not part of M2 and remain `NOT_CONNECTED`.
