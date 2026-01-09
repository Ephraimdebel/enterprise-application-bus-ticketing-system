Enterprise Bus Ticketing System — Bus Management
=================================================

| Full Name       | ID         |
|-----------------|------------|
| Alpha Degago    | UGR/4592/15 |
| Ashenafi Godana | UGR/7906/14 |
| Belean Redwan   | UGR/5921/15 |
| Ephraim Debel   | UGR/0640/15 |
| Feben Getachew  | UGR/4295/15 |

1. Business Problem Description
-----------------------------

Bus operators and ticketing platforms often rely on fragmented or manual processes for scheduling, reservations, and payment reconciliation. Common problems include scattered booking data across systems, double-booking or overbooking due to lack of real-time seat visibility, slow or unreliable payment verification, and poor communication with passengers about confirmations or changes. These challenges cause revenue leakage, customer dissatisfaction, and operational friction for providers.

The Enterprise Bus Ticketing System aims to centralize trip scheduling, booking, provider and vehicle management, and payments in a single backend platform. It focuses on reliable, event-driven integration between bounded contexts so each service remains authoritative for its own data while maintaining system-wide consistency.

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
- `Passenger` — Manages passenger profiles, identification, saved preferences, and loyalty metadata.
- `Dispute` — Manages booking and payment disputes, investigation workflows, and resolution state.
- `Rating` — Collects and aggregate post-trip ratings and feedback for trips, drivers, and providers.

2. Domain Decomposition
-----------------------

🔵 Core Subdomains
These represent the most essential business capabilities of the bus management system.

1. Trip & Schedule Management
	Tracks trip creation, timetable generation, route assignments, vehicle allocation, and seat capacity per trip.
	Business value: Ensures reliable operations and accurate seat availability for customers and operators.

2. Booking & Ticketing
	Handles reservations, seat assignments, ticket issuance, cancellations, rebookings, and booking lifecycle management.
	Business value: Drives revenue, reduces booking errors, and improves passenger experience.

3. Payment & Finance Management
	Manages payment intents, verification, settlement, refunds, receipts, and reconciliation with external payment providers.
	Business value: Secures cash flow and provides transparent financial records for operators and customers.

🟡 Supporting Subdomains
Subdomains that enable the core functionality.

1. Provider & Fleet Management
	Maintains vehicle fleets, driver (or crew) metadata, route definitions, capacity rules and provider-specific constraints.
	Business value: Keeps operational resources accurate and optimizes route/vehicle utilization.

2. User & Role Management
	Controls authentication, authorization, and role-based access for passengers, operators, and administrators.
	Business value: Protects sensitive booking and financial data and enforces least-privilege access.

3. Notification & Communication
	Manages confirmations, reminders, updates and alert delivery channels (SMS, email, push integrations).
	Business value: Improves passenger satisfaction and reduces no-shows and disputes.

4. Passenger Management
	Handles passenger profiles, identity verification, saved preferences, and loyalty programs.
	Business value: Improves user experience and enables personalization and loyalty features.

5. Dispute & Feedback Management
	Handles complaints, dispute workflows, evidence collection, and resolution; also collects post-trip feedback.
	Business value: Provides mechanisms to resolve issues, reduce revenue leakage, and capture quality signals.

⚪ Generic Subdomains
Standard, reusable subdomains.

1. Reporting & Analytics
	Provides aggregated operational metrics, utilization reports, revenue dashboards, and reconciliation helpers.
	Business value: Enables data-driven decision making and financial transparency.

2. System Administration
	Handles configuration, logging, monitoring, health checks, and maintenance utilities.
	Business value: Ensures platform reliability, observability, and maintainability.

3. Bounded Contexts (Core Domain)

BC1: BusProvider Context
Responsibility:
Maintains provider-specific data such as vehicle fleets, route topology, capacity constraints, and provider-level business rules. It is the authoritative source for provider metadata and operational capabilities.

BC2: Trip Context
Responsibility:
Creates and manages scheduled trips (a vehicle operating a route at a specific time), handles seat availability, timetables, and trip lifecycle events (publish, start, complete).

BC3: Booking & Payment Context
Responsibility:
Manages reservation lifecycle, seat holds and confirmations, payment initiation and verification, refunds, and issuance of tickets/receipts. Coordinates with `Trip` for seat state and with `Notification` to communicate with passengers.

BC4: Passenger Context
Responsibility:
Maintains passenger profiles, contact and identity information, saved preferences, and loyalty or wallet information. Serves as the authoritative store for passenger identity and contact data used by Booking and Notification contexts.

BC5: Dispute Context
Responsibility:
Handles complaint and dispute lifecycles for bookings and payments: submission, evidence collection, investigation, resolution, and state tracking. Coordinates with `Payment` for refunds and with `Booking` for state corrections.

BC6: Rating Context
Responsibility:
Collects and aggregates post-trip ratings and feedback for trips, drivers, and providers. Provides summarized metrics to `Reporting` and optionally to `Provider` contexts for operational improvements.

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

User Stories
------------

Story 1: Trip Publication & Seat Availability

Contexts Involved
- `Trip` (primary)
- `BusProvider` (secondary)
- `Booking` (secondary)
- `Notification` (secondary)

Domain Events
- `TripCreated`
- `TripPublished`
- `SeatsAvailable`

Eventual Consistency
- `Trip` creates the schedule and emits `TripCreated` → `BusProvider` may enrich trip metadata and emit `TripPublished`.
- `TripPublished` → `Booking` subscribes and opens seat selection; when seats are ready `SeatsAvailable` is emitted and `Notification` informs interested parties.

Story 2: Seat Reservation, Payment, and Confirmation

Contexts Involved
- `Booking` (primary)
- `Payment` (secondary)
- `Trip` (secondary)
- `Notification` (secondary)

Domain Events
- `SeatReserved`
- `PaymentInitiated`
- `PaymentCompleted`
- `BookingConfirmed`

Eventual Consistency
- `Booking` emits `SeatReserved` when a passenger reserves a seat and moves the booking to a pending-payment state.
- `Payment` reacts to `SeatReserved` by creating a payment flow and emits `PaymentInitiated` → on success it emits `PaymentCompleted`.
- `Booking` listens for `PaymentCompleted` and transitions the reservation to confirmed, then emits `BookingConfirmed` and `Notification` sends the ticket/receipt.

Story 3: Cancellation and Refund

Contexts Involved
- `Booking` (primary)
- `Payment` (secondary)
- `Trip` (secondary)
- `Notification` (secondary)

Domain Events
- `BookingCancelled`
- `RefundInitiated`
- `RefundCompleted`
- `SeatReleased`

Eventual Consistency
- Customer or operator cancels a booking; `Booking` emits `BookingCancelled` and releases the reserved seat locally.
- `Payment` observes `BookingCancelled` and starts refund processing, emitting `RefundInitiated` and later `RefundCompleted`.
- Once refund completes, `Booking` finalizes cancellation state and `Trip` or `Booking` emits `SeatReleased` so availability reflects the freed seat; `Notification` sends confirmations of cancellation and refund.


Architecture & Design Notes
---------------------------

- Project layout follows a per-context modular structure: each context typically contains `Api`, `Application`, `Domain`, and `Infrastructure` projects to separate concerns and encapsulate business logic.
- The system uses a transactional Outbox pattern to reliably publish domain events alongside state changes; see the outbox documentation at [docs/outbox_documentation.md](docs/outbox_documentation.md) for pattern details and rationale.
- Domain event handlers and boundary translation live in the `Application` projects (for example, look for domain event handler classes under `src/Booking/Booking.Application`).
