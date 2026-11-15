# Hospital Management System

An ASP.NET Core REST API for core hospital workflows. The solution is organized into Domain, Application, Infrastructure, and Presentation projects. It uses DDD building blocks in selected modules, CQRS with MediatR, Entity Framework Core, SQL Server, and JWT authentication.

> **Project status:** early-stage application under active development. Several modules have working API/use-case paths, while patient workflows, staff identity integration, broader automated tests, and deployment tooling remain planned work.

## Architecture

### Projects and current dependencies

```text
                         ┌────────────────────────┐
                         │ PL / Presentation      │
                         │ ASP.NET Core API       │
                         └──────┬─────────┬───────┘
                                │         │
                                ▼         ▼
                      ┌────────────┐  ┌────────────────┐
                      │Application │─▶│Infrastructure  │
                      │CQRS/use    │  │EF Core/SQL     │
                      │cases       │  │repositories    │
                      └─────┬──────┘  └───────┬────────┘
                            │                 │
                            └────────┬────────┘
                                     ▼
                              ┌──────────┐
                              │ Domain   │
                              │ rules,   │
                              │ entities │
                              └──────────┘
```

The intended direction is for Application to depend on Domain abstractions. **Today, Application also references Infrastructure** for persistence types such as `ApplicationDbContext`; this is a known dependency that should be removed as the architecture is tightened.

| Project | Responsibility |
|---|---|
| `Domain/` | Business entities and aggregates, value objects, rules, domain events, errors, and repository/service interfaces. |
| `Application/` | MediatR commands and queries, handlers, validators, DTOs, mappings, and pipeline behaviors. |
| `Infrastructure/` | EF Core `ApplicationDbContext`, SQL Server mappings, repositories, `UnitOfWork`, Identity persistence, and migrations. |
| `PL/` | ASP.NET Core controllers, middleware, JWT setup, dependency injection, and Swagger. |
| `Domain.Tests/` | xUnit tests for domain behavior and selected persistence behavior. |

### Shared patterns

- **DDD:** aggregate roots encapsulate state changes and raise domain events; value objects and business rules are used in the modules that have been refactored.
- **CQRS:** Application separates commands and queries and uses MediatR handlers.
- **Validation:** FluentValidation runs through a MediatR pipeline behavior.
- **Transactions:** a MediatR transaction behavior and `UnitOfWork` support persistence. The current transaction behavior wraps every request, including queries; limiting it to write commands and standardizing event dispatch is planned. Some authentication handlers also manage persistence and event publication directly.
- **Persistence:** EF Core maps the domain and ASP.NET Identity data to SQL Server.

## Modules

| Module | Implemented capabilities | Current status / known gaps |
|---|---|---|
| **Authentication** | User aggregate, email/username/full-name/phone value objects, uniqueness and lockout rules, password history, lifecycle and security domain events. ASP.NET Identity registration, username/email login, JWT issue/validation, authenticated password change, and encrypted-at-rest TOTP secret persistence. | API flows for two-factor setup/verification, email confirmation, password reset, refresh tokens, and account recovery are not implemented. JWT roles exist, but staff records are not yet linked to user identities and authorization policies by role are not defined. **Known transaction defect:** register and change-password handlers open transactions while the global MediatR transaction behavior also opens one, so these database-backed flows need transaction consolidation and integration verification. |
| **Doctors** | Doctor aggregate behavior, value objects, domain events, department transfer, office/contact/specialization updates, lifecycle actions, schedules, and appointment-related rules/use cases. | Expand aggregate and API tests; review authorization and consistency of validation/error handling across commands. |
| **Departments** | Department lifecycle and information changes, value objects, domain events, CQRS handlers, and controller endpoints. | Automated behavior and API tests are limited; persistence and uniqueness behavior should be covered end to end. |
| **Appointments** | Appointment aggregate, schedule/description/participant value objects, domain events, create/update/cancel use cases, and doctor/patient/upcoming/cancelled queries. | Add tests for overlap, availability, concurrent booking, and cross-aggregate rules; ensure these are enforced consistently at the application and persistence boundaries. |
| **Medicines and dispensing** | Medicine commands and queries, stock adjustment, dispensing, expiry/low-stock queries, and supporting entities for dispense and stock adjustment. | No medicine-focused test suite is present. Bring the module to the same DDD conventions and add coverage for inventory concurrency and dispensing rules. |
| **Nursing, staff, and shifts** | Nurse and shift endpoints/use cases, staff scheduling, patient assignments, transfers, and vital-sign recording. | Current-user context expects a `staff_id` JWT claim, while issued tokens currently carry the domain user and Identity IDs. Link staff accounts to users before relying on staff-specific identity and authorization. Add tests for API policies and persistence workflows. |
| **Patients and clinical records** | Patient, medical-record, prescription, pharmacy, and vital-sign persistence/domain entities; patient repository support is present. | A complete patient Application/API module is not present. Define patient registration/profile workflows and access rules before exposing clinical records. |

## API overview

The API is rooted at `/api`. Swagger UI is enabled in the Development environment.

| Controller | Route | Main operations |
|---|---|---|
| `AuthenticationController` | `/api/auth` | `POST /register`, `POST /login`, `POST /change-password` |
| `DoctorsController` | `/api/Doctors` (`[controller]`) | Register, retrieve, update doctor details, department transfer, deactivate/reactivate |
| `DepartmentsController` | `/api/departments` | Create, retrieve, list, rename/update, activate/deactivate |
| `AppointmentsController` | `/api/Appointments` (`[controller]`) | Create, retrieve/list, query by doctor/patient/status, reschedule, update details, cancel |
| `MedicineController` | `/api/Medicine` (`[controller]`) | CRUD, stock adjustment, dispense, expiry and low-stock queries |
| `NurseController` | `/api/nurses` | Register, update, assign/transfer/end patient assignments, record vital signs, department/head-nurse updates |
| `ShiftController` | `/api/Shifts` | Create/update/remove, retrieve by ID/staff, list available shifts |

All API endpoints require a valid bearer token by default. Registration and login are anonymous; password change requires authentication. Role-specific access rules are a planned follow-up. Route casing and some endpoint conventions still need standardization.

## Technology

| Concern | Technology |
|---|---|
| API, Application, Domain, Infrastructure target | .NET 8 |
| Test project target | .NET 9 |
| Web framework | ASP.NET Core Web API |
| Data access | EF Core 9, SQL Server |
| Identity and tokens | ASP.NET Core Identity, JWT Bearer |
| Use cases | MediatR 11, CQRS |
| Validation and mapping | FluentValidation 12, AutoMapper 12 |
| Tests | xUnit, FluentAssertions |

The test project currently targets .NET 9 while the application projects target .NET 8. Aligning the target frameworks is part of the maintenance plan.

## Getting started

### Prerequisites

- .NET 8 SDK (all solution projects target .NET 8).
- SQL Server, local or containerized.
- EF Core CLI (`dotnet-ef`) compatible with EF Core 9 for migration commands.

### Configure

The default connection string is in `PL/appsettings.json`. Change it for your local SQL Server or set `ConnectionStrings__DefaultConnection` in the environment.

Development uses a development-only JWT signing key from `PL/appsettings.Development.json`. For any non-development environment, configure a private `JWT__SigningKey` with at least 32 UTF-8 bytes, along with the required `JWT__Issuer` and `JWT__Audience` values. Do not use the development key outside local development.

### Restore, migrate, build, test, and run

```bash
dotnet restore Hospital-Management-System.sln
dotnet ef database update --project Infrastructure --startup-project PL --context ApplicationDbContext
dotnet build Hospital-Management-System.sln
dotnet test Hospital-Management-System.sln
dotnet run --project PL --launch-profile http
```

The HTTP development profile serves the API at `http://localhost:5277` and Swagger at `http://localhost:5277/swagger`.

The repository currently contains a single `InitialCreate` migration. Check the target database schema before applying it to an existing database; the initial migration is intended to establish the schema and may conflict with tables created outside EF migrations.

### Operations

- `/health` is an anonymous process health endpoint. Configure the connection string and JWT signing key through environment variables or a secret store outside development.
- API logs use structured JSON on standard output.
- `compose.yaml` starts SQL Server and the API. Set `MSSQL_SA_PASSWORD` and `JWT_SIGNING_KEY` (at least 32 bytes) before running `docker compose up --build`. The SQL data lives in the `sqlserver-data` volume. Apply migrations before using the API.
- Back up the SQL Server database and the data-protection key ring together. The key ring encrypts persisted two-factor secrets; losing it makes those secrets unreadable. Test restores before applying migrations to an existing deployment.
- CI restores, builds, and runs the solution tests for pull requests and pushes to `main` or `master`.

### Authorization and routes

Administrative operations under departments, doctor management, and shifts require the `Admin` role. Appointments require `Admin` or `Doctor`; nurse operations and medicine access require `Admin`, `Doctor`, or `Nurse`, while medicine stock changes require `Admin`. Authentication endpoints are limited to 10 requests per minute per client IP. Route prefixes use lowercase names (`/api/doctors`, `/api/appointments`, `/api/medicines`, `/api/shifts`). Patient-owned clinical data endpoints are not exposed yet; add them only together with a persisted identity-to-patient link and ownership checks.

### Authentication endpoints

- `POST /api/auth/register` — creates a patient account and returns a JWT.
- `POST /api/auth/login` — accepts a username or email and returns a JWT.
- `POST /api/auth/change-password` — authenticated password change.

Send the returned token as `Authorization: Bearer <token>` to protected endpoints.

## Tests

The most recent full solution test run passed **132 tests**. Existing tests cover doctor lifecycle/scheduling/appointments, departments, nurses and staff shifts, patient assignments, user authentication rules/value objects, and two-factor secret persistence conversion.

Test coverage is still uneven: medicine behavior, authentication/Application handlers, controller authorization, HTTP contracts, and real SQL Server persistence flows do not have comprehensive automated tests. Add Application unit tests and API/infrastructure integration tests as those workflows are expanded.

## Development plan

The list below describes work still to do; it is separate from the implemented capabilities above.

### Priority 1 — Make identity and authorization complete

- Link Identity/domain users to Doctor, Nurse, and other staff records and issue the staff identifier claim consistently.
- Add role and policy authorization for administrative, clinical, and patient operations; verify resource ownership for patient data.
- Consolidate database transactions: keep transaction creation in one layer, and apply the transaction behavior only to writes rather than queries.
- Complete two-factor setup and verification, email confirmation, password reset/recovery, and refresh-token flows.
- Add request throttling and security-focused API tests for login, registration, and account lockout.

### Priority 2 — Complete core workflows and their tests

- Add patient registration/profile use cases and API endpoints; define permissions for medical records and prescriptions.
- Expand department and appointment tests, including schedule conflicts, concurrent bookings, and integration with doctor availability.
- Add medicine/dispensing tests and protect stock updates from concurrent writes.
- Add Application handler tests and API/infrastructure integration tests using a test database.

### Priority 3 — Make architecture consistent

- Remove Application's direct dependency on Infrastructure by introducing persistence ports and moving transaction boundaries to a clear application abstraction.
- Standardize aggregates, value objects, domain events, errors, and validation across Medicine, Nurse, Staff, Shift, Patient, and assignment workflows.
- Dispatch domain events consistently after commit; consider an outbox for reliable delivery when external handlers are introduced.
- Standardize route casing, DTOs, error responses, pagination, and API versioning.

### Priority 4 — Prepare for deployment and operations

- Align project target frameworks and update vulnerable/outdated package dependencies.
- Add CI for restore, build, tests, migration validation, and code-quality checks.
- Add Docker/Compose support, structured logging, health checks, and environment-specific configuration/secrets.
- Define database migration and backup procedures for existing deployments.
- Add billing, notifications, and audit workflows only after their domain and access requirements are defined.
