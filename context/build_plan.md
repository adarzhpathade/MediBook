# build_plan.md

# MediBook Development Roadmap

## Development Philosophy

MediBook will be developed incrementally in well-defined stages.

Each stage must satisfy the following before moving to the next:

* Feature implementation completed
* Project builds successfully
* No runtime errors
* Manual testing completed
* Existing functionality remains intact
* Documentation updated (if required)
* Changes committed to Git
* Changes pushed to the remote repository

Every stage should leave the project in a stable, deployable state.

---

# Phase 1 – Project Foundation

## Goal

Set up the project structure, configure dependencies, establish the MVC architecture, and verify database connectivity before implementing any business features.

---

# Feature F01 – Create ASP.NET Core MVC Project

## Description

Initialize the solution using ASP.NET Core MVC (.NET 8).

### Tasks

* Create ASP.NET Core MVC project
* Configure project name
* Configure solution
* Configure HTTPS
* Configure launch settings

### Dependencies

None

### Files Impacted

* Program.cs
* appsettings.json
* MediBook.csproj
* launchSettings.json

### Acceptance Criteria

* Project runs successfully.
* Default MVC routing works.
* HTTPS enabled.

### Verification

* Run project.
* Home page loads.
* No build errors.

---

# Feature F02 – Configure Project Folder Structure

## Description

Organize folders according to the architecture document.

### Tasks

Create folders:

* Controllers
* Models
* Services
* Repositories
* Data
* Helpers
* Middleware
* Views
* wwwroot

Create subfolders:

Models

* Entities
* ViewModels
* DTOs

Services

* Interfaces

Repositories

* Interfaces

wwwroot

* css
* js
* images
* icons

### Dependencies

F01

### Acceptance Criteria

Folder structure matches architecture.

### Verification

Project compiles successfully.

---

# Feature F03 – Configure Bootstrap

## Description

Install and configure Bootstrap 5.

### Tasks

* Configure Layout
* Navbar placeholder
* Footer placeholder
* Bootstrap CSS
* Bootstrap JS

### Dependencies

F02

### Files Impacted

* _Layout.cshtml
* Site.css
* site.js

### Acceptance Criteria

Bootstrap loads correctly.

### Verification

Responsive layout visible.

---

# Feature F04 – Configure Neon PostgreSQL

## Description

Configure database connection.

### Tasks

* Install Npgsql
* Configure connection string
* Verify connectivity

### Dependencies

F03

### Files Impacted

* appsettings.json
* Program.cs

### Acceptance Criteria

Connection opens successfully.

### Verification

Execute test query.

---

# Feature F05 – Database Connection Factory

## Description

Create reusable database connection class.

### Tasks

Create

Data/

* DbConnectionFactory.cs

Responsibilities

* Open connection
* Return NpgsqlConnection
* Handle configuration

### Dependencies

F04

### Acceptance Criteria

Application can obtain reusable database connections.

### Verification

Connection opens and closes successfully.

---

# Feature F06 – Shared Layout

## Description

Create reusable application layout.

### Tasks

Create:

* Header
* Navigation
* Footer
* Scripts section

### Dependencies

F03

### Acceptance Criteria

Every page uses shared layout.

### Verification

Navigate between pages.

---

# Feature F07 – Global Exception Handling

## Description

Implement centralized exception middleware.

### Tasks

* Exception middleware
* Friendly error page
* Logging

### Dependencies

F06

### Acceptance Criteria

Unhandled exceptions display custom error page.

### Verification

Force an exception.

---

# Feature F08 – Git Repository Initialization

## Description

Initialize version control.

### Tasks

* Initialize Git
* Configure .gitignore
* Connect GitHub repository

### Dependencies

F07

### Acceptance Criteria

Repository connected successfully.

### Verification

Push first commit.

---

# Phase 1 Completion Checklist

* ASP.NET Core project created
* Folder structure complete
* Bootstrap configured
* PostgreSQL connected
* Database connection factory created
* Shared layout completed
* Exception handling implemented
* Git repository initialized

---

# Manual Testing

* Application builds
* Application launches
* Navigation works
* Bootstrap loads
* Database connection successful
* No console errors

---

# Git Milestone

## Commit

```text
Initial project setup with ASP.NET Core MVC, Bootstrap, Neon PostgreSQL, and project architecture
```

## Push

```bash
git add .

git commit -m "Initial project setup with ASP.NET Core MVC, Bootstrap, Neon PostgreSQL, and project architecture"

git push origin main
```

---

# Stage Status

✅ Phase 1 Complete

Ready for Phase 2 – Authentication & Authorization.


# Phase 2 – Authentication & Authorization

## Goal

Implement a secure custom authentication system with role-based authorization for Patients, Doctors, and Administrators. Users should only access modules permitted for their role.

---

# Feature F09 – Create Authentication Database Schema

## Description

Design the database tables required for authentication.

### Tasks

Create the following table:

### Users

| Column       | Type           |
| ------------ | -------------- |
| UserId       | UUID / SERIAL  |
| FullName     | VARCHAR        |
| Email        | VARCHAR UNIQUE |
| PasswordHash | TEXT           |
| Role         | VARCHAR        |
| IsActive     | BOOLEAN        |
| CreatedAt    | TIMESTAMP      |

### Dependencies

* Phase 1

### Files Impacted

* SQL Scripts
* DbInitializer.cs

### Acceptance Criteria

* Users table created successfully.
* Email uniqueness enforced.

### Verification

* Insert sample user.
* Verify unique email constraint.

---

# Feature F10 – User Registration

## Description

Allow patients to create a new account.

### Tasks

* Registration View
* Registration ViewModel
* Validation
* Password Hashing
* Save User
* Create Patient Profile automatically
* Redirect to Login

### Dependencies

F09

### Files Impacted

* AccountController.cs
* Register.cshtml
* RegisterViewModel.cs
* AccountService.cs
* UserRepository.cs
* PatientRepository.cs

### Acceptance Criteria

* User registers successfully.
* Password stored as hash.
* Patient record created.

### Verification

* Register new account.
* Verify database records.

---

# Feature F11 – User Login

## Description

Authenticate users using email and password.

### Tasks

* Login Page
* Validate credentials
* Verify hashed password
* Create session
* Redirect according to role

Role Routing

Patient → Patient Dashboard

Doctor → Doctor Dashboard

Admin → Admin Dashboard

### Dependencies

F10

### Files Impacted

* Login.cshtml
* AccountController.cs
* LoginViewModel.cs
* AccountService.cs

### Acceptance Criteria

* Valid users login.
* Invalid users rejected.
* Session created.

### Verification

* Login using all roles.
* Invalid password test.

---

# Feature F12 – Logout

## Description

Destroy authenticated session.

### Tasks

* Logout Action
* Clear Session
* Redirect Home

### Dependencies

F11

### Files Impacted

* AccountController.cs

### Acceptance Criteria

* Session removed.
* Protected pages inaccessible.

### Verification

* Logout.
* Attempt dashboard access.

---

# Feature F13 – Password Hashing Helper

## Description

Create reusable password hashing utility.

### Tasks

Create:

Helpers/

* PasswordHasher.cs

Responsibilities

* Hash Password
* Verify Password

### Dependencies

F10

### Acceptance Criteria

* Password never stored in plain text.

### Verification

* Compare stored hash.
* Verify login.

---

# Feature F14 – Session Management

## Description

Manage authenticated user sessions.

### Tasks

Store

* UserId
* Name
* Role
* Email

Implement helper methods.

### Dependencies

F11

### Files Impacted

* SessionHelper.cs
* Program.cs

### Acceptance Criteria

* Session available across requests.

### Verification

* Refresh browser.
* Session persists.

---

# Feature F15 – Role-Based Authorization

## Description

Restrict module access by user role.

### Rules

Patient

* Patient Module
* Public Pages

Doctor

* Doctor Module

Admin

* Admin Module

### Dependencies

F14

### Files Impacted

* AuthenticationMiddleware.cs
* Controllers

### Acceptance Criteria

Unauthorized users cannot access protected pages.

### Verification

Attempt cross-role access.

---

# Feature F16 – Access Denied Page

## Description

Create a user-friendly unauthorized page.

### Tasks

* AccessDenied.cshtml
* Redirect unauthorized users

### Dependencies

F15

### Acceptance Criteria

Unauthorized access displays friendly message.

### Verification

Visit restricted URL.

---

# Feature F17 – Forgot Password (UI Only)

## Description

Create the forgot password page for Version 1.

### Tasks

* Forgot Password View
* Email Input
* Validation

Backend reset functionality will be implemented in a future version.

### Dependencies

F11

### Acceptance Criteria

Page accessible.

### Verification

Open page.

---

# Phase 2 Completion Checklist

* Users table completed
* Registration completed
* Login completed
* Logout completed
* Password hashing completed
* Session management completed
* Role authorization completed
* Access denied page completed
* Forgot password UI completed

---

# Manual Testing

### Registration

* Register Patient
* Duplicate Email
* Validation

### Login

* Correct credentials
* Incorrect password
* Non-existing email

### Authorization

Patient cannot access Doctor/Admin pages.

Doctor cannot access Admin pages.

Admin can access Admin pages only.

### Session

* Login
* Refresh browser
* Logout
* Session destroyed

---

# Git Milestone

## Commit

```text
Complete custom authentication and role-based authorization system
```

## Push

```bash
git add .

git commit -m "Complete custom authentication and role-based authorization system"

git push origin main
```

---

# Stage Status

✅ Phase 2 Complete

Ready for Phase 3 – Public Module (Landing Page, Home, Doctor Search Preview, Authentication UI).


# Phase 3 – Public Module

## Goal

Develop a modern, responsive, and professional public-facing website that introduces MediBook, allows visitors to explore doctors, and provides seamless access to authentication.

This phase focuses on building the first impression of the platform while maintaining clean UI, accessibility, and responsive design.

---

# Feature F18 – Landing Page

## Description

Develop the homepage that introduces MediBook and encourages users to register or log in.

### Sections

* Navigation Bar
* Hero Section
* Search Doctors
* Featured Doctors
* Specializations
* About MediBook
* Testimonials
* Call-to-Action
* Footer

### Dependencies

* Phase 2

### Files Impacted

* HomeController.cs
* Views/Home/Index.cshtml
* _Layout.cshtml
* site.css
* site.js

### Acceptance Criteria

* Landing page loads correctly.
* Responsive layout.
* Navigation works.

### Verification

* Desktop
* Tablet
* Mobile

---

# Feature F19 – Navigation Bar

## Description

Create a reusable navigation component.

### Navigation

Home

Doctors

About

Login

Register

### Requirements

* Sticky navigation
* Mobile responsive
* Active page highlighting
* Brand logo

### Dependencies

F18

### Acceptance Criteria

Navigation available on all public pages.

### Verification

Navigate between pages.

---

# Feature F20 – Hero Section

## Description

Create a modern hero section.

### Components

* Headline
* Description
* Primary CTA
* Secondary CTA
* Illustration / Hero Image

Buttons

* Book Appointment
* Find Doctors

### Dependencies

F18

### Acceptance Criteria

Hero communicates project purpose clearly.

### Verification

Buttons navigate correctly.

---

# Feature F21 – Search Doctors Preview

## Description

Allow visitors to search doctors directly from the landing page.

Version 1 will redirect to the complete doctor search page.

### Search Fields

* Doctor Name
* Specialization

### Dependencies

F20

### Acceptance Criteria

Search bar visible.

### Verification

Submit search.

---

# Feature F22 – Featured Doctors

## Description

Display a limited number of featured doctors.

### Card Information

* Profile Image
* Name
* Specialization
* Experience
* Consultation Fee
* View Profile Button

Initially populated using mock data.

Real database integration will occur in the Patient Module.

### Dependencies

F18

### Acceptance Criteria

Cards display correctly.

### Verification

Responsive cards.

---

# Feature F23 – Specializations Section

## Description

Display available medical specialties.

Examples

* Cardiologist
* Dentist
* Dermatologist
* Neurologist
* Orthopedic
* Pediatrician
* Psychiatrist
* General Physician

Each specialization displays:

* Icon
* Name
* Doctor Count (optional placeholder)

### Dependencies

F18

### Acceptance Criteria

Grid displays correctly.

### Verification

Responsive layout.

---

# Feature F24 – About MediBook

## Description

Introduce the platform.

### Content

* Mission
* Benefits
* Why choose MediBook
* Platform Highlights

### Dependencies

F18

### Acceptance Criteria

Readable and responsive section.

### Verification

Cross-device testing.

---

# Feature F25 – Testimonials

## Description

Display patient testimonials.

Each testimonial contains

* Profile Image
* Name
* Rating
* Feedback

Version 1 uses static data.

### Dependencies

F18

### Acceptance Criteria

Cards displayed properly.

### Verification

Responsive layout.

---

# Feature F26 – Footer

## Description

Reusable footer for all public pages.

### Content

* Logo
* Quick Links
* Contact
* Copyright
* Social Icons

### Dependencies

F18

### Acceptance Criteria

Footer displayed across all public pages.

### Verification

Navigate through pages.

---

# Feature F27 – Public Doctor Listing

## Description

Create a page where visitors can browse available doctors before logging in.

### Features

* Search
* Filter by Specialization
* Filter by Experience
* Filter by Consultation Fee

Doctor Card

* Image
* Name
* Specialization
* Qualification
* Experience
* Consultation Fee
* View Profile

Booking requires authentication.

### Dependencies

F22

### Files Impacted

* HomeController.cs
* DoctorController.cs
* Doctors.cshtml

### Acceptance Criteria

Doctor listing page works.

### Verification

Search and filters operate correctly.

---

# Feature F28 – Public Doctor Profile

## Description

Allow visitors to view detailed doctor information.

### Information

* Doctor Image
* Name
* Qualification
* Biography
* Experience
* Consultation Fee
* Available Days
* Available Time Slots

Book Appointment button

If user is not logged in

↓

Redirect to Login

### Dependencies

F27

### Acceptance Criteria

Doctor profile displays correctly.

### Verification

Open multiple doctor profiles.

---

# Feature F29 – Public Page SEO & Accessibility

## Description

Improve discoverability and accessibility.

### Tasks

* Meta Title
* Meta Description
* Semantic HTML
* Proper Heading Structure
* Image Alt Text
* Keyboard Navigation
* ARIA Labels

### Dependencies

F18

### Acceptance Criteria

Accessibility audit passes.

### Verification

Keyboard-only navigation.

---

# Phase 3 Completion Checklist

* Landing page completed
* Navbar completed
* Hero section completed
* Search section completed
* Featured doctors completed
* Specializations completed
* About section completed
* Testimonials completed
* Footer completed
* Public doctor listing completed
* Public doctor profile completed
* Accessibility improvements completed

---

# Manual Testing

### Landing Page

* Hero loads
* Buttons work
* Navigation works

### Doctor Listing

* Search
* Filters
* Cards
* Responsive layout

### Doctor Profile

* Information displayed
* Book Appointment redirects to Login when unauthenticated

### Accessibility

* Keyboard navigation
* Alt text
* Proper heading order
* Responsive design

---

# Git Milestone

## Commit

```text
Complete public module with landing page, doctor listing, and responsive public UI
```

## Push

```bash
git add .

git commit -m "Complete public module with landing page, doctor listing, and responsive public UI"

git push origin main
```

---

# Stage Status

✅ Phase 3 Complete

Ready for Phase 4 – Patient Module, including Patient Dashboard, Find Doctors, Doctor Profile, Book Appointment, My Appointments, and Patient Profile.

# Phase 4 – Patient Module

## Goal

Develop a complete patient portal that allows patients to discover doctors, book appointments, manage their profile, and track appointment history through a modern dashboard.

The Patient Module is the primary workflow of MediBook and should provide a smooth and intuitive user experience.

---

# Feature F30 – Patient Dashboard

## Description

Develop the patient dashboard that provides an overview of appointments and quick access to common actions.

### Components

* Welcome Card
* Upcoming Appointment
* Appointment Statistics
* Recent Appointments
* Quick Actions

### Statistics

* Total Appointments
* Upcoming Appointments
* Completed Appointments
* Cancelled Appointments

### Quick Actions

* Find Doctors
* Book Appointment
* View Appointments
* Edit Profile

### Dependencies

* Phase 3

### Files Impacted

* PatientController.cs
* Dashboard.cshtml
* DashboardViewModel.cs
* DashboardService.cs

### Acceptance Criteria

* Dashboard loads successfully.
* Statistics display correctly.
* Quick actions navigate properly.

### Verification

* Login as Patient.
* Dashboard loads without errors.

---

# Feature F31 – Find Doctors

## Description

Allow patients to browse and search doctors from the database.

### Features

Search

* Doctor Name

Filters

* Specialization
* Consultation Fee
* Experience

Sorting

* Name
* Experience
* Consultation Fee

Doctor Card

* Profile Picture
* Name
* Qualification
* Specialization
* Experience
* Consultation Fee
* Available Days
* View Profile Button

### Dependencies

F30

### Files Impacted

* PatientController.cs
* DoctorRepository.cs
* DoctorService.cs
* FindDoctors.cshtml

### Acceptance Criteria

* Doctors loaded from database.
* Search works.
* Filters work.
* Sorting works.

### Verification

* Search by name.
* Filter by specialization.
* Sort by experience.

---

# Feature F32 – Doctor Profile

## Description

Display complete doctor information.

### Information

* Profile Picture
* Full Name
* Qualification
* Specialization
* Experience
* Biography
* Consultation Fee
* Available Days
* Available Time Slots

Buttons

* Book Appointment
* Back to Doctors

### Dependencies

F31

### Files Impacted

* PatientController.cs
* DoctorProfile.cshtml

### Acceptance Criteria

* Doctor information displayed correctly.

### Verification

* Open multiple doctor profiles.

---

# Feature F33 – Book Appointment

## Description

Allow patients to request appointments.

### Booking Form

Patient selects

* Date
* Time
* Reason for Visit

System automatically assigns

* Patient
* Doctor
* Status = Pending
* CreatedAt

### Validation

* Date cannot be in the past.
* Time must be available.
* Required fields validated.

### Dependencies

F32

### Files Impacted

* AppointmentController.cs
* AppointmentService.cs
* AppointmentRepository.cs
* BookAppointment.cshtml

### Acceptance Criteria

* Appointment saved successfully.
* Status = Pending.

### Verification

* Book appointment.
* Verify database.

---

# Feature F34 – Appointment Validation

## Description

Prevent invalid bookings.

### Validation Rules

* Past dates not allowed.
* Duplicate appointments prevented.
* Doctor availability checked.
* Required fields validated.
* Invalid doctor ID rejected.

### Dependencies

F33

### Acceptance Criteria

Invalid appointments rejected.

### Verification

Attempt invalid bookings.

---

# Feature F35 – My Appointments

## Description

Allow patients to view appointment history.

### Categories

* Pending
* Confirmed
* Rescheduled
* Completed
* Cancelled
* Declined

Each Card Displays

* Doctor
* Specialization
* Date
* Time
* Status
* Consultation Fee

Actions

* View Details
* Cancel Appointment (if permitted)

### Dependencies

F33

### Files Impacted

* PatientController.cs
* MyAppointments.cshtml

### Acceptance Criteria

Appointments grouped correctly.

### Verification

Test every appointment status.

---

# Feature F36 – Cancel Appointment

## Description

Allow patients to cancel appointments before consultation.

### Rules

Patients can cancel only if

* Appointment is Pending
* Appointment is Confirmed
* Appointment date has not passed

Status becomes

Cancelled

### Dependencies

F35

### Acceptance Criteria

Appointment status updated.

### Verification

Cancel appointment.

---

# Feature F37 – Appointment Details

## Description

Display complete appointment information.

### Information

* Doctor
* Patient
* Date
* Time
* Status
* Reason
* Consultation Fee

Timeline

* Created
* Updated
* Status Changes

### Dependencies

F35

### Acceptance Criteria

Details page loads.

### Verification

Open appointment.

---

# Feature F38 – Patient Profile

## Description

Allow patients to manage personal information.

### Personal Information

* Full Name
* Phone
* Gender
* Date of Birth
* Address
* Email (Read Only)

### Account

* Change Password

### Dependencies

F30

### Files Impacted

* PatientController.cs
* Profile.cshtml
* PatientService.cs

### Acceptance Criteria

Profile updates successfully.

### Verification

Edit profile.

---

# Feature F39 – Change Password

## Description

Allow patients to securely update passwords.

### Validation

* Current Password
* New Password
* Confirm Password

Rules

* Minimum password length
* Password confirmation
* Hash new password

### Dependencies

F38

### Acceptance Criteria

Password updated.

### Verification

Login with new password.

---

# Feature F40 – Patient Notifications (UI Placeholder)

## Description

Prepare notification area for future versions.

Version 1 includes

* Notification icon
* Empty notification list
* Placeholder component

Future versions will integrate

* Email
* SMS
* Real-time notifications

### Dependencies

F30

### Acceptance Criteria

Notification placeholder visible.

### Verification

Dashboard renders correctly.

---

# Phase 4 Completion Checklist

* Patient Dashboard
* Find Doctors
* Doctor Profile
* Book Appointment
* Appointment Validation
* My Appointments
* Appointment Details
* Cancel Appointment
* Patient Profile
* Change Password
* Notification Placeholder

---

# Manual Testing

### Dashboard

* Cards display correctly.
* Statistics accurate.

### Find Doctors

* Search
* Filters
* Sorting

### Appointment Booking

* Valid booking
* Invalid booking
* Duplicate booking
* Past date validation

### My Appointments

* Status filtering
* Details page
* Cancel appointment

### Profile

* Update profile
* Change password
* Login using new password

### Responsive Testing

* Desktop
* Tablet
* Mobile

---

# Git Milestone

## Commit

```text
Complete Patient Module with dashboard, doctor discovery, appointment booking, profile management, and appointment tracking
```

## Push

```bash
git add .

git commit -m "Complete Patient Module with dashboard, doctor discovery, appointment booking, profile management, and appointment tracking"

git push origin main
```

---

# Stage Status

✅ Phase 4 Complete

Ready for **Phase 5 – Doctor Module**, including Doctor Dashboard, Appointment Requests, Schedule Management, Appointment History, Profile Management, and Appointment Completion Workflow.

# Phase 5 – Doctor Module

## Goal

Develop a complete doctor portal where doctors can manage appointments, configure availability, maintain their professional profile, and monitor their daily schedule.

The Doctor Module is responsible for controlling the appointment lifecycle after a patient submits a booking request.

---

# Feature F41 – Doctor Dashboard

## Description

Develop the doctor's dashboard that provides an overview of appointments, pending requests, and schedule statistics.

### Components

* Welcome Card
* Today's Appointments
* Pending Appointment Requests
* Upcoming Schedule
* Statistics Cards
* Quick Actions

### Statistics

* Today's Appointments
* Pending Requests
* Completed Appointments
* Total Patients

### Quick Actions

* View Requests
* Manage Schedule
* View All Appointments
* Update Profile

### Dependencies

* Phase 4

### Files Impacted

* DoctorController.cs
* Dashboard.cshtml
* DashboardViewModel.cs
* DashboardService.cs

### Acceptance Criteria

* Dashboard loads successfully.
* Statistics display correctly.
* Quick actions function properly.

### Verification

* Login as Doctor.
* Dashboard loads without errors.

---

# Feature F42 – Appointment Requests

## Description

Display all appointment requests awaiting doctor approval.

### Appointment Card

* Patient Name
* Appointment Date
* Appointment Time
* Reason for Visit
* Status
* Created Date

### Available Actions

* Accept
* Decline
* Reschedule
* View Details

### Dependencies

F41

### Files Impacted

* DoctorController.cs
* AppointmentService.cs
* AppointmentRepository.cs
* AppointmentRequests.cshtml

### Acceptance Criteria

Pending appointments are displayed correctly.

### Verification

Create multiple appointment requests and verify they appear.

---

# Feature F43 – Accept Appointment

## Description

Allow doctors to approve appointment requests.

### Workflow

Pending

↓

Doctor Accepts

↓

Status = Confirmed

### Rules

* Only Pending appointments may be accepted.
* Confirmation date remains unchanged.

### Dependencies

F42

### Acceptance Criteria

Appointment status changes to Confirmed.

### Verification

Accept a pending appointment.

---

# Feature F44 – Decline Appointment

## Description

Allow doctors to reject appointment requests.

### Workflow

Pending

↓

Doctor Declines

↓

Status = Declined

### Rules

* Only Pending appointments can be declined.

### Dependencies

F42

### Acceptance Criteria

Appointment status updates to Declined.

### Verification

Decline an appointment.

---

# Feature F45 – Reschedule Appointment

## Description

Allow doctors to propose a new appointment date and time.

### Workflow

Pending

↓

Doctor selects new Date & Time

↓

Status = Rescheduled

↓

Patient can view updated appointment details

### Validation

* Date cannot be in the past.
* Time must be available.
* Prevent scheduling conflicts.

### Dependencies

F42

### Files Impacted

* AppointmentService.cs
* AppointmentRepository.cs
* RescheduleAppointment.cshtml

### Acceptance Criteria

Appointment updates successfully.

### Verification

Reschedule multiple appointments.

---

# Feature F46 – Schedule Management

## Description

Display the doctor's consultation schedule.

### Views

* Daily
* Weekly
* Monthly

Each appointment displays

* Patient
* Time
* Status

### Dependencies

F41

### Files Impacted

* Schedule.cshtml
* DoctorController.cs

### Acceptance Criteria

Schedule displays correctly.

### Verification

Navigate between schedule views.

---

# Feature F47 – All Appointments

## Description

Allow doctors to browse all appointments.

### Categories

* Upcoming
* Completed
* Cancelled
* Declined
* Rescheduled

### Search

* Patient Name
* Date

### Filters

* Status
* Date
* Patient

### Sorting

* Date
* Patient
* Status

### Dependencies

F46

### Acceptance Criteria

Filtering and searching function correctly.

### Verification

Test each filter.

---

# Feature F48 – Complete Appointment

## Description

Allow doctors to mark consultations as completed.

### Workflow

Confirmed

↓

Consultation Finished

↓

Completed

### Rules

* Only Confirmed appointments may be completed.
* Completed appointments become read-only.

### Dependencies

F47

### Acceptance Criteria

Status updates to Completed.

### Verification

Complete multiple appointments.

---

# Feature F49 – Doctor Profile Management

## Description

Allow doctors to manage their professional information.

### Editable Information

* Profile Photo
* Qualification
* Specialization
* Experience
* Biography
* Consultation Fee
* Clinic Address
* Contact Number

### Dependencies

F41

### Files Impacted

* DoctorProfile.cshtml
* DoctorService.cs

### Acceptance Criteria

Doctor profile updates successfully.

### Verification

Update information and reload page.

---

# Feature F50 – Availability Management

## Description

Allow doctors to define consultation availability.

### Configuration

Available Days

* Monday
* Tuesday
* Wednesday
* Thursday
* Friday
* Saturday
* Sunday

Available Time Slots

* Start Time
* End Time
* Consultation Duration

### Rules

* Prevent overlapping slots.
* Validate start/end times.
* Support multiple time slots per day.

### Dependencies

F49

### Acceptance Criteria

Availability saved successfully.

### Verification

Create and update schedules.

---

# Feature F51 – Change Password

## Description

Allow doctors to securely update account passwords.

### Validation

* Current Password
* New Password
* Confirm Password

### Rules

* Password hashing
* Confirmation validation
* Minimum password length

### Dependencies

F49

### Acceptance Criteria

Password updates successfully.

### Verification

Login using new password.

---

# Phase 5 Completion Checklist

* Doctor Dashboard
* Appointment Requests
* Accept Appointment
* Decline Appointment
* Reschedule Appointment
* Schedule Management
* All Appointments
* Complete Appointment
* Doctor Profile
* Availability Management
* Change Password

---

# Manual Testing

### Dashboard

* Statistics
* Quick Actions
* Upcoming Schedule

### Appointment Workflow

* Accept
* Decline
* Reschedule
* Complete

### Schedule

* Daily View
* Weekly View
* Monthly View

### Profile

* Update profile
* Update availability
* Change password

### Responsive Testing

* Desktop
* Tablet
* Mobile

---

# Git Milestone

## Commit

```text
Complete Doctor Module with appointment management, scheduling, profile management, and consultation workflow
```

## Push

```bash
git add .

git commit -m "Complete Doctor Module with appointment management, scheduling, profile management, and consultation workflow"

git push origin main
```

---

# Stage Status

✅ Phase 5 Complete

Ready for **Phase 6 – Admin Module**, where we'll build the administrative dashboard, doctor management, patient management, appointment management, search, filtering, analytics, and overall platform administration.

# Phase 6 – Admin Module

## Goal

Develop a complete administration portal that allows administrators to manage the entire MediBook platform, including doctors, patients, appointments, and platform statistics.

The Admin Module has the highest privilege level and provides complete oversight of the system while maintaining data integrity and security.

---

# Feature F52 – Admin Dashboard

## Description

Create the administrative dashboard that provides an overview of platform activities and statistics.

### Dashboard Cards

* Total Doctors
* Total Patients
* Total Appointments
* Pending Appointments
* Completed Appointments
* Today's Appointments

### Dashboard Widgets

* Recent Registrations
* Recent Appointments
* Appointment Status Chart
* Doctor Distribution
* Quick Actions

### Quick Actions

* Manage Doctors
* Manage Patients
* Manage Appointments
* View Reports

### Dependencies

* Phase 5

### Files Impacted

* AdminController.cs
* Dashboard.cshtml
* DashboardService.cs
* DashboardViewModel.cs

### Acceptance Criteria

* Dashboard statistics display correctly.
* Widgets load successfully.
* Responsive layout.

### Verification

Login as Admin and verify all dashboard cards.

---

# Feature F53 – Manage Doctors

## Description

Allow administrators to manage doctor accounts.

### Features

* View Doctors
* Search Doctors
* Add Doctor
* Edit Doctor
* Delete Doctor
* Activate / Deactivate Doctor

### Doctor Information

* Name
* Email
* Specialization
* Qualification
* Experience
* Consultation Fee
* Status

### Dependencies

F52

### Files Impacted

* AdminController.cs
* DoctorRepository.cs
* DoctorService.cs
* ManageDoctors.cshtml

### Acceptance Criteria

CRUD operations work correctly.

### Verification

* Add doctor
* Edit doctor
* Delete doctor
* Search doctor

---

# Feature F54 – Add Doctor

## Description

Allow administrators to register new doctors directly.

### Form Fields

* Full Name
* Email
* Password
* Qualification
* Specialization
* Experience
* Consultation Fee
* Biography

System automatically creates

* User
* Doctor Profile

Role

Doctor

### Dependencies

F53

### Acceptance Criteria

Doctor created successfully.

### Verification

Login using newly created doctor account.

---

# Feature F55 – Manage Patients

## Description

Allow administrators to manage patient accounts.

### Features

* View Patients
* Search Patients
* View Patient Profile
* Edit Patient
* Activate / Deactivate Patient

### Information

* Name
* Email
* Phone
* Gender
* Registration Date

### Dependencies

F52

### Files Impacted

* AdminController.cs
* PatientRepository.cs
* PatientService.cs
* ManagePatients.cshtml

### Acceptance Criteria

Patient management works.

### Verification

Search and update patient.

---

# Feature F56 – Manage Appointments

## Description

Allow administrators to monitor all appointments.

### Features

* View All Appointments
* Search
* Filter
* Sort
* View Details

### Filters

* Status
* Doctor
* Patient
* Date
* Specialization

### Sorting

* Date
* Doctor
* Patient
* Status

### Dependencies

F52

### Files Impacted

* AppointmentRepository.cs
* AppointmentService.cs
* ManageAppointments.cshtml

### Acceptance Criteria

Appointments displayed correctly.

### Verification

Test filters and search.

---

# Feature F57 – Appointment Details

## Description

Provide administrators with complete appointment information.

### Information

* Appointment ID
* Doctor
* Patient
* Date
* Time
* Status
* Reason
* Created Date
* Updated Date

### Dependencies

F56

### Acceptance Criteria

Complete information displayed.

### Verification

Open multiple appointments.

---

# Feature F58 – Platform Search

## Description

Implement global platform search.

### Search Areas

* Doctors
* Patients
* Appointments

### Search Fields

* Name
* Email
* Appointment ID
* Specialization

### Dependencies

F53

### Acceptance Criteria

Search results accurate.

### Verification

Search every entity.

---

# Feature F59 – Platform Statistics

## Description

Generate platform analytics.

### Statistics

* Total Users
* Total Doctors
* Total Patients
* Total Appointments
* Pending
* Confirmed
* Completed
* Cancelled
* Declined
* Rescheduled

### Charts

* Appointment Status Distribution
* Monthly Appointments
* Doctor Specializations
* Patient Registrations

### Dependencies

F52

### Acceptance Criteria

Statistics generated correctly.

### Verification

Compare dashboard data with database.

---

# Feature F60 – Admin Profile

## Description

Allow administrators to update personal information.

### Information

* Full Name
* Email (Read Only)
* Phone
* Address

### Account

* Change Password

### Dependencies

F52

### Files Impacted

* AdminProfile.cshtml
* AdminController.cs

### Acceptance Criteria

Profile updates successfully.

### Verification

Update profile and reload page.

---

# Feature F61 – Change Password

## Description

Allow administrators to securely update their password.

### Validation

* Current Password
* New Password
* Confirm Password

### Rules

* Password hashing
* Password confirmation
* Minimum password length

### Dependencies

F60

### Acceptance Criteria

Password updated successfully.

### Verification

Login using new password.

---

# Feature F62 – Audit Logging (Foundation)

## Description

Prepare the system for future audit tracking.

Version 1 stores basic administrative actions.

### Logged Actions

* Doctor Created
* Doctor Updated
* Doctor Deleted
* Patient Updated
* Appointment Updated
* Administrator Login

Future versions can expand this into a dedicated audit system.

### Dependencies

F52

### Acceptance Criteria

Critical administrative actions logged.

### Verification

Perform admin operations and verify logs.

---

# Phase 6 Completion Checklist

* Admin Dashboard
* Manage Doctors
* Add Doctor
* Manage Patients
* Manage Appointments
* Appointment Details
* Global Search
* Platform Statistics
* Admin Profile
* Change Password
* Audit Logging

---

# Manual Testing

### Dashboard

* Statistics
* Charts
* Quick Actions

### Doctor Management

* Add
* Edit
* Delete
* Search

### Patient Management

* Search
* Update
* View

### Appointment Management

* Search
* Filter
* View Details

### Profile

* Update Profile
* Change Password

### Responsive Testing

* Desktop
* Tablet
* Mobile

---

# Git Milestone

## Commit

```text
Complete Admin Module with dashboard, doctor management, patient management, appointment management, analytics, and administrative tools
```

## Push

```bash
git add .

git commit -m "Complete Admin Module with dashboard, doctor management, patient management, appointment management, analytics, and administrative tools"

git push origin main
```

---

# Stage Status

✅ Phase 6 Complete

Ready for **Phase 7 – UI Polish, Responsive Design, Validation, Error Handling, Performance Optimization, Accessibility, Testing, and Final Production Readiness**.

# Phase 7 – UI Polish, Quality Assurance & Production Readiness

## Goal

Transform MediBook from a functional application into a polished, professional, production-ready healthcare platform.

This phase focuses on improving the overall user experience, performance, accessibility, responsiveness, security, maintainability, and preparing the project for deployment.

---

# Feature F63 – Complete Responsive Design

## Description

Ensure every page provides an excellent experience across all screen sizes.

### Supported Devices

* Desktop
* Laptop
* Tablet
* Mobile

### Tasks

* Responsive Navbar
* Responsive Cards
* Responsive Tables
* Responsive Forms
* Responsive Dashboard
* Responsive Appointment Pages
* Responsive Admin Module

### Dependencies

* Phase 6

### Acceptance Criteria

Application works correctly on all supported devices.

### Verification

Test using browser responsive mode and physical devices.

---

# Feature F64 – UI Consistency

## Description

Ensure consistent styling throughout the application.

### Tasks

Standardize

* Buttons
* Cards
* Tables
* Forms
* Alerts
* Badges
* Navigation
* Typography
* Colors
* Icons
* Spacing

### Dependencies

F63

### Acceptance Criteria

All pages follow the same design language.

### Verification

Review every module.

---

# Feature F65 – Form Validation

## Description

Improve validation across the application.

### Validation Types

Client-side

* Required Fields
* Email
* Password
* Phone Number
* Dates

Server-side

* Duplicate Email
* Appointment Validation
* Availability Validation

### Dependencies

F64

### Acceptance Criteria

Invalid input handled gracefully.

### Verification

Submit invalid forms.

---

# Feature F66 – Error Pages

## Description

Create friendly error pages.

### Pages

* 400 Bad Request
* 401 Unauthorized
* 403 Forbidden
* 404 Not Found
* 500 Internal Server Error

### Dependencies

F65

### Acceptance Criteria

Users never see raw exceptions.

### Verification

Trigger each error.

---

# Feature F67 – Notifications & User Feedback

## Description

Provide visual feedback after user actions.

### Notifications

* Success
* Warning
* Error
* Information

### Actions

* Login
* Registration
* Appointment Booking
* Appointment Update
* Profile Update
* Delete Operations

### Dependencies

F65

### Acceptance Criteria

Users receive clear feedback.

### Verification

Test all CRUD operations.

---

# Feature F68 – Performance Optimization

## Description

Improve application performance.

### Tasks

* Optimize SQL Queries
* Reduce Database Calls
* Optimize Images
* Minify CSS
* Minify JavaScript
* Optimize Bootstrap Assets
* Remove Unused Code

### Dependencies

F67

### Acceptance Criteria

Improved page load time.

### Verification

Measure load times before and after optimization.

---

# Feature F69 – Accessibility Improvements

## Description

Ensure the application is accessible.

### Tasks

* Keyboard Navigation
* Proper Labels
* Alt Text
* Semantic HTML
* ARIA Attributes
* Color Contrast
* Focus Indicators

### Dependencies

F68

### Acceptance Criteria

Application meets basic WCAG accessibility guidelines.

### Verification

Keyboard-only navigation and accessibility audit.

---

# Feature F70 – Security Hardening

## Description

Review and strengthen application security.

### Tasks

* Review Authentication
* Verify Authorization
* Validate Sessions
* Prevent SQL Injection
* Prevent XSS
* Prevent CSRF
* Secure Cookies
* Secure Configuration

### Dependencies

F69

### Acceptance Criteria

Security review completed successfully.

### Verification

Perform security testing.

---

# Feature F71 – Code Refactoring

## Description

Improve maintainability without changing functionality.

### Tasks

* Remove Duplicate Code
* Improve Naming
* Simplify Methods
* Organize Files
* Improve Comments
* Remove Dead Code

### Dependencies

F70

### Acceptance Criteria

Codebase is clean and maintainable.

### Verification

Review project structure.

---

# Feature F72 – Comprehensive Testing

## Description

Perform complete system testing.

### Test Areas

#### Public Module

* Landing Page
* Login
* Registration

#### Patient Module

* Dashboard
* Find Doctors
* Booking
* Appointment History
* Profile

#### Doctor Module

* Dashboard
* Appointment Requests
* Schedule
* Profile

#### Admin Module

* Dashboard
* Doctor Management
* Patient Management
* Appointment Management

### Dependencies

F71

### Acceptance Criteria

All modules function correctly.

### Verification

Complete end-to-end testing.

---

# Feature F73 – Bug Fixing

## Description

Resolve all issues identified during testing.

### Categories

* UI Bugs
* Validation Bugs
* Database Bugs
* Authentication Bugs
* Appointment Workflow Bugs
* Responsive Bugs

### Dependencies

F72

### Acceptance Criteria

No critical or major bugs remain.

### Verification

Regression testing completed.

---

# Feature F74 – Deployment Preparation

## Description

Prepare the project for production deployment.

### Tasks

* Production Configuration
* Environment Variables
* Database Connection Verification
* Publish Profile
* Build Optimization
* Final Cleanup

### Dependencies

F73

### Acceptance Criteria

Project is deployment-ready.

### Verification

Successful Release build.

---

# Phase 7 Completion Checklist

* Responsive Design
* UI Consistency
* Validation
* Error Pages
* Notifications
* Performance Optimization
* Accessibility
* Security Review
* Code Refactoring
* System Testing
* Bug Fixes
* Deployment Preparation

---

# Final Project Verification

## Authentication

* Registration
* Login
* Logout
* Authorization

## Patient Module

* Dashboard
* Find Doctors
* Doctor Profile
* Book Appointment
* Appointment History
* Cancel Appointment
* Profile

## Doctor Module

* Dashboard
* Appointment Requests
* Accept
* Decline
* Reschedule
* Complete
* Schedule
* Profile

## Admin Module

* Dashboard
* Doctors
* Patients
* Appointments
* Statistics

## General

* Responsive Design
* Accessibility
* Validation
* Error Handling
* Performance
* Security

---

# Git Milestone

## Commit

```text
Complete UI polish, optimization, testing, bug fixes, and production readiness
```

## Push

```bash
git add .

git commit -m "Complete UI polish, optimization, testing, bug fixes, and production readiness"

git push origin main
```

---

# Final Release Checklist

* Build succeeds without warnings.
* No runtime errors.
* Database migrations complete.
* All CRUD operations verified.
* Authentication secure.
* Authorization verified.
* Appointment workflow verified.
* Responsive design tested.
* Accessibility reviewed.
* Documentation updated.
* Git repository synchronized.
* Final release tagged.

---

# Release Tag

```bash
git tag -a v1.0.0 -m "MediBook Version 1.0.0"

git push origin main --tags
```

---

# Project Completion Status

**Development Progress:** **100%**

✅ Phase 1 – Foundation

✅ Phase 2 – Authentication & Authorization

✅ Phase 3 – Public Module

✅ Phase 4 – Patient Module

✅ Phase 5 – Doctor Module

✅ Phase 6 – Admin Module

✅ Phase 7 – UI Polish, Testing & Production Readiness

---

# Final Deliverables

Upon completion, the project will include:

* ASP.NET Core MVC (.NET 8) application
* Custom Authentication & Role-Based Authorization
* Neon PostgreSQL database integration using ADO.NET (Npgsql)
* Patient, Doctor, and Admin portals
* Complete appointment scheduling workflow
* Responsive Bootstrap 5 interface
* Secure and maintainable MVC architecture
* Production-ready codebase
* Comprehensive project documentation
* Well-structured Git commit history with milestone-based commits
* Version **v1.0.0** tagged and ready for deployment

The project is now considered ready for deployment and future feature expansion.