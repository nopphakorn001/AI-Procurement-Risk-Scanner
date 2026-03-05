import axios from "axios";
import type { Supplier } from "../types/supplier";

const BASE_URL = import.meta.env.VITE_API_URL ?? "http://localhost:8080";

const http = axios.create({ baseURL: `${BASE_URL}/api` });

export const suppliersApi = {
  getAll: (): Promise<Supplier[]> =>
    http.get<Supplier[]>("/suppliers").then((r) => r.data),

  create: (payload: { name: string; country: string }): Promise<{ id: string }> =>
    http.post<{ id: string }>("/suppliers", payload).then((r) => r.data),

  update: (id: string, payload: { name: string; country: string }): Promise<void> =>
    http.put(`/suppliers/${id}`, payload).then(() => undefined),

  remove: (id: string): Promise<void> =>
    http.delete(`/suppliers/${id}`).then(() => undefined),
};
