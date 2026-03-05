import { useState } from "react";
import type { Supplier } from "../types/supplier";

interface Props {
  initial?: Supplier;
  onSubmit: (data: { name: string; country: string }) => Promise<void>;
  onCancel: () => void;
}

export function SupplierForm({ initial, onSubmit, onCancel }: Props) {
  const [name, setName] = useState(initial?.name ?? "");
  const [country, setCountry] = useState(initial?.country ?? "");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      await onSubmit({ name, country });
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} style={{ display: "flex", gap: "8px", flexWrap: "wrap" }}>
      <input
        value={name}
        onChange={(e) => setName(e.target.value)}
        placeholder="Supplier name"
        required
        style={inputStyle}
      />
      <input
        value={country}
        onChange={(e) => setCountry(e.target.value)}
        placeholder="Country"
        required
        style={inputStyle}
      />
      <button type="submit" disabled={loading}>
        {loading ? "Saving..." : initial ? "Update" : "Add Supplier"}
      </button>
      <button type="button" onClick={onCancel}>
        Cancel
      </button>
    </form>
  );
}

const inputStyle: React.CSSProperties = {
  padding: "6px 10px",
  border: "1px solid #d1d5db",
  borderRadius: "6px",
  minWidth: "180px",
};
