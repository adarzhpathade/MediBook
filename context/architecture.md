# architecture.md

# MediBook Architecture

## Architecture Goals

The architecture is designed to achieve the following:

* Clean MVC Architecture
* Separation of Concerns
* Scalable project structure
* Easy maintenance
* Reusable components
* Secure authentication
* Efficient database access
* Production-ready organization

---

# Technology Stack

| Technology                | Purpose                  | Reason                                             |
| ------------------------- | ------------------------ | -------------------------------------------------- |
| ASP.NET Core MVC (.NET 8) | Web Framework            | Modern, scalable MVC framework                     |
| C#                        | Backend Language         | Strong typing, performance, maintainability        |
| Razor Views               | UI Rendering             | Server-side rendering with MVC                     |
| Bootstrap 5               | UI Framework             | Responsive and consistent UI                       |
| JavaScript                | Client-side interactions | Lightweight frontend behavior                      |
| Neon PostgreSQL           | Database                 | Cloud-hosted PostgreSQL database                   |
| ADO.NET + Npgsql          | Database Access          | Full SQL control with excellent PostgreSQL support |
| Custom Authentication     | Authentication           | Lightweight role-based authentication              |
| Role-Based Authorization  | Security                 | Restrict access by user role                       |

---

# High-Level Architecture

```
Browser
      │
      ▼
ASP.NET Core MVC

      │
      ▼

Controllers
      │
      ▼

Services (Business Logic)
      │
      ▼

Repositories (ADO.NET)
      │
      ▼

Neon PostgreSQL
```

---

# Project Folder Structure

```
MediBook/

│
├── Controllers/
│   ├── HomeController.cs
│   ├── AccountController.cs
│   ├── PatientController.cs
│   ├── DoctorController.cs
│   ├── AdminController.cs
│   └── AppointmentController.cs
│
├── Models/
│   ├── Entities/
│   │   ├── User.cs
│   │   ├── Patient.cs
│   │   ├── Doctor.cs
│   │   └── Appointment.cs
│   │
│   ├── ViewModels/
│   │   ├── LoginViewModel.cs
│   │   ├── RegisterViewModel.cs
│   │   ├── DoctorProfileViewModel.cs
│   │   ├── AppointmentViewModel.cs
│   │   └── DashboardViewModel.cs
│   │
│   └── DTOs/
│
├── Services/
│   ├── Interfaces/
│   ├── AccountService.cs
│   ├── DoctorService.cs
│   ├── PatientService.cs
│   ├── AppointmentService.cs
│   └── DashboardService.cs
│
├── Repositories/
│   ├── Interfaces/
│   ├── UserRepository.cs
│   ├── DoctorRepository.cs
│   ├── PatientRepository.cs
│   └── AppointmentRepository.cs
│
├── Data/
│   ├── DbConnectionFactory.cs
│   └── DbInitializer.cs
│
├── Helpers/
│   ├── PasswordHasher.cs
│   ├── SessionHelper.cs
│   ├── ValidationHelper.cs
│   └── DateTimeHelper.cs
│
├── Middleware/
│   ├── AuthenticationMiddleware.cs
│   └── ExceptionMiddleware.cs
│
├── Views/
│   ├── Home/
│   ├── Account/
│   ├── Patient/
│   ├── Doctor/
│   ├── Admin/
│   ├── Appointment/
│   └── Shared/
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   ├── images/
│   ├── icons/
│   └── uploads/
│
├── appsettings.json
├── Program.cs
└── MediBook.csproj
```

---

# Layer Responsibilities

## Controllers

Responsibilities

* Receive HTTP Requests
* Validate ModelState
* Call Services
* Return Views
* Handle Redirects

Controllers must **never** contain business logic.

---

## Services

Services contain all business logic.

Responsibilities

* Appointment workflow
* Authentication
* Dashboard calculations
* Doctor search
* Validation
* Scheduling rules

Services communicate with repositories.

---

## Repositories

Repositories communicate with PostgreSQL using ADO.NET.

Responsibilities

* SQL Queries
* CRUD Operations
* Data Mapping
* Database Transactions

Repositories must never contain business logic.

---

## Models

Models represent application data.

Categories

* Entity Models
* View Models
* DTOs

---

# MVC Request Flow

```
Browser

↓

Controller

↓

Service

↓

Repository

↓

PostgreSQL

↓

Repository

↓

Service

↓

Controller

↓

View

↓

Browser
```

---

# Authentication Flow

```
Login Form

↓

AccountController

↓

AccountService

↓

UserRepository

↓

Users Table

↓

Verify Password

↓

Create Session

↓

Redirect by Role

Patient → Patient Dashboard

Doctor → Doctor Dashboard

Admin → Admin Dashboard
```

---

# Authorization

Three Roles

```
Patient

Doctor

Admin
```

Access Rules

Patient

* Patient Module
* Public Pages

Doctor

* Doctor Module

Admin

* Admin Module

Public

* Landing
* Login
* Register
* Forgot Password

Unauthorized users are redirected to Login or Access Denied.

---

# Database Architecture

Main Tables

```
Users

Patients

Doctors

Appointments
```

Relationships

```
Users
│
├── Patients
│
└── Doctors

Patients
│
└── Appointments

Doctors
│
└── Appointments
```

---

# Appointment Lifecycle

```
Patient Books

↓

Pending

↓

Doctor Review

↓

Confirmed

OR

Declined

OR

Rescheduled

↓

Completed

OR

Cancelled
```

Only doctors may change appointment status (except patient cancellation where allowed).

---

# Frontend Architecture

Views are built using

* Razor Views
* Bootstrap 5
* JavaScript

Structure

```
Layout

↓

Navbar

↓

Content

↓

Footer
```

Shared Components

* Navbar
* Footer
* Validation Messages
* Alerts
* Pagination
* Search Bar

---

# Dashboard Architecture

## Patient Dashboard

* Welcome Card
* Upcoming Appointment
* Appointment Statistics
* Recent Activity
* Quick Actions

---

## Doctor Dashboard

* Today's Schedule
* Pending Requests
* Statistics
* Calendar Preview

---

## Admin Dashboard

* Total Doctors
* Total Patients
* Total Appointments
* Pending Requests
* Platform Statistics

---

# Data Access Pattern

Every request follows:

```
Controller

↓

Service

↓

Repository

↓

Npgsql Connection

↓

Execute SQL

↓

Map Data

↓

Return Object
```

Connection management uses a centralized `DbConnectionFactory`.

---

# Error Handling

Global exception middleware handles unexpected errors.

Validation errors remain within controllers.

Repository errors are logged and propagated to services.

User-friendly error pages are displayed instead of raw exceptions.

---

# Security

The application implements:

* Password Hashing
* Secure Sessions
* Role-Based Authorization
* Anti-Forgery Tokens
* Server-side Validation
* SQL Parameterization
* XSS Protection
* CSRF Protection
* Secure Cookies

Passwords are never stored in plain text.

---

# Performance

* Optimized SQL queries
* Parameterized queries
* Minimal page reloads
* Bootstrap responsive layout
* Efficient indexing
* Lazy loading where appropriate

---

# Deployment

Target Environment

* ASP.NET Core (.NET 8)
* IIS / Azure App Service
* Neon PostgreSQL
* GitHub Repository

Configuration is managed through `appsettings.json` and environment variables.

---

# Monitoring

Application logs include:

* Authentication events
* Appointment actions
* Errors
* Database failures
* Unauthorized access attempts

Future integration may include Serilog or Application Insights.

---

# Caching Strategy

Version 1 intentionally avoids complex caching.

Future versions may cache:

* Doctor specializations
* Doctor listings
* Dashboard statistics

---

# Architectural Constraints

The following rules must never be violated:

* Controllers must not contain business logic.
* Views must not access the database.
* Repositories must only perform data access.
* Services contain all business rules.
* SQL queries must use parameterized commands.
* Authentication must be centralized.
* Role checks must never be duplicated unnecessarily.
* Shared UI components should be reused.

---

# Non-Violation Rules

The AI agent and developers must always follow these principles:

* Never place SQL inside Razor Views.
* Never perform database operations inside Controllers.
* Never expose passwords.
* Never bypass authorization checks.
* Never duplicate business logic.
* Never hardcode connection strings.
* Never store sensitive configuration in source code.
* Never skip validation.
* Never break the MVC pattern.
* Keep the application buildable after every completed stage.

---

# Development Workflow

Development follows incremental milestones.

Each stage must satisfy the following before proceeding:

* Feature implementation complete
* Build succeeds
* No compilation errors
* Manual testing completed
* Documentation updated (if required)
* Progress tracker updated
* Commit to Git
* Push to remote repository

No new stage begins until the previous stage has been successfully committed and pushed.

---

# Future Scalability

The architecture supports future expansion with minimal restructuring:

* Email notifications
* SMS integration
* Payment gateway
* REST API
* Mobile application
* JWT Authentication
* Identity integration
* SignalR notifications
* Medical records
* Doctor reviews
* Multi-clinic support
* Cloud storage integration

The architecture is intentionally modular to allow these features to be added without major changes to the existing codebase.
