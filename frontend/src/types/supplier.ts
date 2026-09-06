export interface Supplier {
  id: string;
  name: string;
  country: string;
  riskScore: number | null;
  reasoning: string | null;
}

export type RiskFactor = "Identity" | "FinancialStability" | "OperationalCapacity" | "Compliance" | "SupplyContinuity";
export interface SupplierEvidence { id:string; supplierId:string; factor:RiskFactor; sourceType:string; sourceReference:string; observedAtUtc:string; reviewer:string; confidence:number; riskValue:number; summary:string; createdAtUtc:string; freshness:"FRESH"|"STALE"; }
export interface RiskFactorResult { factor:RiskFactor; weight:number; evidenceStatus:"RECORDED"|"NO_DATA"|"STALE"; riskValue:number|null; confidence:number|null; evidenceId:string|null; observedAtUtc:string|null; contribution:number|null; }
export interface RiskSummary { evidenceCoverage:number; riskScore:number|null; recommendation:"COLLECT_EVIDENCE"|"OWNER_REVIEW"; factors:RiskFactorResult[]; calculationPolicy:string; }
