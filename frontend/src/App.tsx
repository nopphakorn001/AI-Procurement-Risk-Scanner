import { useCallback, useEffect, useState } from "react";
import { statusApi, suppliersApi, type ProductStatus } from "./api/suppliers";
import { SupplierForm } from "./components/SupplierForm";
import { SupplierTable } from "./components/SupplierTable";
import type { Supplier } from "./types/supplier";

const REFRESH_INTERVAL_MS = 30_000;

export default function App() {
  const [suppliers, setSuppliers] = useState<Supplier[]>([]);
  const [showAddForm, setShowAddForm] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [status, setStatus] = useState<ProductStatus | null>(null);

  const loadSuppliers = useCallback(async () => {
    try {
      const data = await suppliersApi.getAll();
      setSuppliers(data);
      setStatus(await statusApi.get());
      setError(null);
    } catch {
      setError("Failed to load suppliers. Is the API running?");
    }
  }, []);

  useEffect(() => {
    loadSuppliers();
    const interval = setInterval(loadSuppliers, REFRESH_INTERVAL_MS);
    return () => clearInterval(interval);
  }, [loadSuppliers]);

  const handleCreate = async (data: { name: string; country: string }) => {
    await suppliersApi.create(data);
    setShowAddForm(false);
    await loadSuppliers();
  };

  const handleUpdate = async (id: string, data: { name: string; country: string }) => {
    await suppliersApi.update(id, data);
    await loadSuppliers();
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Delete this supplier?")) return;
    await suppliersApi.remove(id);
    await loadSuppliers();
  };

  const handleScore = async (id: string, data: { riskScore: number; reasoning: string }) => {
    await suppliersApi.score(id, data);
    await loadSuppliers();
  };

  return (
    <div style={{ maxWidth: "900px", margin: "40px auto", padding: "0 16px", fontFamily: "system-ui, sans-serif" }}>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px" }}>
        <h1 style={{ margin: 0 }}>Procurement Risk Scanner</h1>
        <button onClick={() => setShowAddForm(!showAddForm)}>
          {showAddForm ? "Cancel" : "+ Add Supplier"}
        </button>
      </div>

      <div style={{ background: "#eff6ff", border: "1px solid #bfdbfe", borderRadius: "8px", padding: "12px", marginBottom: "18px", fontSize: "0.85rem" }}>
        <b>Local standalone:</b> {status?.status ?? "CONNECTING"} · Database {status?.database ?? "UNKNOWN"} · AI {status?.aiConnector ?? "UNKNOWN"} · Automation {status?.automationConnector ?? "UNKNOWN"}
        <div style={{ color: "#475569", marginTop: "4px" }}>Scores are recorded only from explicit evidence. No paid AI or external automation is connected.</div>
      </div>

      {error && (
        <div style={{ background: "#fee2e2", color: "#dc2626", padding: "12px", borderRadius: "6px", marginBottom: "16px" }}>
          {error}
        </div>
      )}

      {showAddForm && (
        <div style={{ marginBottom: "24px", padding: "16px", background: "#f9fafb", borderRadius: "8px" }}>
          <SupplierForm onSubmit={handleCreate} onCancel={() => setShowAddForm(false)} />
        </div>
      )}

      <SupplierTable
        suppliers={suppliers}
        onUpdate={handleUpdate}
        onDelete={handleDelete}
        onScore={handleScore}
      />

      <p style={{ color: "#9ca3af", fontSize: "0.75rem", marginTop: "16px" }}>
        Local data refreshes every 30 seconds. Optional AI and n8n connectors remain NOT_CONNECTED until separately configured and approved.
      </p>
    </div>
  );
}
