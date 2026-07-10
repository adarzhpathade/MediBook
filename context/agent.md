# agent.md

# MediBook AI Development Agent Guide

> **Version:** 1.0
> **Project:** MediBook – Doctor Appointment Scheduler
> **Framework:** ASP.NET Core MVC (.NET 8)

---

# Mission

Your mission is to build MediBook into a secure, scalable, maintainable, and production-ready Doctor Appointment Scheduling platform while strictly following the project documentation.

This project follows a **Documentation-First Development** methodology.

Every implementation decision must align with the project documents.

---

# Project Goal

Develop a modern healthcare appointment scheduling platform where:

* Patients can discover doctors and book appointments.
* Doctors can manage appointment requests and schedules.
* Administrators can manage the entire platform.

The application should demonstrate professional software engineering practices and clean ASP.NET Core MVC architecture.

---

# Technology Stack

## Backend

* ASP.NET Core MVC (.NET 8)
* C#

## Frontend

* Razor Views
* Bootstrap 5
* JavaScript

## Database

* Neon PostgreSQL

## Database Access

* ADO.NET
* Npgsql

## Authentication

* Custom Authentication
* Role-Based Authorization

---

# Required Reading Order

Before writing **any code**, read the following files in this exact order:

1. `project_overview.md`
2. `architecture.md`
3. `build_plan.md`
4. `code_standards.md`
5. `library_docs.md`
6. `database_schema.md`
7. `api_contracts.md`
8. `progress_tracker.md`

When UI development begins, also read:

9. `ui_tokens.md`
10. `ui_rules.md`
11. `ui_registry.md`

No implementation should begin without understanding these documents.

---

# Architecture Summary

The application follows a layered MVC architecture.

```text
Browser
      │
      ▼
Controllers
      │
      ▼
Services
      │
      ▼
Repositories
      │
      ▼
ADO.NET + Npgsql
      │
      ▼
Neon PostgreSQL
```

### Layer Responsibilities

#### Controllers

Responsible for:

* Receiving requests
* Model validation
* Calling Services
* Returning Views

Controllers must never contain:

* SQL
* Business logic
* Password hashing

---

#### Services

Responsible for:

* Business rules
* Appointment workflow
* Authentication
* Dashboard calculations
* Validation

---

#### Repositories

Responsible for:

* SQL Queries
* CRUD Operations
* Data Mapping
* Transactions

---

#### Views

Responsible only for presentation.

Views must never contain:

* SQL
* Business logic
* Authentication logic

---

# Folder Structure

The project structure defined in `architecture.md` is mandatory.

Never introduce folders that are not part of the documented architecture unless explicitly approved.

---

# Development Workflow

Every development session follows the same lifecycle.

## Step 1

Read

* Progress Tracker
* Current Active Feature

---

## Step 2

Verify

* Dependencies completed
* Previous phase completed
* Build is successful

---

## Step 3

Implement only the active feature.

Do not work on future features.

Do not partially implement future modules.

---

## Step 4

Build

```bash
dotnet build
```

Fix all compilation errors before continuing.

---

## Step 5

Run and manually test the implemented functionality.

Verify:

* Navigation
* Validation
* Database
* Authentication
* Authorization
* Responsive behavior (if applicable)

---

## Step 6

Update

`progress_tracker.md`

Mark

* Completed feature
* Current percentage
* Current active feature

---

## Step 7

Commit changes.

Example

```bash
git add .

git commit -m "Implement patient dashboard"

git push origin main
```

---

## Step 8

Only after Git push should the next feature begin.

---

# Feature Development Rules

Implement only one feature at a time.

Never combine multiple unrelated features into one implementation.

Each feature must be:

* Functional
* Tested
* Buildable
* Committed
* Pushed

before starting the next feature.

---

# Git Workflow

Git is mandatory.

Every completed stage must end with:

```bash
git add .

git commit -m "<meaningful message>"

git push origin main
```

### Major Milestones

* Foundation
* Authentication
* Public Module
* Patient Module
* Doctor Module
* Admin Module
* Production Readiness

At project completion:

```bash
git tag -a v1.0.0 -m "MediBook Version 1.0.0"

git push origin main --tags
```

Never leave uncommitted work before moving to a new phase.

---

# Coding Standards Summary

Always

* Follow MVC
* Use Dependency Injection
* Use strongly typed ViewModels
* Keep Controllers thin
* Use parameterized SQL
* Hash passwords
* Validate ModelState
* Use transactions for multi-table operations
* Reuse components

Never

* Write SQL in Controllers
* Write SQL in Razor Views
* Store passwords in plain text
* Duplicate business logic
* Ignore validation
* Ignore authorization
* Hardcode configuration values

---

# Database Rules

Database access must always go through Repositories.

Use:

* ADO.NET
* Npgsql
* Parameterized queries
* Transactions where necessary

Never concatenate SQL strings.

---

# Security Rules

Always implement:

* Password Hashing (BCrypt)
* Anti-Forgery Tokens
* Role Authorization
* Session Validation
* Input Validation
* SQL Injection Prevention
* XSS Prevention
* CSRF Protection

Never expose raw exception messages.

---

# UI Rules

Until design assets are provided:

* Use Bootstrap 5 components.
* Keep layouts clean and responsive.
* Avoid unnecessary custom CSS.

Once the UI documentation is available:

* Follow `ui_tokens.md`
* Follow `ui_rules.md`
* Reuse components from `ui_registry.md`

No custom UI patterns should be introduced outside the design system.

---

# Documentation Synchronization

Whenever implementation changes affect the architecture or workflow:

Update the corresponding documentation.

Examples:

* Database changes → `database_schema.md`
* API changes → `api_contracts.md`
* Progress → `progress_tracker.md`

Documentation and implementation must remain synchronized.

---

# Error Handling Policy

All exceptions should be handled gracefully.

Use:

* Friendly validation messages
* Custom error pages
* Centralized exception middleware
* Logging

Never expose stack traces to end users.

---

# Testing Checklist

Before marking a feature complete:

* Project builds successfully.
* Feature works as expected.
* Existing features remain functional.
* Validation tested.
* Authorization tested.
* Responsive layout verified (if applicable).

---

# Definition of Done

A feature is complete only when:

* Acceptance criteria satisfied.
* Build succeeds.
* No compilation errors.
* Manual testing completed.
* Documentation updated.
* Progress tracker updated.
* Git commit created.
* Git push completed.

If any item is incomplete, the feature is **not** considered done.

---

# Forbidden Actions

The AI agent must never:

* Skip reading documentation.
* Modify unrelated files.
* Break MVC architecture.
* Add unnecessary packages.
* Hardcode secrets.
* Store passwords in plain text.
* Duplicate business logic.
* Bypass authorization.
* Push broken builds.
* Begin the next feature before the current one is complete.

---

# AI Development Principles

The AI agent should think like a Senior Software Engineer.

Priorities:

1. Correctness
2. Maintainability
3. Readability
4. Security
5. Scalability
6. Performance
7. User Experience

Never sacrifice architecture for short-term convenience.

---

# Project Completion Checklist

The project is complete only when:

* All 74 features are implemented.
* All phases are completed.
* Documentation is synchronized.
* Git history contains milestone commits.
* Version `v1.0.0` is tagged.
* Application builds successfully.
* All manual testing passes.
* Database is verified.
* Authentication and authorization are secure.
* Responsive UI is complete.
* Production configuration is ready.

---

# Final Instruction to All AI Coding Agents

MediBook is a documentation-driven project.

The documentation is the **single source of truth**.

When in doubt:

1. Read the documentation.
2. Follow the documented architecture.
3. Implement only the active feature.
4. Test thoroughly.
5. Update progress.
6. Commit.
7. Push.
8. Then continue.

Never skip steps.

Never assume undocumented behavior.

Build MediBook incrementally, professionally, and maintainably—one verified feature at a time.
