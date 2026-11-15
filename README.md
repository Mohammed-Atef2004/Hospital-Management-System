# 🏥 Hospital Management System (HMS)

A production-grade **ASP.NET Core 8** REST API for managing hospital operations, built with **Domain-Driven Design (DDD)**, **Clean Architecture**, and **CQRS** via MediatR.

---

## 🏗️ Architecture Overview

The solution follows **Clean Architecture** with strict layer separation and zero upward dependencies:

```
┌──────────────────────────────────────────┐
│              Presentation (PL)           │  ← ASP.NET Core Web API
├──────────────────────────────────────────┤
│              Application                 │  ← CQRS · Use Cases · DTOs
├──────────────────────────────────────────┤
│              Infrastructure              │  ← EF Core · Repositories · UoW
├──────────────────────────────────────────┤
│                 Domain                   │  ← Entities · Aggregates · Rules
└──────────────────────────────────────────┘
```

| Layer | Project | Responsibility |
|---|---|---|
| **Domain** | `Domain/` | Entities, Aggregates, Value Objects, Domain Exceptions, Repository Interfaces |
| **Application** | `Application/` | Commands, Queries, Handlers, Validators, DTOs, Pipeline Behaviors |
| **Infrastructure** | `Infrastructure/` | EF Core DbContext, Fluent API Configurations, Repository Implementations, UnitOfWork, Migrations |
| **Presentation** | `PL/` | ASP.NET Core Controllers, Middleware, DI wiring |

---

## ✅ What's Implemented

### Domain Layer — DDD Building Blocks

- **Aggregate Roots** with encapsulated business logic and domain exception enforcement
  - `Doctor` — schedule management, deactivation/reactivation, department transfer, appointment booking with conflict detection
  - `Department` — activate/deactivate, rename, change description with custom domain exceptions
  - `Medicine` — stock management, dispensing with expiry/quantity guards
  - `Nurse` — shift management, patient assignment
- **SharedKernel** base classes — `Entity`, `AggregateRoot`, `ValueObject`, `DomainEvent`, `Result<T>`, `GenericResult`, `Error`
- **Domain Exceptions** — per-aggregate typed exceptions (`DoctorNotAvailableException`, `InsufficientStockException`, `DepartmentAlreadyActiveException`, etc.)
- **Domain Events** — `UserRegisteredDomainEvent`, `UserAccountLockedDomainEvent`, `UserPasswordChangedDomainEvent`, and more
- **Business Rules** — `AccountLockoutRule`, `PasswordMustNotBeReusedRule`, `UserEmailMustBeUniqueRule`, `UsernameMustBeUniqueRule`
- **Value Objects** — `Email`, `FullName`, `PhoneNumber`, `Username` with validation errors

### Application Layer — CQRS with MediatR

**Departments**
- `CreateDepartmentCommand` — with duplicate name guard (throws `ConflictException`)
- `RenameDepartmentCommand`, `ChangeDepartmentDescriptionCommand`
- `ActivateDepartmentCommand`, `DeactivateDepartmentCommand`
- `GetAllDepartmentsQuery`, `GetDepartmentByIdQuery`

**Doctors**
- `RegisterNewDoctorCommand` — creates domain aggregate via `Doctor.Create()`
- `DeactivateDoctorCommand`, `ReactivateDoctorCommand`
- `DefineDoctorWeeklyShiftCommand`, `UpdateDoctorWorkShiftCommand`, `RemoveDoctorWeeklyShiftCommand`
- `TransferDoctorToDepartmentCommand`, `RelocateDoctorOfficeCommand`
- `UpdateDoctorSpecializationCommand`, `UpdateDoctorContactDetailsCommand`, `CorrctDoctorNameCommand`
- `GetAllDoctorsQuery`, `GetDoctorByIdQuery`

**Medicines**
- `AddMedicineCommand`, `UpdateMedicineInfoCommand`, `DeleteMedicineCommand`
- `MedicineAdjustStockCommand` — increase/decrease stock with reason tracking
- `DispenseMedicineCommand` — validates stock and expiry before dispensing
- `GetAllMedicinesQuery`, `GetMedicineByIdQuery`, `GetMedicineWithDetailsQuery`
- `GetMedicinesLowStockQuery`, `GetExpiredMedicinesQuery`, `GetExpiringMedicinesNextMonthQuery`

**Nurses**
- `AddNurseCommand`, `UpdateNurseCommand`, `DeleteNurseCommand`
- `CreateShiftCommand`, `UpdateShiftCommand`, `DeleteShiftCommand`
- `GetAllNursesQuery`, `GetNurseByIdQuery`, `GetNursePatientsQuery`, `GetNurseShiftsQuery`

**Pipeline Behaviors**
- `ValidationBehavior<TRequest, TResponse>` — FluentValidation integration
- `TransactionBehavior<TRequest, TResponse>` — automatic DB transaction per command

### Infrastructure Layer

- **EF Core 9** with SQL Server, Fluent API configurations for all entities
- **TPT (Table-Per-Type)** mapping strategy for Staff hierarchy (Doctor, Nurse)
- **Generic Repository** pattern with `IGenericRepository<T>` and `IUnitOfWork`
- **Repositories** — `DoctorRepository`, `DepartmentRepository`, `MedicineRepository`, `NurseRepository`, `PatientRepository`, `ScheduleRepository`
- **Migrations** — 3 migrations covering full schema including Dispense/StockAdjustment and Schedule tables
- **Soft Delete** support via `ISoftDeletable` and `IAudiatable` interfaces
- **Unique constraints** — schedule conflict detection at DB level (`DayOfWeek + StartTime + EndTime`)

### Presentation Layer (REST API)

| Controller | Base Route | Key Endpoints |
|---|---|---|
| `DepartmentsController` | `/api/departments` | CRUD + activate/deactivate |
| `DoctorController` | `/api/doctors` | Register, shifts, relocate, transfer, lifecycle |
| `MedicineController` | `/api/medicines` | CRUD + stock adjust + dispense + expiry queries |
| `NurseController` | `/api/nurses` | CRUD + shift management + patient list |

---

## 🧪 Tests

**Domain unit tests** via xUnit + FluentAssertions:

- `DoctorTests` — creation, validation
- `DoctorScheduleTests` — add/update/remove schedule logic
- `DoctorAppointmentTests` — booking past dates, unavailable doctors
- `DoctorStateTests` — activate/deactivate state machine
- `DepartmentTests` — department lifecycle rules
- `UserAuthenticationTests` and `UserValueObjectTests` — authentication rules, lockout, profile values, and security state

---

## 🛠️ Tech Stack

| Concern | Technology |
|---|---|
| Framework | ASP.NET Core 8 |
| ORM | Entity Framework Core 9 |
| Database | SQL Server |
| CQRS | MediatR 11 |
| Validation | FluentValidation 12 |
| Mapping | AutoMapper 12 |
| Authentication | ASP.NET Core Identity + JWT Bearer |
| Testing | xUnit + FluentAssertions |
| Architecture | DDD + Clean Architecture + CQRS |

---

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (local or Docker)

### Setup

```bash
# Clone the repo
git clone https://github.com/your-username/Hospital-Management-System.git
cd Hospital-Management-System

# Restore packages
dotnet restore

# Apply migrations
dotnet ef database update --project Infrastructure --startup-project PL

# Run the API
dotnet run --project PL
```

The API will be available at `https://localhost:7281` with Swagger UI at `/swagger`.

### Authentication

- `POST /api/auth/register` creates a patient account and returns a JWT.
- `POST /api/auth/login` accepts a username or email and returns a JWT.
- All other API endpoints require a valid bearer token.
- Development uses a development-only signing key from `appsettings.Development.json`. Set `JWT__SigningKey` to a private value of at least 32 UTF-8 bytes in every non-development environment.

### Connection String

Update `PL/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=HMS;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

---

## 📁 Project Structure

```
Hospital-Management-System/
├── Domain/                    # Entities, Aggregates, Interfaces, SharedKernel
│   ├── Doctors/
│   ├── Departments/
│   ├── Medicines/
│   ├── Nurses/
│   ├── Schedules/
│   ├── Entities/              # Shared entities (Appointment, Patient, etc.)
│   ├── SharedKernel/          # Base classes and abstractions
│   └── Users/                 # Identity-related domain
│
├── Application/               # CQRS handlers, validators, DTOs
│   ├── Features/
│   │   ├── Departments/
│   │   ├── Doctors/
│   │   ├── Medicines/
│   │   └── Nurses/
│   └── Common/                # Pipeline behaviors, exceptions
│
├── Infrastructure/            # EF Core, Repositories, Migrations
│   ├── Presistence/
│   │   ├── Database/
│   │   └── Configurations/
│   ├── Repositories/
│   └── Migrations/
│
├── PL/                        # ASP.NET Core Web API (Controllers, Program.cs)
│   └── Controllers/
│
└── Domain.Tests/              # Unit tests for domain logic
```

---

## 🗺️ Roadmap

- [ ] JWT Authentication & Role-Based Authorization
- [ ] Patient management endpoints
- [ ] Billing & Invoice module
- [ ] Notification service (email/SMS)
- [ ] Audit logging
- [ ] Docker support
- [ ] Integration tests

---

## 📄 License

MIT
