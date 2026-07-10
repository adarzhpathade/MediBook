# api_contracts.md

# MediBook API Contracts

This document defines the internal controller endpoints, request models, response models, validation rules, and error handling for MediBook.

Although MediBook is built as an **ASP.NET Core MVC application**, documenting controller endpoints provides a clear contract between the UI, Controllers, Services, and Repositories.

---

# API Standards

## Request Flow

```text
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

Razor View / Redirect
```

---

## Standard Response Pattern

MVC controllers will return one of the following:

* View()
* RedirectToAction()
* Json()
* BadRequest()
* NotFound()
* Unauthorized()

---

## Validation Rules

Every POST request must

* Validate ModelState
* Validate Authentication
* Validate Authorization
* Validate Business Rules

No database operation should execute when validation fails.

---

# Authentication Endpoints

---

## Register User

### Route

```text
POST /Account/Register
```

### Authentication

Public

### Purpose

Register a new patient account.

### Request Model

```json
{
  "fullName": "John Doe",
  "email": "john@example.com",
  "password": "Password@123",
  "confirmPassword": "Password@123"
}
```

### Success

* User created
* Patient profile created
* Redirect to Login

### Validation

* Required fields
* Valid email
* Unique email
* Password confirmation
* Password policy

### Error Responses

```text
Email already exists.
```

```text
Passwords do not match.
```

---

## Login

### Route

```text
POST /Account/Login
```

### Authentication

Public

### Request

```json
{
  "email":"john@example.com",
  "password":"Password@123"
}
```

### Success

Creates authenticated session.

Redirects according to role.

Patient

↓

Patient Dashboard

Doctor

↓

Doctor Dashboard

Admin

↓

Admin Dashboard

### Validation

* Email exists
* Password correct
* User active

### Error

```text
Invalid email or password.
```

---

## Logout

### Route

```text
GET /Account/Logout
```

### Authentication

Authenticated User

### Action

* Destroy session
* Redirect Home

---

# Patient Module

---

## Patient Dashboard

### Route

```text
GET /Patient/Dashboard
```

### Authentication

Patient

### Response

Dashboard ViewModel

Contains

* Statistics
* Upcoming Appointment
* Recent Activity

---

## Find Doctors

### Route

```text
GET /Patient/FindDoctors
```

### Authentication

Patient

### Query Parameters

```text
search

specialization

experience

fee

page
```

### Response

List of Doctor Cards

---

## Doctor Profile

### Route

```text
GET /Patient/DoctorProfile/{doctorId}
```

### Authentication

Patient

### Response

Doctor Profile

* Information
* Availability
* Consultation Fee

### Error

404

Doctor not found.

---

## Book Appointment

### Route

```text
POST /Appointment/Book
```

### Authentication

Patient

### Request

```json
{
  "doctorId":5,
  "appointmentDate":"2026-08-15",
  "appointmentTime":"10:30",
  "reason":"Routine Checkup"
}
```

### Business Rules

* Date cannot be past.
* Doctor must exist.
* Time available.
* No duplicate booking.

### Success

Appointment created.

Status

Pending

### Error

```text
Selected time slot is unavailable.
```

---

## My Appointments

### Route

```text
GET /Patient/MyAppointments
```

### Authentication

Patient

### Response

Appointment List

Supports

* Search
* Status Filter
* Pagination

---

## Appointment Details

### Route

```text
GET /Patient/Appointment/{appointmentId}
```

### Authentication

Patient

### Response

Complete appointment information.

---

## Cancel Appointment

### Route

```text
POST /Appointment/Cancel
```

### Authentication

Patient

### Request

```json
{
   "appointmentId":15
}
```

### Business Rules

Can cancel only

* Pending
* Confirmed

Appointment must not be completed.

### Response

Appointment status updated.

---

## Update Patient Profile

### Route

```text
POST /Patient/Profile
```

### Authentication

Patient

### Request

Patient Profile ViewModel

### Response

Profile updated.

---

## Change Patient Password

### Route

```text
POST /Patient/ChangePassword
```

### Authentication

Patient

### Request

```json
{
    "currentPassword":"",
    "newPassword":"",
    "confirmPassword":""
}
```

### Validation

* Current password valid.
* Password confirmation.
* Password policy.

---

# Doctor Module

---

## Doctor Dashboard

### Route

```text
GET /Doctor/Dashboard
```

### Authentication

Doctor

---

## Appointment Requests

### Route

```text
GET /Doctor/AppointmentRequests
```

### Authentication

Doctor

### Response

Pending appointments.

---

## Accept Appointment

### Route

```text
POST /Doctor/AcceptAppointment
```

### Request

```json
{
   "appointmentId":20
}
```

### Result

Status

Confirmed

---

## Decline Appointment

### Route

```text
POST /Doctor/DeclineAppointment
```

### Request

```json
{
   "appointmentId":20
}
```

### Result

Status

Declined

---

## Reschedule Appointment

### Route

```text
POST /Doctor/RescheduleAppointment
```

### Request

```json
{
   "appointmentId":20,
   "newDate":"2026-08-20",
   "newTime":"14:00"
}
```

### Validation

* Future date
* Available slot
* Appointment pending

### Result

Status

Rescheduled

---

## Complete Appointment

### Route

```text
POST /Doctor/CompleteAppointment
```

### Request

```json
{
   "appointmentId":20
}
```

### Result

Status

Completed

---

## Doctor Schedule

### Route

```text
GET /Doctor/Schedule
```

### Authentication

Doctor

### Response

Daily

Weekly

Monthly Schedule

---

## Update Doctor Profile

### Route

```text
POST /Doctor/Profile
```

### Authentication

Doctor

### Updates

* Qualification
* Biography
* Experience
* Consultation Fee
* Availability

---

## Change Doctor Password

### Route

```text
POST /Doctor/ChangePassword
```

### Authentication

Doctor

Same validation as Patient.

---

# Admin Module

---

## Admin Dashboard

### Route

```text
GET /Admin/Dashboard
```

### Authentication

Admin

### Response

Platform statistics.

---

## Manage Doctors

### Route

```text
GET /Admin/Doctors
```

Supports

* Search
* Pagination
* Sorting

---

## Add Doctor

### Route

```text
POST /Admin/AddDoctor
```

### Request

Doctor ViewModel

### Action

Creates

* User
* Doctor

Inside one transaction.

---

## Update Doctor

### Route

```text
POST /Admin/EditDoctor
```

### Authentication

Admin

Updates doctor information.

---

## Delete Doctor

### Route

```text
POST /Admin/DeleteDoctor
```

### Authentication

Admin

Soft delete recommended.

---

## Manage Patients

### Route

```text
GET /Admin/Patients
```

Supports

* Search
* Pagination

---

## Update Patient

### Route

```text
POST /Admin/EditPatient
```

Authentication

Admin

---

## Manage Appointments

### Route

```text
GET /Admin/Appointments
```

Supports

* Search
* Filter
* Sorting
* Pagination

---

## Appointment Details

### Route

```text
GET /Admin/Appointment/{id}
```

Authentication

Admin

---

## Platform Statistics

### Route

```text
GET /Admin/Statistics
```

Returns

* Total Doctors
* Total Patients
* Total Appointments
* Pending
* Completed
* Cancelled

---

## Admin Profile

### Route

```text
POST /Admin/Profile
```

Authentication

Admin

---

## Change Admin Password

### Route

```text
POST /Admin/ChangePassword
```

Authentication

Admin

---

# Common HTTP Status Responses

| Status | Meaning               |
| ------ | --------------------- |
| 200    | Success               |
| 302    | Redirect              |
| 400    | Validation Failed     |
| 401    | Unauthorized          |
| 403    | Forbidden             |
| 404    | Resource Not Found    |
| 500    | Internal Server Error |

---

# Security Rules

Every POST request must include:

* Anti-Forgery Token
* Authentication Check
* Authorization Check
* Model Validation
* Business Rule Validation

Never trust client-side validation alone.

---

# Error Handling Standards

Controllers should never expose raw exceptions.

Instead, return:

* Friendly validation messages
* Custom error pages
* Logging for unexpected exceptions

---

# Future API Expansion

If MediBook evolves into a Web API or mobile backend, REST endpoints can be introduced under:

```text
/api/v1/
```

Examples:

```text
/api/v1/auth/login
/api/v1/doctors
/api/v1/patients
/api/v1/appointments
/api/v1/admin
```

The business logic should remain unchanged because Controllers already delegate to Services.

---

# AI Coding Agent Notes

When implementing controller actions:

* Validate `ModelState` before processing.
* Authorize the current user's role before executing business logic.
* Keep Controllers thin.
* Delegate all business logic to Services.
* Use Repositories exclusively for database access.
* Wrap multi-table operations in transactions.
* Return appropriate views or redirects with user-friendly messages.
* After implementing each endpoint:

  * Build the solution.
  * Test the feature.
  * Update `progress_tracker.md`.
  * Commit to Git.
  * Push to GitHub.

This document defines the controller contracts and interaction patterns for the MediBook application and should remain synchronized with the implementation.
