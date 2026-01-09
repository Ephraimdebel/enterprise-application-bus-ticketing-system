Enterprise Bus Ticketing System — Bus Management
=================================================

| Full Name       | ID         |
|-----------------|------------|
| Alpha Degago    | UGR/4592/15 |
| Ashenafi Godana | UGR/7906/14 |
| Belean Redwan   | UGR/5921/15 |
| Ephraim Debel   | UGR/0640/15 |
| Feben Getachew  | UGR/4295/15 |

Project Overview
----------------

This repository implements an enterprise-grade Bus Ticketing and Management System. The project focuses on core bus operations: vehicle and route management, trip scheduling, ticket booking and reservations, payment handling, and reliable inter-service messaging to keep data consistent across bounded contexts.

Bounded Contexts
----------------

- `BusProvider` — Manages vehicles, routes, schedules, capacities, and provider-specific business rules.
- `Trip` — Responsible for creating and publishing scheduled trips (a bus operating a route at a specific time), managing trip lifecycle and seat availability.
- `Booking` — Handles reservations and ticket lifecycle, seat assignments, cancellations, and business validation around bookings.
- `Payment` — Manages payment intents, verification, receipts, and payment status tracking.
- `Notification` — Produces and routes user-facing messages and system notifications (e.g., booking confirmations, payment receipts).

Domain Decomposition
--------------------

Core subdomains
- Trip Scheduling: scheduling and lifecycle of trips, capacity and manifest generation.
- Booking & Reservations: seat reservations, ticket issuance, cancellations and refunds.
- Payment Processing: payment initiation, verification, and reconciliation.

Supporting subdomains
- Provider Management: vehicle and route definitions, provider-specific metadata and constraints.
- Notification: asynchronous delivery of confirmations and alerts.

Generic subdomains
- Reporting & Analytics: aggregated views, operational metrics, and reconciliation helpers.
- System Administration: configuration, logs, and maintenance utilities.

Cross-Context Flows (examples)
------------------------------

1) Trip Publication → Bookings
- Event: `TripCreated` emitted by `Trip` when a new trip is scheduled.
- Effect: `Booking` subscribes to `TripCreated` to open seat selection for that trip; `Notification` issues an informational message when seats become available.

2) Seat Reservation → Payment
- Event: `SeatReserved` emitted by `Booking` when a passenger reserves a seat.
- Effect: `Payment` begins `PaymentInitiated` for the reservation; `Booking` transitions to a pending-payment state and awaits `PaymentCompleted` or `PaymentFailed`.

3) Payment Completed → Ticket Issued
- Event: `PaymentCompleted` emitted by `Payment` after verification.
- Effect: `Booking` marks the reservation as confirmed and `Notification` sends the ticket/receipt.

These flows are intentionally eventual-consistency oriented: services emit domain events and other contexts react asynchronously, allowing each bounded context to remain the authoritative source for its own data.

Architecture & Design Notes
---------------------------

- Project layout follows a per-context modular structure: each context typically contains `Api`, `Application`, `Domain`, and `Infrastructure` projects to separate concerns and encapsulate business logic.
- The system uses a transactional Outbox pattern to reliably publish domain events alongside state changes; see the outbox documentation at [docs/outbox_documentation.md](docs/outbox_documentation.md) for pattern details and rationale.
- Domain event handlers and boundary translation live in the `Application` projects (for example, look for domain event handler classes under `src/Booking/Booking.Application`).
