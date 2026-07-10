# project_overview.md

# MediBook – Doctor Appointment Scheduler

## Project Vision

MediBook is a modern web-based Doctor Appointment Scheduler built using **ASP.NET Core MVC (.NET 8)** that simplifies appointment booking between patients and healthcare providers.

The platform provides dedicated portals for **Patients**, **Doctors**, and **Administrators**, each with clearly defined responsibilities and permissions. The application focuses on providing a clean, secure, responsive, and intuitive experience while following a maintainable MVC architecture suitable for real-world healthcare environments.

The project is intended to demonstrate professional software engineering practices, including:

* ASP.NET Core MVC (.NET 8)
* Razor Views
* Bootstrap 5
* ADO.NET with Npgsql
* Neon PostgreSQL
* Custom Authentication
* Role-Based Authorization
* Clean MVC Architecture
* Responsive UI
* Production-ready project organization

---

# Problem Statement

Many healthcare providers still rely on manual appointment scheduling through phone calls or paper-based systems.

These approaches often lead to:

* Long waiting times
* Double bookings
* Inefficient communication
* Poor appointment tracking
* Difficult doctor discovery
* Administrative overhead

MediBook aims to solve these problems through a centralized online appointment scheduling platform where patients, doctors, and administrators can efficiently manage appointments and healthcare workflows.

---

# Core Vision

Build a secure, scalable, and user-friendly healthcare scheduling platform that enables seamless interaction between patients and doctors while demonstrating modern ASP.NET Core MVC development practices.

---

# Target Audience

## Patients

People looking for healthcare professionals and wishing to book appointments online.

Goals:

* Find doctors
* View doctor profiles
* Book appointments
* Track appointment status
* Manage appointment history

---

## Doctors

Healthcare professionals managing appointments and availability.

Goals:

* Review appointment requests
* Accept appointments
* Decline appointments
* Reschedule appointments
* Manage schedules
* Complete consultations

---

## Administrators

Platform managers responsible for system oversight.

Goals:

* Manage doctors
* Manage patients
* Monitor appointments
* View platform statistics
* Maintain overall platform integrity

---

# User Personas

## Patient Persona

**Name:** Sarah Johnson

Needs:

* Easy doctor discovery
* Quick appointment booking
* Appointment tracking
* Clean interface

Pain Points:

* Long waiting times
* Difficult appointment process
* Poor communication

---

## Doctor Persona

**Name:** Dr. Michael Smith

Needs:

* Organized schedule
* Appointment management
* Flexible availability
* Professional profile

Pain Points:

* Manual scheduling
* Appointment conflicts
* Inefficient workflow

---

## Admin Persona

**Name:** Platform Administrator

Needs:

* Platform management
* User administration
* Appointment monitoring
* System statistics

Pain Points:

* Managing multiple users
* Monitoring platform activity
* Maintaining data consistency

---

# User Journey

## Patient Flow

Landing Page

↓

Register

↓

Login

↓

Dashboard

↓

Find Doctors

↓

Doctor Profile

↓

Book Appointment

↓

Appointment Pending

↓

Doctor Review

↓

Confirmed / Declined / Rescheduled

↓

Consultation

↓

Completed

---

## Doctor Flow

Login

↓

Dashboard

↓

Appointment Requests

↓

Accept / Decline / Reschedule

↓

Manage Schedule

↓

Consultation

↓

Mark Completed

---

## Admin Flow

Login

↓

Dashboard

↓

Manage Doctors

↓

Manage Patients

↓

Manage Appointments

↓

Platform Statistics

---

# User Stories

## Patient

* Register an account.
* Login securely.
* Update profile.
* Search doctors.
* Filter doctors.
* View doctor profiles.
* Book appointments.
* Cancel appointments before consultation.
* Track appointment status.
* View appointment history.

---

## Doctor

* Login securely.
* Update professional profile.
* Configure availability.
* Manage appointment requests.
* Accept appointments.
* Decline appointments.
* Reschedule appointments.
* Complete appointments.
* View appointment history.

---

## Administrator

* Login securely.
* Manage doctors.
* Manage patients.
* View all appointments.
* Search platform records.
* View analytics.
* Manage the system.

---

# Features In Scope

## Public Module

* Landing Page
* Hero Section
* Featured Doctors
* Doctor Search
* Specializations
* About Section
* Testimonials
* Footer
* Login
* Register
* Forgot Password

---

## Authentication

* User Registration
* Login
* Logout
* Password Hashing
* Session Management
* Role-Based Authorization

Roles:

* Patient
* Doctor
* Admin

---

## Patient Module

### Dashboard

* Welcome Card
* Upcoming Appointment
* Appointment Statistics
* Recent Appointments
* Quick Actions

### Find Doctors

* Search
* Filter by Specialization
* Filter by Fee
* Filter by Experience

### Doctor Profile

* Doctor Information
* Qualification
* Experience
* Biography
* Consultation Fee
* Available Schedule

### Book Appointment

* Select Date
* Select Time
* Enter Reason
* Submit Appointment

### My Appointments

* Upcoming
* Completed
* Declined
* Rescheduled
* Cancelled

### Patient Profile

* Personal Information
* Contact Details
* Change Password

---

## Doctor Module

### Dashboard

* Today's Appointments
* Pending Requests
* Upcoming Schedule
* Statistics

### Appointment Requests

* Accept
* Decline
* Reschedule

### Schedule

* Calendar View
* Daily Schedule
* Weekly Schedule

### All Appointments

* Search
* Filter
* History

### Doctor Profile

* Qualifications
* Experience
* Biography
* Consultation Fee
* Available Days
* Available Time Slots

---

## Admin Module

### Dashboard

* Total Doctors
* Total Patients
* Total Appointments
* Pending Requests
* Platform Statistics

### Manage Doctors

* Add
* Edit
* Delete
* Search

### Manage Patients

* View
* Search
* Manage

### Manage Appointments

* View
* Search
* Filter

### Admin Profile

* Personal Information
* Change Password

---

# Features Out of Scope

The first version will NOT include:

* Online Payments
* Video Consultation
* Live Chat
* SMS Notifications
* Email Notifications
* Medical Records
* Prescriptions
* Laboratory Reports
* Doctor Ratings
* Insurance Integration
* Mobile Application
* Multi-Clinic Support
* AI Recommendations

---

# Appointment Workflow

Patient searches doctor

↓

Views doctor profile

↓

Selects date & time

↓

Enters reason

↓

Submits appointment request

↓

Status = Pending

↓

Doctor reviews request

Doctor can:

* Accept
* Decline
* Reschedule

If accepted

↓

Confirmed

If declined

↓

Declined

If rescheduled

↓

Rescheduled

↓

Patient views updated appointment

↓

Consultation

↓

Doctor marks Completed

---

# Appointment Status

* Pending
* Confirmed
* Declined
* Rescheduled
* Completed
* Cancelled

---

# Functional Requirements

## Authentication

* Registration
* Login
* Logout
* Password Hashing
* Role Authorization
* Session Management

---

## Doctor Management

* CRUD Operations
* Professional Information
* Availability
* Consultation Fee
* Biography
* Experience

---

## Patient Management

* Profile
* Appointment Booking
* Appointment History
* Appointment Cancellation

---

## Appointment Management

* Appointment Requests
* Accept
* Decline
* Reschedule
* Complete
* Status Tracking

---

## Administration

* Manage Users
* Search Records
* Dashboard Statistics
* Appointment Monitoring

---

# Non-Functional Requirements

## Performance

* Fast page loading
* Optimized SQL queries
* Responsive UI

---

## Security

* Password hashing
* Secure authentication
* Role-based authorization
* Input validation
* SQL Injection prevention
* XSS protection
* CSRF protection

---

## Scalability

* Layered architecture
* Modular Controllers
* Repository-like separation using services where appropriate
* Reusable components

---

## Reliability

* Exception handling
* Validation
* Database integrity
* Logging

---

## Usability

* Modern UI
* Mobile responsive
* Accessible
* Easy navigation
* Dashboard-first experience

---

# Technology Stack

## Backend

* ASP.NET Core MVC (.NET 8)

## Language

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

# Database Overview

Main Tables

* Users
* Patients
* Doctors
* Appointments

Relationships

* One User → One Patient or Doctor
* One Patient → Many Appointments
* One Doctor → Many Appointments

---

# Success Criteria

The project is considered successful when:

* Patients can register and book appointments.
* Doctors manage appointment requests.
* Administrators manage platform data.
* Authentication is secure.
* Role authorization works correctly.
* Appointment workflow functions without errors.
* Database operations are stable.
* Responsive UI works across devices.
* Code follows clean MVC architecture.

---

# Future Expansion

Possible Version 2 features:

* Online Payments
* Email Notifications
* SMS Notifications
* Calendar Sync
* Doctor Ratings
* AI Doctor Recommendation
* Video Consultation
* Medical Records
* Electronic Prescriptions
* Multi-language Support
* Mobile Apps
* REST API
* Multi-Hospital Support

---

# Assumptions

* One patient can book multiple appointments.
* One doctor can have multiple appointments.
* Doctors control appointment approval.
* Administrators have complete platform access.
* Appointment history remains immutable after completion except by administrators.

---

# Development Workflow

The project will be developed incrementally in clearly defined stages.

Each stage must be fully functional before moving to the next.

## Development Principles

* Build one module at a time.
* Complete backend and frontend for the current stage.
* Test every completed feature.
* Fix issues before continuing.
* Keep the application in a runnable state throughout development.
* Avoid partially implemented modules.
* Follow clean MVC architecture.
* Maintain reusable and readable code.

---

# Version Control Workflow

Git is mandatory throughout the project.

After completing every major stage:

1. Verify the application builds successfully.
2. Test the implemented functionality.
3. Ensure no existing feature is broken.
4. Update project documentation if required.
5. Commit using a meaningful commit message.
6. Push changes to the remote Git repository.

### Expected Milestones

* Project Initialization
* Database Setup
* Authentication Module
* Public Module
* Patient Module
* Doctor Module
* Admin Module
* UI Polish
* Testing & Bug Fixes
* Final Release

### Example Commit Messages

```
Initial ASP.NET Core MVC project setup

Configure PostgreSQL database connection

Implement custom authentication

Complete Patient module

Complete Doctor appointment workflow

Develop Admin dashboard

Improve responsive UI

Fix validation and appointment bugs

Prepare final project release
```

This workflow ensures:

* Reliable backups
* Stable checkpoints
* Easy rollback
* Clean Git history
* Professional development process
* Better collaboration with AI coding agents

---

# Project Development Strategy

This project follows an **AI-assisted, documentation-first development approach**.

The implementation order is:

1. Project Overview
2. Architecture
3. Build Plan
4. Code Standards
5. Library Documentation
6. Database Schema
7. API Contracts
8. UI Design System (after design assets are finalized)
9. Progress Tracker
10. AI Agent Guide

Only after the documentation is finalized will development begin.

Each completed stage will be:

* Fully tested
* Committed to Git
* Pushed to the remote repository
* Recorded in the progress tracker

This ensures a structured, maintainable, and production-ready development lifecycle.
