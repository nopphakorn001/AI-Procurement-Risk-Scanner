import { useState } from "react";
import type { Supplier } from "../types/supplier";
import { RiskBadge } from "./RiskBadge";
import { SupplierDetail } from "./SupplierDetail";
import { SupplierForm } from "./SupplierForm";

interface Props {
  suppliers: Supplier[];
  onUpdate: (id: string, data: { name: string; country: string }) => Promise<void>;
  onDelete: (id: string) => Promise<void>;
}

export function SupplierTable({ suppliers, onUpdate, onDelete }: Props) {
  const [editingId, setEditingId] = useState<string | null>(null);
  const [detailSupplier, setDetailSupplier] = useState<Supplier | null>(null);

  return (
    <>
    {detailSupplier && (
      <SupplierDetail supplier={detailSupplier} onClose={() => setDetailSupplier(null)} />
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
