# library_docs.md

# MediBook Library Documentation

This document defines how third-party libraries and frameworks are used throughout the MediBook project.

It is **project-specific**, not generic documentation.

Every AI coding agent and developer should follow these usage patterns to maintain consistency across the codebase.

---

# Technology Stack Summary

| Library / Framework       | Purpose                   |
| ------------------------- | ------------------------- |
| ASP.NET Core MVC (.NET 8) | Application Framework     |
| C#                        | Backend Language          |
| Razor Views               | Server-side UI            |
| Bootstrap 5               | Responsive UI Framework   |
| JavaScript                | Client-side Interactivity |
| Neon PostgreSQL           | Cloud Database            |
| Npgsql                    | PostgreSQL Driver         |
| ADO.NET                   | Database Access           |
| BCrypt.Net                | Password Hashing          |

---

# ASP.NET Core MVC

## Purpose

Provides the MVC framework for the entire application.

---

## Responsibilities

* Routing
* Controllers
* Dependency Injection
* Model Binding
* Validation
* Session Management
* Middleware
* Razor Rendering

---

## Best Practices

Always

* Keep Controllers thin.
* Move business logic to Services.
* Use strongly typed ViewModels.
* Return IActionResult.
* Validate ModelState.

Example

```csharp
public IActionResult Login()
{
    return View();
}
```

---

Never

* Execute SQL inside Controllers.
* Place business logic inside Controllers.
* Access the database directly.

---

# Dependency Injection

Register every Service and Repository inside Program.cs.

Example

```csharp
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
```

Never instantiate dependencies using

```csharp
new UserRepository();
```

Always use constructor injection.

---

# Razor Views

## Purpose

Render server-side HTML.

---

Views should contain

* HTML
* Razor Syntax
* Bootstrap Components

Views should NOT contain

* SQL
* Business Logic
* Password Hashing

---

Use strongly typed models.

Example

```csharp
@model LoginViewModel
```

Avoid

```csharp
ViewBag

ViewData
```

unless absolutely necessary.

---

# Bootstrap 5

## Purpose

Responsive UI framework.

---

Use Bootstrap components whenever possible.

Preferred Components

* Navbar
* Cards
* Buttons
* Alerts
* Badges
* Tables
* Forms
* Grid
* Pagination
* Modal
* Toast

---

Layout

Use Bootstrap Grid.

Example

```html
<div class="container">

<div class="row">

<div class="col-lg-6">

</div>

</div>

</div>
```

---

Avoid

Large amounts of custom CSS.

Customize Bootstrap instead.

---

# JavaScript

## Purpose

Enhance user interaction.

---

Use JavaScript for

* AJAX
* Validation enhancement
* Dynamic UI
* Confirmation Dialogs
* Toggle Elements

Do NOT implement business rules using JavaScript.

Business rules belong in Services.

---

# Neon PostgreSQL

## Purpose

Primary database.

---

Benefits

* Managed PostgreSQL
* Cloud-hosted
* Automatic backups
* High availability
* SQL compliant

---

Connection String

Stored only inside

```text
appsettings.json
```

Never hardcode connection strings.

---

# Npgsql

## Purpose

Official PostgreSQL provider for .NET.

---

Installation

```bash
dotnet add package Npgsql
```

---

Connection Example

```csharp
using var connection =
new NpgsqlConnection(connectionString);

await connection.OpenAsync();
```

Always close connections automatically using

```csharp
using
```

---

# ADO.NET

## Purpose

Database access layer.

---

Workflow

```text
Open Connection

↓

Create Command

↓

Add Parameters

↓

Execute Query

↓

Map Results

↓

Close Connection
```

---

Parameterized Queries

Always

```sql
SELECT *

FROM Users

WHERE Email=@Email
```

Never

```sql
SELECT *

FROM Users

WHERE Email='" + email + "'"
```

---

Transactions

Use transactions when

* Registering users
* Creating appointments
* Updating multiple tables
* Complex operations

Example

```csharp
using var transaction =
connection.BeginTransaction();
```

Commit

↓

Rollback on Error

---

Repository Pattern

Each repository manages one entity.

Example

UserRepository

↓

Users Table

AppointmentRepository

↓

Appointments Table

Never mix multiple unrelated entities in one repository.

---

# BCrypt.Net

## Purpose

Secure password hashing.

---

Installation

```bash
dotnet add package BCrypt.Net-Next
```

---

Hash Password

```csharp
string hash =
BCrypt.Net.BCrypt.HashPassword(password);
```

Verify Password

```csharp
bool valid =
BCrypt.Net.BCrypt.Verify(password, hash);
```

Never

* Encrypt passwords.
* Store plain text passwords.

Always hash passwords.

---

# Session Management

ASP.NET Session stores

* UserId
* Name
* Role
* Email

Do NOT store

* Password
* Password Hash
* Sensitive personal data

---

# Model Validation

Use DataAnnotations.

Example

```csharp
[Required]

[EmailAddress]

[StringLength(100)]
```

Always validate

```csharp
ModelState.IsValid
```

before processing requests.

---

# Exception Handling

Global exception middleware handles unexpected errors.

Repository methods

↓

Throw

↓

Service

↓

Controller

↓

Friendly Error Page

Never expose stack traces.

---

# Logging

Log

* Authentication failures
* Database errors
* Appointment updates
* System exceptions

Future Version

Serilog integration.

---

# File Uploads

Future support

* Doctor profile pictures
* Medical documents

Rules

* Validate file type.
* Validate file size.
* Generate unique filenames.
* Store outside source code directories if persistent storage is introduced.

---

# Configuration

Configuration belongs in

```text
appsettings.json
```

Development

```text
appsettings.Development.json
```

Production

Environment Variables

Never hardcode

* Connection Strings
* Secrets
* API Keys

---

# SQL Script Organization

Organize SQL scripts

```text
Database/

Tables/

Seed/

Views/

Functions/

Procedures/
```

One file per database object.

---

# Common Utility Helpers

Helpers folder

```text
PasswordHasher

SessionHelper

ValidationHelper

DateTimeHelper
```

Each helper has one responsibility.

---

# Reusable Components

Reusable Partial Views

```text
_Navbar

_Footer

_Alert

_ValidationSummary

_Pagination

_Breadcrumb
```

Reuse instead of duplication.

---

# Package Management

NuGet packages should be

* Stable
* Actively maintained
* Official when possible

Avoid unnecessary dependencies.

---

# Version Compatibility

Project versions

| Component        | Version       |
| ---------------- | ------------- |
| .NET             | 8             |
| ASP.NET Core MVC | 8             |
| Bootstrap        | 5.x           |
| PostgreSQL       | 16+           |
| Npgsql           | Latest Stable |
| BCrypt.Net       | Latest Stable |

Always use compatible versions.

---

# Library Usage Rules

Always

* Use Dependency Injection.
* Use strongly typed ViewModels.
* Use parameterized SQL.
* Hash passwords.
* Dispose database resources.
* Validate user input.
* Reuse partial views.
* Keep libraries updated.

Never

* Hardcode secrets.
* Execute SQL in Controllers.
* Place business logic in Views.
* Store passwords in plain text.
* Ignore exception handling.
* Bypass Dependency Injection.
* Add unnecessary packages.

---

# AI Coding Agent Notes

Before implementing any feature:

1. Read `project_overview.md`.
2. Read `architecture.md`.
3. Read `build_plan.md`.
4. Follow `code_standards.md`.
5. Use only the libraries and patterns defined in this document.
6. Reuse existing services, repositories, helpers, and partial views before creating new ones.
7. After completing a feature:

   * Build the project.
   * Test the feature.
   * Update documentation if needed.
   * Commit to Git.
   * Push to GitHub.
   * Then proceed to the next development stage.

This document is the authoritative reference for all library usage throughout the MediBook project.
