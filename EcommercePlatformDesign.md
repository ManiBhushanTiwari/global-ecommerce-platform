Global E‑Commerce Order Management Platform
Design Document – Coding Evaluation

1. System Architecture
Frontend (Angular) – product browsing, cart, checkout, order tracking, order history

Backend (C# .NET Core Microservices) – Order, Payment, Shipping, Analytics, User services.

Databases – SQL Server (transactions), NoSQL (real‑time cache), Data Lake (analytics).

Event Bus – Kafka/Azure Event Hub for order/payment/shipping events.

API Gateway – JWT/OAuth authentication, rate limiting.

Deployment – Azure Kubernetes Service + Serverless Functions.

(Insert a simple diagram here showing frontend → API Gateway → microservices → databases/event bus.)

2. Database Schema (High‑Level)
Users: UserId, Name, Email, Address.

Products: ProductId, Name, Price, Stock.

Orders: OrderId, UserId, ProductId, Quantity, Status.

Payments: PaymentId, OrderId, Amount, Status, Gateway.

Shipping: ShippingId, OrderId, Carrier, TrackingNumber, Status.

Analytics (NoSQL): Clickstreams, conversion rates.

3. Microservice Boundaries
Order Service – create/update orders, push events.

Payment Service – Stripe/PayPal integration, retries.

Shipping Service – FedEx/UPS API integration.

Analytics Service – logs, dashboards.

User Service – authentication, profiles.

4. Scaling Strategy
Auto‑scaling pods in AKS.

Read replicas + partitioning for DB.

Redis cache for hot data.

Kafka partitions for parallel processing.

CDN for static assets.

5. Security Considerations
JWT/OAuth2.0 authentication.

PCI‑DSS encryption for payments.

API rate limiting.

GDPR compliance (user consent, right to erasure).

Audit logging.

6. Cost Efficiency
Serverless Functions for lightweight tasks.

Spot instances for non‑critical workloads.

Data tiering for cold analytics.

Autoscaling down during off‑peak hours.