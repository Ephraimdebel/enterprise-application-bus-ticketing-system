# 15-Minute Technical Presentation Guide

This guide is designed to help you demonstrate how the project fulfills the **Tactical DDD**, **Modular Monolith**, and **Reliable Messaging** requirements within your 15-minute window.

---

## ⏱️ Video Breakdown

| Time | Section | Focus & Talking Points |
| :--- | :--- | :--- |
| **0:00 - 1:30** | **Intro & Solution Structure** | **Focus**: Clean Architecture & Separation of Concerns.<br>**Talk about**: How each module is physically isolated into separate projects to prevent circular dependencies and "spaghetti code." Mention that this structure makes the transition to microservices trivial. |
| **1:30 - 4:30** | **Tactical DDD** | **Focus**: Purity and Invariant protection.<br>**Talk about**: Show the **Aggregate Root** (`Booking.cs`) and explain how it protects business rules (e.g., booking confirmation logic). Highlight **Value Objects** being immutable and handling validation, ensuring the Domain layer has zero infrastructure dependencies. |
| **4:30 - 8:30** | **Transactional Outbox** | **Focus**: Reliability and Atomicity.<br>**Talk about**: Explain the "Dual-Write" problem and how we solve it by saving domain events to the outbox table in the *same* database transaction as the aggregate. Show the **Quartz worker** and explain how it handles retries automatically if the message broker is down. |
| **8:30 - 11:30** | **Security** | **Focus**: OIDC and RBAC integration.<br>**Talk about**: Mention Keycloak as our Identity Provider. Show the **Docker auto-import** for the realm settings. Explain the custom middleware that validates JWT tokens and propagates identities across module boundaries. |
| **11:30 - 15:00** | **Live End-to-End Demo** | **Focus**: Integration and Verification.<br>**Talk about**: Walk through the creation of a Trip, a Passenger, and then a Booking. Point out the **inter-module communication** (Booking fetching Passenger info via HTTP) and show the event flowing through RabbitMQ to other modules. |

---

## 🏗️ 1. Solution Architecture (Detailed Notes)
**What to show:** Open the Solution Explorer.
**Talking Points:**
- *"Our solution is organized as a Modular Monolith. Each folder is a Bounded Context: Booking, Trip, Passenger, and Dispute."*
- *"Within the Booking module, we follow Clean Architecture. The **Domain** project has zero dependencies—it represents our pure business logic. Infrastructure handles data via EF Core, but this is hidden from the Application layer using the **Repository Pattern**."*

---

## 🧩 2. Tactical DDD (Detailed Notes)
**What to show:** `src/Booking/Booking.Domain/Booking.cs` and `Money.cs`.
**Talking Points:**
- *"The `Booking` class is our **Aggregate Root**. Notice the private constructor—you cannot create a booking in an invalid state. You must use the `Reserve` factory method."*
- *"Look at the `Money` Value Object. It is immutable and encapsulates both currency and amount. This prevents accidental calculation errors and keeps the domain logic clean and expressive."*
- *"When a booking status changes, we raise a **Domain Event**. This event stays within the transaction until it reaches the Outbox."*

---

## ✉️ 3. Transactional Outbox (Detailed Notes)
**What to show:** `BookingDbContext.cs` (`SaveChangesAsync` method).
**Talking Points:**
- *"Technical reliability is a key requirement. In `SaveChangesAsync`, we automatically pluck domain events from the entity tracker and map them to `OutboxMessages`."*
- *"This ensures **Atomicity**: the Booking is saved, and the event is recorded in the same database commit. If one fails, both fail."*
- *"Our Quartz.NET background job then polls the table. If publication to RabbitMQ fails (e.g., broker is offline), the message stays 'Unprocessed' and the worker will retry every 10 seconds automatically."*

---

## 🔐 4. Security (Detailed Notes)
**What to show:** `docker-compose.yml` (keycloak section) and `Program.cs`.
**Talking Points:**
- *"We used Keycloak for full OAuth2 and OpenID Connect compliance. We've included a `realm-export.json` so the entire security environment initializes automatically in Docker."*
- *"In the API, we use standard Bearer token validation. We also implemented a `RequireAuth` toggle in configuration to allow developers to work locally without token overhead while still keeping the production paths secure."*

---

## 🚀 5. End-to-End Flow Demo
**What to show:** Swagger UIs.
**Talking Points:**
- *Step 1: "I'll create a Trip first. Notice it returns a Trip ID."*
- *Step 2: "Now I'll register a Passenger. This is a separate database and a separate module."*
- *Step 3: "Finally, I'll create the Booking. Watch the logs—the Booking API makes a synchronous call to the Passenger API to verify the ID, then saves the booking and fires an Outbox event which settles eventually in the Dispute and Notification modules."*
