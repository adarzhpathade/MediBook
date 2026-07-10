# code_standards.md

# MediBook Coding Standards

This document defines the mandatory coding standards for the MediBook project.

Every developer and AI coding agent must follow these standards throughout the project lifecycle to maintain consistency, readability, scalability, and maintainability.

---

# Engineering Principles

The project follows these core principles:

* Write clean, readable code.
* Prefer simplicity over cleverness.
* Follow the Single Responsibility Principle.
* Avoid duplicated logic.
* Keep methods short and focused.
* Use meaningful naming conventions.
* Build reusable components.
* Follow MVC architecture strictly.
* Every feature should be independently testable.
* Never leave unfinished implementations in the codebase.

---

# General Development Rules

Every feature implementation must:

* Compile successfully.
* Pass manual testing.
* Not break existing functionality.
* Follow project architecture.
* Include proper validation.
* Handle exceptions gracefully.
* Be committed to Git after completion.

---

# Project Structure

The project structure defined in `architecture.md` is mandatory.

Developers must not create random folders or files outside the defined architecture.

---

# Folder Naming

Use PascalCase for folders inside the project.

Examples

```text
Controllers
Models
Repositories
Services
Helpers
Middleware
Views
```

Inside Models

```text
Entities
ViewModels
DTOs
```

---

# File Naming

Use PascalCase.

Correct

```text
AccountController.cs

AppointmentService.cs

DoctorRepository.cs

RegisterViewModel.cs
```

Avoid

```text
accountcontroller.cs

doctor_repo.cs

service1.cs
```

---

# Namespace Standards

Namespaces should match folder structure.

Example

```csharp
MediBook.Controllers

MediBook.Services

MediBook.Repositories

MediBook.Models.Entities
```

---

# C# Standards

Use

* var when type is obvious.
* Explicit type when clarity is improved.

Example

```csharp
var doctor = repository.GetDoctor(id);

User user = repository.GetUser(id);
```

---

# Naming Conventions

## Classes

PascalCase

```text
AppointmentService

PatientRepository

DoctorController
```

---

## Methods

PascalCase

```text
CreateAppointment()

UpdateProfile()

DeleteDoctor()
```

---

## Variables

camelCase

```text
doctor

patientId

appointmentList
```

---

## Constants

PascalCase

```text
MaxAppointments

DefaultPageSize
```

---

## Private Fields

Prefix with underscore.

```csharp
private readonly IUserRepository _userRepository;
```

---

# Controller Standards

Controllers should only:

* Receive HTTP Requests
* Validate input
* Call Services
* Return Views
* Redirect users

Controllers must NEVER

* Execute SQL
* Contain business logic
* Hash passwords
* Build HTML
* Access the database directly

Maximum method length

Approximately 30–40 lines.

---

# Service Standards

Services contain all business logic.

Services may

* Validate business rules
* Calculate dashboard statistics
* Manage appointments
* Handle authentication
* Coordinate repositories

Services must NEVER

* Render Views
* Execute Razor code
* Access Session directly unless required

---

# Repository Standards

Repositories are responsible only for database operations.

Responsibilities

* SQL Queries
* CRUD Operations
* Data Mapping
* Transactions

Repositories must NEVER

* Perform business calculations
* Validate workflows
* Handle UI logic

---

# ADO.NET Standards

Always use

```csharp
using var connection = new NpgsqlConnection(connectionString);
```

Always use parameterized queries.

Correct

```csharp
WHERE Email = @Email
```

Never

```sql
WHERE Email = '" + email + "'
```

Always dispose:

* Connection
* Command
* Reader

---

# SQL Standards

Use uppercase SQL keywords.

Correct

```sql
SELECT *

FROM Doctors

WHERE DoctorId=@DoctorId
```

Avoid

```sql
select *

from doctors
```

One query per repository method.

---

# Entity Standards

Entity classes represent database tables only.

No business logic.

Example

```text
Doctor

Patient

Appointment

User
```

---

# ViewModel Standards

ViewModels exist only for UI.

Never reuse Entity models inside Views when a ViewModel is more appropriate.

Examples

```text
LoginViewModel

RegisterViewModel

AppointmentViewModel
```

---

# DTO Standards

DTOs are used when transferring data between layers or APIs.

DTOs should contain only the required properties.

---

# Razor View Standards

Views should contain

* HTML
* Razor Syntax
* Bootstrap Components

Views must NEVER

* Execute SQL
* Hash Passwords
* Implement business rules

---

# Partial Views

Reusable UI belongs in Partial Views.

Examples

```text
_Navbar

_Footer

_ValidationSummary

_Alert

_Pagination
```

---

# Bootstrap Standards

Use Bootstrap components whenever possible.

Examples

* Cards
* Buttons
* Alerts
* Forms
* Tables
* Modals
* Badges
* Pagination

Avoid unnecessary custom CSS.

---

# JavaScript Standards

JavaScript should be lightweight.

Use JavaScript for

* Form enhancements
* AJAX
* Confirmation dialogs
* Dynamic UI updates

Business logic belongs in C#.

---

# CSS Standards

Keep CSS organized.

Preferred order

* Bootstrap
* Site.css
* Page-specific CSS

Avoid inline styles.

---

# Validation Standards

Every form requires

Client-side validation

AND

Server-side validation

Never rely only on JavaScript validation.

---

# Error Handling

Always wrap repository and service operations in try-catch blocks where appropriate.

Never expose raw exception messages to users.

Show friendly error messages.

---

# Logging Standards

Log

* Authentication failures
* Database exceptions
* Appointment updates
* System errors

Future versions may integrate Serilog.

---

# Security Standards

Mandatory

* Password Hashing
* Parameterized Queries
* CSRF Protection
* XSS Protection
* Secure Cookies
* Session Timeout
* Role Authorization

Passwords are never stored in plain text.

---

# Performance Guidelines

Avoid

* Repeated SQL queries
* Duplicate database calls
* Large ViewModels
* Unnecessary page reloads

Prefer

* Efficient queries
* Pagination
* Filtering at database level

---

# Documentation Standards

Every public class requires XML documentation.

Example

```csharp
/// <summary>
/// Handles appointment management.
/// </summary>
public class AppointmentService
{
}
```

Complex methods should include explanatory comments.

---

# Git Workflow

Every completed feature follows:

Implement

↓

Build

↓

Test

↓

Fix Issues

↓

Commit

↓

Push

Never begin a new feature with uncommitted work.

---

# Commit Message Standards

Use meaningful commit messages.

Examples

```text
Implement patient dashboard

Develop doctor appointment workflow

Complete authentication module

Fix appointment validation

Improve responsive design
```

Avoid

```text
update

changes

fixed

work
```

---

# Pull Request Checklist (Optional)

Before merging:

* Builds successfully
* No warnings
* Manual testing completed
* Documentation updated
* No duplicated code

---

# Code Review Checklist

Review for:

* Readability
* Naming
* Validation
* Security
* Performance
* MVC compliance
* Error handling

---

# Definition of Done

A feature is complete only when:

* Functionality implemented.
* Code reviewed.
* Build succeeds.
* Manual testing completed.
* No runtime errors.
* Documentation updated if necessary.
* Progress tracker updated.
* Git commit created.
* Changes pushed to GitHub.

---

# Non-Negotiable Rules

Developers and AI coding agents must NEVER:

* Write SQL inside Razor Views.
* Access the database directly from Controllers.
* Place business logic inside Views.
* Store passwords in plain text.
* Hardcode connection strings.
* Skip validation.
* Ignore authorization.
* Duplicate business logic.
* Break MVC architecture.
* Leave commented-out production code.
* Push broken builds to Git.

These standards are mandatory for the entire MediBook project and must be followed throughout every development stage.
