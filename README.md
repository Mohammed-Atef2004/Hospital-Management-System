# Hospital Management System

An ASP.NET Core Web API for hospital staff, appointments, departments, medicines, shifts, and patient assignments. The solution uses Domain, Application, Infrastructure, and API projects, with SQL Server persistence, ASP.NET Core Identity, JWT authentication, MediatR, and selected domain-driven design patterns.

> **Status:** This project is under active development. Core staff and scheduling workflows are present. Patient-facing workflows, identity-to-staff linking, and several authentication flows are not implemented yet. See [Known gaps](#known-gaps).

## Solution structure

| Project | Purpose |
| --- | --- |
| `Domain` | Entities, aggregates, value objects, domain rules, events, and persistence interfaces. |
| `Application` | MediatR commands and queries, handlers, validation, DTOs, mappings, and transaction abstractions. |
| `Infrastructure` | EF Core context, SQL Server configuration, repositories, Identity persistence, transaction implementation, and migrations. |
| `PL` | ASP.NET Core API, controllers, authentication, authorization, middleware, and dependency injection. |
| `Domain.Tests` | xUnit tests for domain behavior and selected persistence behavior. |

The Application project depends on Domain abstractions. Infrastructure implements the transaction port used by Application. The API project composes the layers at startup.

### Request and persistence behavior

- MediatR separates commands from queries. Write commands implement `ICommandRequest<T>` and run inside the transaction behavior; queries do not open transactions.
- Failed domain `Result` responses roll back the transaction. Domain events are dispatched after commit. Event delivery is best-effort; there is no outbox yet.
- FluentValidation runs through a MediatR pipeline behavior. EF Core persists domain and ASP.NET Identity data in SQL Server.

## Current modules

| Module | Available capabilities |
| --- | --- |
| Authentication | Registration, username/email login, JWTs, password changes, lockout rules, password history, and encrypted storage for a two-factor secret. |
| Doctors and departments | Doctor lifecycle and profile updates; department creation, updates, activation, and deactivation. |
| Appointments | Create, retrieve, update, cancel, and query appointments. |
| Medicines | Medicine management, stock adjustment, dispensing, expiry, and low-stock queries. |
| Nursing and shifts | Nurse management, shift scheduling, patient assignments and transfers, and vital-sign recording. |
| Patients and records | Patient and clinical-record persistence entities are present; a complete patient API and application workflow are not. |

## API

The API base path is `/api`. Swagger is available in Development. Routes use lowercase names.

| Route | Main operations |
| --- | --- |
| `/api/auth` | Register, login, change password. |
| `/api/doctors` | List, retrieve, register, update, transfer, deactivate, and reactivate doctors. |
| `/api/departments` | Create, list, retrieve, rename, update description, activate, and deactivate. |
| `/api/appointments` | Create, list, retrieve by doctor or patient, reschedule, update, and cancel. |
| `/api/medicines` | List, retrieve, create, update, delete, adjust stock, dispense, and query stock/expiry. |
| `/api/nurses` | List, retrieve, register, update, assign/transfer/end assignments, record vital signs, and update department or head-nurse status. |
| `/api/shifts` | Create, update, remove, retrieve, and query available shifts. |
| `/health` | Anonymous process health endpoint. |

All routes require authentication by default, except registration, login, and `/health`. Department, doctor-management, and shift endpoints require the `Admin` role. Appointment endpoints require `Admin` or `Doctor`. Nurse endpoints and medicine reads/dispensing require `Admin`, `Doctor`, or `Nurse`; medicine creation, updates, deletion, and stock adjustments require `Admin`.

Authentication endpoints are limited to 10 requests per minute per client IP. Patient clinical records are not exposed through the API because identity-to-patient ownership checks are not implemented.

## Requirements

- .NET 8 SDK
- SQL Server 2022 or compatible SQL Server
- EF Core CLI 9.0.9 or a compatible EF Core 9 tool
- Docker Compose (optional, for the container setup)

## Local setup

1. Set `ConnectionStrings__DefaultConnection` for your SQL Server. The checked-in connection string is only a local development default.
2. Development reads its JWT signing key from `PL/appsettings.Development.json`. Outside Development, set `JWT__SigningKey` to a private value of at least 32 UTF-8 bytes and configure `JWT__Issuer` and `JWT__Audience`. Never reuse the development key in another environment.
3. Restore packages, apply migrations, build, test, and run:

```bash
dotnet restore Hospital-Management-System.sln
dotnet ef database update --project Infrastructure --startup-project PL --context ApplicationDbContext
dotnet build Hospital-Management-System.sln
dotnet test Hospital-Management-System.sln
dotnet run --project PL --launch-profile http
```

The HTTP development profile serves the API at `http://localhost:5277`; Swagger is at `http://localhost:5277/swagger`.

The repository has an `InitialCreate` migration. Before applying it to an existing database, compare the current schema with the migration: it is intended to establish the schema and can conflict with tables created outside EF migrations.

## Docker Compose

Set `MSSQL_SA_PASSWORD` and `JWT_SIGNING_KEY` in the environment. The JWT key must contain at least 32 bytes. Then start the API and SQL Server:

```bash
docker compose up --build
```

The API listens on port `8080`, and SQL Server data is stored in the `sqlserver-data` volume. Apply the EF migration to the Compose database before using the API. Compose does not run migrations automatically.

## Configuration and operations

- Environment variables override JSON configuration. Keep production connection strings and signing keys in a deployment secret store.
- Logs are written as structured JSON to standard output.
- The `/health` endpoint checks that the API process responds; it does not verify database connectivity.
- Back up SQL Server data and the ASP.NET Core Data Protection key ring. The key ring protects persisted two-factor secrets; losing it makes those secrets unreadable. Test database and key-ring restores before applying migrations to an existing deployment.
- CI runs restore, Release build, tests, and EF pending-model-change validation for pull requests and pushes to `main` or `master`.

## Tests

The test project targets .NET 8 with the rest of the solution. The existing suite contains 132 tests covering doctor lifecycle, scheduling and appointments, departments, nurses and shifts, patient assignments, user authentication rules and value objects, and two-factor secret persistence conversion.

Coverage remains limited for Application handlers, API authorization and HTTP contracts, concurrent appointment/stock updates, and SQL Server integration workflows. The CI workflow runs the existing tests; it does not replace those missing test suites.

## Known gaps

- Staff accounts are not linked to Doctor/Nurse records, so issued JWTs do not contain a reliable `staff_id` claim. Staff-specific ownership checks must wait for this link.
- Two-factor setup and login verification, email confirmation, password reset/recovery, and refresh tokens do not have complete API flows.
- Patient registration/profile endpoints and ownership rules for medical records and prescriptions are not implemented.
- Appointment availability/conflict checks and medicine stock updates need protection against concurrent writes.
- Application handler, API/infrastructure integration, security, and concurrency tests are incomplete.
- Package versions need a full vulnerability and compatibility review. API versioning, pagination, and an outbox are also future work.
- Billing, notifications, and audit workflows remain out of scope until their domain and access requirements are defined.
