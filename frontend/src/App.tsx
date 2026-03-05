import { useCallback, useEffect, useState } from "react";
import { suppliersApi } from "./api/suppliers";
import { SupplierForm } from "./components/SupplierForm";
import { SupplierTable } from "./components/SupplierTable";
import type { Supplier } from "./types/supplier";

const REFRESH_INTERVAL_MS = 30_000;

export default function App() {
  const [suppliers, setSuppliers] = useState<Supplier[]>([]);
  const [showAddForm, setShowAddForm] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadSuppliers = useCallback(async () => {
    try {
      const data = await suppliersApi.getAll();
      setSuppliers(data);
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

  return (
    <div style={{ maxWidth: "900px", margin: "40px auto", padding: "0 16px", fontFamily: "system-ui, sans-serif" }}>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px" }}>
        <h1 style={{ margin: 0 }}>Procurement Risk Scanner</h1>
        <button onClick={() => setShowAddForm(!showAddForm)}>
          {showAddForm ? "Cancel" : "+ Add Supplier"}
        </button>
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
      />

      <p style={{ color: "#9ca3af", fontSize: "0.75rem", marginTop: "16px" }}>
        Risk scores are updated automatically every 5 minutes via n8n + Claude AI. Page refreshes every 30s.
      </p>
    </div>
  );
}
