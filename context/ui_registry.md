# MediBook UI Registry

Version: 1.0

This document serves as the official component registry for MediBook.

Every UI component must be reusable.

Developers and AI coding agents should always reuse existing components before creating new ones.

---

# Component Status Legend

| Status | Meaning |
|---------|----------|
| ✅ | Implemented |
| 🟡 | Planned |
| 🔄 | Needs Update |

Version 1 starts with all components in **Planned** status.

---

# Folder Structure

```
Views/
│
├── Shared/
│   ├── Components/
│   ├── Layouts/
│   ├── Partials/
│   └── Modals/
│
├── Patient/
├── Doctor/
├── Admin/
├── Home/
└── Account/
```

---

# Layout Components

---

## AppLayout

**Status**

🟡 Planned

**Path**

```
Views/Shared/Layouts/_AppLayout.cshtml
```

**Purpose**

Main authenticated layout.

Used by

- Patient
- Doctor
- Admin

Contains

- Sidebar
- Top Navbar
- Main Content
- Toast Container

Dependencies

- Sidebar
- TopNavbar

---

## PublicLayout

Status

🟡 Planned

Path

```
Views/Shared/Layouts/_PublicLayout.cshtml
```

Used for

- Landing
- About
- Authentication

Contains

- Navbar
- Footer

---

# Navigation Components

---

## Sidebar

Status

🟡 Planned

Path

```
Views/Shared/Components/_Sidebar.cshtml
```

Props

```
CurrentRole

ActivePage
```

Features

- Logo
- Navigation
- Bottom Actions

Shared by

Patient

Doctor

Admin

---

## Top Navbar

Status

🟡 Planned

Path

```
Views/Shared/Components/_TopNavbar.cshtml
```

Contains

- Search
- Notifications
- Help
- User Avatar

Reusable

Yes

---

## Breadcrumb

Status

🟡 Planned

Purpose

Display current page hierarchy.

---

# Dashboard Components

---

## Dashboard Header

Path

```
Views/Shared/Components/_DashboardHeader.cshtml
```

Contains

- Title
- Subtitle
- Action Buttons

Reusable

Across all dashboards.

---

## Statistics Card

Status

🟡 Planned

Props

```
Title

Value

Icon

Color

Subtitle
```

Used in

Patient

Doctor

Admin

---

## Activity Timeline

Status

🟡 Planned

Displays

Recent activities.

---

## Calendar Widget

Status

🟡 Planned

Views

Monthly

Weekly

Daily

Reusable

Doctor

Patient

Admin

---

# Doctor Components

---

## Doctor Card

Status

🟡 Planned

Props

```
Doctor Name

Image

Experience

Specialization

Consultation Fee

Rating
```

Buttons

Book

View Profile

---

## Doctor Profile Card

Displays

Doctor details.

---

## Availability Widget

Displays

Available Days

Available Time Slots

---

# Appointment Components

---

## Appointment Card

Status

🟡 Planned

Props

```
Doctor

Patient

Date

Time

Status

Actions
```

Actions

View

Cancel

Reschedule

---

## Appointment Summary

Displays

Booking summary.

---

## Appointment Timeline

Displays

Booking history.

---

## Status Badge

Props

Status

Styles

Pending

Confirmed

Cancelled

Completed

Declined

Rescheduled

---

# Patient Components

---

## Patient Card

Displays

Patient summary.

---

## Medical Snapshot

Displays

Medical details.

Future expansion ready.

---

## Recommended Doctors

Displays

Horizontal doctor cards.

---

# Profile Components

---

## Profile Header

Displays

Avatar

Name

Statistics

Action Buttons

---

## Profile Statistics

Displays

Visits

Appointments

Saved Doctors

Reports

---

## Personal Information Form

Reusable

Patient

Doctor

Admin

---

## Change Password Form

Shared across

Patient

Doctor

Admin

---

# Authentication Components

---

## Login Form

Fields

Email

Password

Remember Me

Forgot Password

Buttons

Login

Social Login

---

## Register Form

Fields

Name

Email

Password

Confirm Password

---

## Forgot Password Form

Fields

Email

---

# Search Components

---

## Search Bar

Props

Placeholder

Width

Filters

Reusable

Entire application.

---

## Filter Bar

Supports

Search

Dropdowns

Status

Date

Sort

---

# Form Components

---

## Text Input

Standard rounded input.

---

## Email Input

Reusable.

---

## Password Input

Includes

Show Password button.

---

## Phone Input

Reusable.

---

## Date Picker

Used for

Appointments

Profile

Schedule

---

## Time Picker

Used for

Appointment booking.

---

## Select Dropdown

Rounded style.

---

## Textarea

Used for

Biography

Reason for Visit

Notes

---

# Button Components

---

## Primary Button

Black

White Text

---

## Secondary Button

White

Border

---

## Outline Button

Transparent

Black Border

---

## Danger Button

Red

---

## Success Button

Green

---

## Icon Button

Circular

Icon only.

---

# Table Components

---

## Data Table

Reusable

Patient

Doctor

Admin

Supports

Pagination

Sorting

Filtering

Search

---

## Pagination

Shared.

---

## Table Toolbar

Contains

Search

Export

Filters

---

# Modal Components

---

## Confirmation Modal

Reusable

Delete

Cancel

Logout

---

## Appointment Details Modal

Displays

Complete appointment information.

---

## Reschedule Modal

Used by

Doctors

---

## Delete Modal

Reusable

Entire project.

---

# Notification Components

---

## Toast

Types

Success

Warning

Info

Error

---

## Alert

Inline messages.

---

## Empty State

Illustration

↓

Message

↓

Button

---

## Loading Skeleton

Cards

Tables

Forms

Dashboard

---

# Utility Components

---

## Avatar

Sizes

Small

Medium

Large

---

## Divider

Horizontal

Vertical

---

## Chip

Used for

Specialization

Experience

Tags

---

## Badge

Used for

Counters

Notifications

---

## Tooltip

Reusable.

---

## Spinner

Only for

Small loading areas.

Avoid full-page spinners.

---

# Shared Partials

```
_Head.cshtml

_Header.cshtml

_Sidebar.cshtml

_TopNavbar.cshtml

_Footer.cshtml

_Breadcrumb.cshtml

_SearchBar.cshtml

_StatusBadge.cshtml

_Alert.cshtml

_Toast.cshtml

_Pagination.cshtml

_EmptyState.cshtml

_LoadingSkeleton.cshtml

_ValidationSummary.cshtml
```

---

# Naming Standards

Views

PascalCase

```
Dashboard.cshtml

Login.cshtml

Register.cshtml
```

Components

Underscore Prefix

```
_Sidebar.cshtml

_DoctorCard.cshtml
```

CSS

```
dashboard.css

appointments.css
```

JavaScript

```
dashboard.js

appointment.js
```

---

# Reusability Rules

Before creating a component

↓

Search UI Registry

↓

If component exists

↓

Reuse

↓

If component needs modification

↓

Extend it

↓

Never duplicate UI

---

# Component Lifecycle

Design

↓

Register in this file

↓

Develop

↓

Test

↓

Review

↓

Mark as Implemented

---

# AI Coding Agent Rules

Before creating any UI

1. Read `ui_tokens.md`.
2. Read `ui_rules.md`.
3. Check this registry.
4. Reuse an existing component whenever possible.
5. If a new component is required:
   - Add it to this registry.
   - Follow naming standards.
   - Keep it modular.
   - Make it reusable.
6. After implementation:
   - Update the component status from 🟡 Planned to ✅ Implemented.
   - Commit changes.
   - Push to GitHub.

This registry is the authoritative catalog of all reusable UI components in MediBook and must remain synchronized with the application's implementation.

---

# Patient Dashboard Design Language (Imprint)

File: `Views/Patient/Dashboard.cshtml`
Last updated: 2026-07-08

| Property         | Class / Style |
| ---------------- | ------------- |
| Background       | `bg-white` (Cards), `var(--mb-bg)` (Layout) |
| Border           | `border-0` |
| Border radius    | `rounded-4`, `rounded-pill` (Buttons) |
| Text — primary   | `text-dark`, `fw-bold` |
| Text — secondary | `text-muted` |
| Spacing          | `p-4` (Card padding), `g-4`, `gap-3`, `gap-4`, `mb-4`, `mb-5` |
| Hover state      | Cards/Rows: `onmouseover="this.style.backgroundColor='var(--mb-bg)'"` |
| Shadow           | `shadow-sm` |
| Primary Button   | `btn btn-dark rounded-pill fw-bold` |
| Secondary Button | `btn btn-light rounded-pill fw-bold` |
| Bentos           | `bento-card pink`, `blue`, `green`, `yellow` |

**Pattern notes:**
- Use Bootstrap 5 grid (`row`, `col-*`) and utilities (`d-flex`, `justify-content-between`).
- Icons should use `material-symbols-outlined`. For colored background circles behind icons, use `background-color: rgba(191, 212, 255, 0.2); color: var(--mb-accent-blue);` (or respective accent color).
- Do not use Tailwind classes in this project. Use standard Bootstrap 5.
## Baseline � Established 2026-07-08

[Note: This baseline was established via /imprint audit]

| Property         | Correct class |
| ---------------- | ------------- |
| Page Container   | <div class="d-flex flex-column gap-4"> |
| Page Title       | <h1 class="display-6 fw-bold text-dark mb-4"> |
| Card Container   | g-white rounded-4 p-4 shadow-sm border-0 |
| Card Title       | s-5 fw-bold text-dark m-0 |
| Primary Button   | tn btn-dark rounded-pill fw-bold px-4 py-2 |
| Secondary Button | tn btn-outline-dark rounded-pill fw-bold px-4 py-2 |
| Text Primary     | 	ext-dark fw-bold |
| Text Secondary   | 	ext-muted |
| Status Badge     | adge rounded-pill fw-bold with custom rgba bg |
