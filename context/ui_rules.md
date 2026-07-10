# MediBook UI Rules

Version: 1.0

This document converts the design tokens into implementation rules.

Every page, component, and future feature must follow these rules to maintain a consistent user experience.

---

# Design Principles

The UI should always feel

- Premium
- Modern
- Minimal
- Spacious
- Calm
- Professional
- Medical but not sterile

Avoid

- Heavy borders
- Busy layouts
- Bright saturated colors
- Small crowded components
- Inconsistent spacing

---

# Layout Rules

## Application Layout

Desktop Layout

```
+---------------------------------------------------------+
| Sidebar |                 Top Navbar                    |
|         |-----------------------------------------------|
|         |                                               |
|         |               Page Content                    |
|         |                                               |
|         |                                               |
+---------------------------------------------------------+
```

Sidebar

- Fixed
- Width 280px
- Full height
- Rounded right corners

Main Content

- Margin-left = Sidebar Width
- Padding = 40px
- Max Width = 1600px

---

# Authentication Layout

Login

Register

Forgot Password

Use Split Layout

```
+-------------------+------------------------+
| Illustration      | Form                   |
| Floating Card     | Login/Register Form    |
+-------------------+------------------------+
```

Mobile

↓

Illustration hidden

↓

Centered Form

---

# Public Layout

Landing Page

```
Navbar

↓

Hero

↓

Features

↓

Doctors

↓

Testimonials

↓

Footer
```

Always center content.

Use generous spacing.

---

# Responsive Rules

Desktop

1200px+

Sidebar visible.

Tablet

768–1199px

Collapsible sidebar.

Mobile

Below 768px

Sidebar becomes Drawer.

Cards become single-column.

Tables become responsive.

---

# Navigation Rules

Sidebar

Always contains

- Logo
- Navigation
- Bottom Actions

Active Item

- Pink Background
- White Text

Inactive Item

- Gray Text

Hover

- Light Background
- Smooth transition

Icons

20px

Label

14px Medium

---

# Top Navbar

Contains

- Search Bar
- Notifications
- Help
- User Avatar

Search always expands.

Notification badge only appears when unread count > 0.

---

# Page Header

Every page begins with

Large Title

↓

Subtitle

↓

Action Buttons

Example

```
My Appointments

Track upcoming visits...
```

Never place buttons before titles.

---

# Dashboard Rules

Statistics always appear first.

Order

Statistics

↓

Main Content

↓

Secondary Widgets

↓

Tables

↓

Recent Activity

---

# Statistics Cards

Each card contains

- Icon
- Title
- Value
- Small Description

Card Height

140px

Equal width.

Never mix multiple metrics in one card.

---

# Card Rules

Cards always use

- White background
- 32px radius
- Soft shadow
- 24px padding

Hover

- Slight lift
- Larger shadow

Cards never touch each other.

Minimum spacing

24px

---

# Button Rules

Primary

Black Background

White Text

Rounded Pill

---

Secondary

White

Black Border

Black Text

---

Danger

Light Red

Dark Red Text

---

Success

Green

White Text

---

Button Heights

52px

Minimum Width

140px

---

# Form Rules

Forms use

White Inputs

Rounded Pills

Generous spacing

Fields

16px gap

Sections

32px gap

Form Labels

Above Input

Required Field

Display *

Placeholder

Light Gray

---

# Input Rules

Height

54px

Radius

999px

Padding

20px

Focus

Black outline

Never blue outline.

---

# Select Rules

Use native Bootstrap select.

Custom styling only.

Chevron aligned right.

---

# Checkbox Rules

Rounded

16px

Black when selected.

---

# Radio Rules

Circular

Filled black when selected.

---

# Search Bar

Always

Rounded Pill

Icon left

Placeholder

Search...

Height

54px

---

# Table Rules

Cards wrap tables.

Header

White

Body

Alternating rows optional.

Actions

Always last column.

Pagination

Bottom right.

Responsive

Horizontal scrolling on mobile.

---

# Appointment Card

Contains

Doctor Avatar

↓

Doctor Name

↓

Specialization

↓

Date

↓

Time

↓

Status Badge

↓

Action Buttons

Actions

View Details

Cancel

Reschedule

---

# Doctor Card

Contains

Photo

↓

Doctor Name

↓

Specialization

↓

Experience

↓

Consultation Fee

↓

Rating

↓

Book Button

Cards remain equal height.

---

# Profile Rules

Top Card

Avatar

↓

Information

↓

Statistics

↓

Actions

Sections

Personal Information

Medical Information

Account

Notifications

Security

---

# Calendar Rules

Large calendar

Month navigation

Selected date

Black Circle

Available

White

Unavailable

Gray

Today

Pink Outline

---

# Timeline Rules

Vertical timeline.

Newest event first.

Each event

Dot

↓

Title

↓

Timestamp

↓

Description

---

# Status Badges

Pending

Yellow

Confirmed

Green

Cancelled

Red

Completed

Blue

Rescheduled

Pink

Declined

Gray

Badges always

Rounded

Uppercase

Small font

---

# Modal Rules

Backdrop

Dark

Blur

Card

Centered

Rounded

32px

Actions

Bottom right.

Escape key closes modal.

---

# Toast Notifications

Position

Top Right

Duration

4 Seconds

Success

Green

Warning

Yellow

Error

Red

Info

Blue

---

# Loading States

Use

Skeleton Loaders

Never use spinners for full-page loading unless necessary.

Cards

↓

Skeleton

Tables

↓

Skeleton Rows

---

# Empty States

Always include

Illustration

↓

Title

↓

Description

↓

Primary CTA

Example

"No appointments yet."

↓

Book Appointment

---

# Error States

Friendly language.

Never expose exceptions.

Example

Unable to load appointments.

Please try again.

---

# Confirmation Dialogs

Required for

Delete

Cancel Appointment

Logout

Reset Password

Actions

Cancel

Confirm

Primary action highlighted.

---

# Accessibility Rules

Minimum contrast ratio

WCAG AA

Keyboard navigation required.

Focus indicators always visible.

Images require ALT text.

ARIA labels required where appropriate.

Never rely on color alone.

---

# Animation Rules

Cards

200ms

Buttons

150ms

Sidebar

250ms

Dropdown

150ms

Modal

250ms

Animation should be subtle.

---

# Mobile Rules

Sidebar becomes drawer.

Statistics become vertical.

Cards become full width.

Tables become cards.

Forms become single-column.

Action buttons stack vertically.

Touch targets

Minimum 44px.

---

# Bootstrap Rules

Use Bootstrap Grid.

Avoid overriding Bootstrap components unnecessarily.

Create reusable utility classes.

Never use inline styles.

---

# Component Reuse Rules

If a component already exists

↓

Reuse it.

Never duplicate

Buttons

Cards

Inputs

Tables

Modals

Navigation

---

# UI Consistency Checklist

Before completing any page verify

✓ Typography follows tokens

✓ Colors follow tokens

✓ Spacing consistent

✓ Cards consistent

✓ Buttons consistent

✓ Sidebar consistent

✓ Responsive

✓ Accessible

✓ No custom one-off components

Only after passing this checklist is a UI considered complete.