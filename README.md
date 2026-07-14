# Global E-Commerce Platform

## Structure
- **/frontend** → Angular app (product browsing, order placement, admin dashboard)
- **/backend** → ASP.NET Core Web API (orders, payments, shipping, analytics)
- **/docs** → System design document (Markdown + diagrams)

## Getting Started
### Frontend
```bash
cd frontend/ecommerce-frontend
ng serve
# global-ecommerce-platform

## Backend
cd backend/EcommerceBackend
dotnet restore
dotnet run
# Runs at http://localhost:5071

## Frontend
cd frontend
npm ci
ng serve
# Runs at http://localhost:4200
