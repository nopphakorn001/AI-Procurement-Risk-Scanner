import { useState } from "react";
import type { Supplier } from "../types/supplier";
import { RiskBadge } from "./RiskBadge";
import { SupplierDetail } from "./SupplierDetail";
import { SupplierForm } from "./SupplierForm";

interface Props {
  suppliers: Supplier[];
  onUpdate: (id: string, data: { name: string; country: string }) => Promise<void>;
  onDelete: (id: string) => Promise<void>;
  onScore: (id: string, data: { riskScore: number; reasoning: string }) => Promise<void>;
}

export function SupplierTable({ suppliers, onUpdate, onDelete, onScore }: Props) {
  const [editingId, setEditingId] = useState<string | null>(null);
  const [detailSupplier, setDetailSupplier] = useState<Supplier | null>(null);
  const [assessing, setAssessing] = useState<Supplier | null>(null);

  return (
    <>
    {detailSupplier && (
      <SupplierDetail supplier={detailSupplier} onClose={() => setDetailSupplier(null)} />
    )}
    {assessing && (
      <AssessmentForm supplier={assessing} onCancel={() => setAssessing(null)} onSubmit={async (data) => {
        await onScore(assessing.id, data);
        setAssessing(null);
      }} />
    )}
    <table style={{ width: "100%", borderCollapse: "collapse" }}>
      <thead>
        <tr style={{ background: "#f3f4f6" }}>
          <Th>Name</Th>
          <Th>Country</Th>
          <Th>Risk Score</Th>
          <Th>Actions</Th>
        </tr>
      </thead>
      <tbody>
        {suppliers.map((s) => (
          <tr key={s.id} style={{ borderBottom: "1px solid #e5e7eb" }}>
            {editingId === s.id ? (
              <td colSpan={4} style={{ padding: "8px" }}>
                <SupplierForm
                  initial={s}
                  onSubmit={async (data) => {
                    await onUpdate(s.id, data);
                    setEditingId(null);
                  }}
                  onCancel={() => setEditingId(null)}
                />
              </td>
            ) : (
              <>
                <Td>
                  <span
                    onClick={() => setDetailSupplier(s)}
                    style={{ cursor: "pointer", color: "#2563eb", textDecoration: "underline" }}
                  >
                    {s.name}
                  </span>
                </Td>
                <Td>{s.country}</Td>
                <Td>
                  <RiskBadge score={s.riskScore} />
                </Td>
                <Td>
                  <button onClick={() => setEditingId(s.id)} style={{ marginRight: "8px" }}>
                    Edit
                  </button>
                  <button onClick={() => setAssessing(s)} style={{ marginRight: "8px" }}>
                    Assess
                  </button>
                  <button onClick={() => onDelete(s.id)} style={{ color: "#dc2626" }}>
                    Delete
                  </button>
                </Td>
              </>
            )}
          </tr>
        ))}
        {suppliers.length === 0 && (
          <tr>
            <td colSpan={4} style={{ textAlign: "center", padding: "24px", color: "#6b7280" }}>
              No suppliers yet. Add one above.
            </td>
          </tr>
        )}
      </tbody>
    </table>
    </>
  );
}

const cellStyle: React.CSSProperties = { padding: "10px 12px", textAlign: "left" };
const Th = ({ children }: { children: React.ReactNode }) => (
  <th style={{ ...cellStyle, fontWeight: 600 }}>{children}</th>
);
const Td = ({ children }: { children: React.ReactNode }) => (
  <td style={cellStyle}>{children}</td>
);

function AssessmentForm({ supplier, onCancel, onSubmit }: {
  supplier: Supplier;
  onCancel: () => void;
  onSubmit: (data: { riskScore: number; reasoning: string }) => Promise<void>;
}) {
  const [score, setScore] = useState(supplier.riskScore?.toString() ?? "");
  const [reasoning, setReasoning] = useState(supplier.reasoning ?? "");
  const [saving, setSaving] = useState(false);
  return <div style={{ position: "fixed", inset: 0, background: "rgba(15,23,42,.55)", display: "grid", placeItems: "center", zIndex: 1001 }}>
    <form onSubmit={async (event) => { event.preventDefault(); setSaving(true); try { await onSubmit({ riskScore: Number(score), reasoning }); } finally { setSaving(false); } }} style={{ background: "white", padding: "24px", borderRadius: "12px", width: "min(440px, 90vw)" }}>
      <h2 style={{ marginTop: 0 }}>Record assessment</h2>
      <p style={{ color: "#475569" }}>{supplier.name} · Enter only a score supported by reviewed evidence.</p>
      <label>Risk score (0–100)<input type="number" min="0" max="100" step="0.1" required value={score} onChange={(e) => setScore(e.target.value)} style={{ display: "block", width: "100%", boxSizing: "border-box", margin: "6px 0 14px", padding: "8px" }} /></label>
      <label>Evidence / rationale<textarea required maxLength={1000} value={reasoning} onChange={(e) => setReasoning(e.target.value)} style={{ display: "block", width: "100%", boxSizing: "border-box", minHeight: "100px", margin: "6px 0 14px", padding: "8px" }} /></label>
      <div style={{ display: "flex", gap: "8px", justifyContent: "flex-end" }}><button type="button" onClick={onCancel}>Cancel</button><button disabled={saving}>{saving ? "Saving…" : "Save assessment"}</button></div>
    </form>
  </div>;
}
