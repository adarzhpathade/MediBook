# MediBook AI Agent Instructions

Welcome to the MediBook project.

This project follows a Documentation-First Development workflow.

The documentation inside `/docs` (or your documentation folder) is the single source of truth.

---

# Project

Name

MediBook – Doctor Appointment Scheduler

Framework

ASP.NET Core MVC (.NET 8)

Language

C#

Database

Neon PostgreSQL

Database Access

ADO.NET + Npgsql

Frontend

- Razor Views
- Bootstrap 5
- JavaScript

Authentication

Custom Authentication

Role-Based Authorization

---

# Required Reading Order

Before writing any code, read these files:

1. project_overview.md
2. architecture.md
3. build_plan.md
4. code_standards.md
5. library_docs.md
6. database_schema.md
7. api_contracts.md
8. progress_tracker.md

When implementing UI:

9. ui_tokens.md
10. ui_rules.md
11. ui_registry.md

---

# Development Rules

Work on one feature only.

Never partially implement future features.

Always follow the build plan.

Follow the current active feature listed in

progress_tracker.md

---

# Architecture Rules

Follow MVC strictly.

Browser

↓

Controller

↓

Service

↓

Repository

↓

Database

Never violate this flow.

---

# Controllers

Controllers should only

- Receive Requests
- Validate ModelState
- Call Services
- Return Views

Never

- Execute SQL
- Write business logic
- Hash passwords

---

# Services

Services contain

- Business Logic
- Validation
- Appointment Workflow
- Authentication Logic

---

# Repositories

Repositories only

- Execute SQL
- Map Data
- Perform CRUD

Never include business logic.

---

# Database

Always

- Use ADO.NET
- Use Npgsql
- Use parameterized SQL
- Use transactions for multi-table operations

Never concatenate SQL strings.

---

# Authentication

Passwords

Always hashed using BCrypt.

Never stored in plain text.

Sessions store only

- UserId
- Name
- Email
- Role

---

# UI Rules

Use

- Bootstrap 5
- Manrope
- Premium Editorial Design System

Reuse components from

ui_registry.md

Do not invent new design patterns.

---

# Code Standards

Always

- Use Dependency Injection
- Strongly Typed ViewModels
- XML Documentation
- Meaningful Naming
- Proper Error Handling

Never

- Duplicate logic
- Ignore validation
- Ignore authorization

---

# Git Workflow

Every completed feature

↓

Build

↓

Test

↓

Update progress_tracker.md

↓

Commit

↓

Push

Commit message example

Implement patient dashboard

Never leave uncommitted work.

---

# Before Finishing Any Task

Verify

- Build succeeds
- No warnings
- Manual testing complete
- Progress tracker updated
- Documentation synchronized
- Git commit completed
- Git pushed

Only then proceed to the next feature.

---

# Definition of Done

A feature is complete only if

✓ Acceptance Criteria pass

✓ Build succeeds

✓ Tests pass

✓ Documentation updated

✓ Progress tracker updated

✓ Git committed

✓ Git pushed

---

# Important

The documentation is the source of truth.

If implementation conflicts with documentation,

update the documentation first,

then implement.

Never make undocumented architectural decisions.