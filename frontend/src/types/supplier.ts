export interface Supplier {
  id: string;
  name: string;
  country: string;
  riskScore: number | null;
  reasoning: string | null;
}
