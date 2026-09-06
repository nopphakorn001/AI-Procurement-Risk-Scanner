# Project Specification

Goal:
AI Procurement Risk Scanner

Architecture:
.NET 8 Clean Architecture
SQLite local development / SQL Server Docker option
React + Vite standalone UI
Optional n8n automation (NOT_CONNECTED by default)
Evidence-backed risk assessment; optional AI connector is a later governed milestone

Database:
Suppliers
- Id
- Name
- Country
- RiskScore

API:
GET /health
GET /api/status
POST /api/suppliers
GET /api/suppliers
PUT /api/suppliers/{id}
DELETE /api/suppliers/{id}
POST /api/suppliers/{id}/score
