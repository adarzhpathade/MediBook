# database_schema.md

# MediBook Database Schema

This document defines the complete database design for the MediBook Doctor Appointment Scheduler.

The schema is normalized, scalable, and optimized for PostgreSQL while keeping the implementation simple for an ASP.NET Core MVC + ADO.NET application.

---

# Database Overview

**Database Engine**

* Neon PostgreSQL

**Database Access**

* ADO.NET
* Npgsql

**Architecture**

* Relational Database
* Normalized Design (3NF)
* Foreign Key Constraints
* Indexed Search Columns

---

# Entity Relationship Diagram (Logical)

```text
                   Users
                     │
         ┌───────────┴───────────┐
         │                       │
     Patients                Doctors
         │                       │
         └───────────┬───────────┘
                     │
              Appointments
```

---

# Table: Users

Stores login credentials and authentication information.

## Columns

| Column       | Data Type    | Constraints               |
| ------------ | ------------ | ------------------------- |
| UserId       | SERIAL       | Primary Key               |
| FullName     | VARCHAR(150) | NOT NULL                  |
| Email        | VARCHAR(150) | UNIQUE, NOT NULL          |
| PasswordHash | TEXT         | NOT NULL                  |
| Role         | VARCHAR(20)  | NOT NULL                  |
| IsActive     | BOOLEAN      | DEFAULT TRUE              |
| CreatedAt    | TIMESTAMP    | DEFAULT CURRENT_TIMESTAMP |

---

## Constraints

Primary Key

```sql
PRIMARY KEY (UserId)
```

Unique

```sql
UNIQUE (Email)
```

Role Values

* Patient
* Doctor
* Admin

---

# Table: Patients

Stores patient-specific information.

## Columns

| Column      | Data Type   | Constraints |
| ----------- | ----------- | ----------- |
| PatientId   | SERIAL      | Primary Key |
| UserId      | INTEGER     | Foreign Key |
| Phone       | VARCHAR(20) |             |
| Gender      | VARCHAR(20) |             |
| DateOfBirth | DATE        |             |
| Address     | TEXT        |             |

---

## Relationship

```text
Users (1)

↓

Patients (1)
```

Foreign Key

```sql
FOREIGN KEY (UserId)

REFERENCES Users(UserId)

ON DELETE CASCADE
```

---

# Table: Doctors

Stores doctor information.

## Columns

| Column          | Data Type     | Constraints |
| --------------- | ------------- | ----------- |
| DoctorId        | SERIAL        | Primary Key |
| UserId          | INTEGER       | Foreign Key |
| Specialization  | VARCHAR(100)  | NOT NULL    |
| Qualification   | VARCHAR(200)  | NOT NULL    |
| Experience      | INTEGER       |             |
| ConsultationFee | DECIMAL(10,2) |             |
| Biography       | TEXT          |             |
| AvailableDays   | TEXT          |             |
| AvailableTime   | TEXT          |             |
| ProfileImage    | VARCHAR(255)  | NULL        |

---

Relationship

```text
Users

↓

Doctors
```

Foreign Key

```sql
FOREIGN KEY(UserId)

REFERENCES Users(UserId)

ON DELETE CASCADE
```

---

# Table: Appointments

Stores appointment records.

## Columns

| Column          | Data Type   | Constraints               |
| --------------- | ----------- | ------------------------- |
| AppointmentId   | SERIAL      | Primary Key               |
| PatientId       | INTEGER     | Foreign Key               |
| DoctorId        | INTEGER     | Foreign Key               |
| AppointmentDate | DATE        | NOT NULL                  |
| AppointmentTime | TIME        | NOT NULL                  |
| Reason          | TEXT        |                           |
| Status          | VARCHAR(30) | NOT NULL                  |
| CreatedAt       | TIMESTAMP   | DEFAULT CURRENT_TIMESTAMP |
| UpdatedAt       | TIMESTAMP   | NULL                      |

---

Relationship

```text
Patients

↓

Appointments

↑

Doctors
```

Foreign Keys

```sql
FOREIGN KEY(PatientId)

REFERENCES Patients(PatientId)

ON DELETE CASCADE
```

```sql
FOREIGN KEY(DoctorId)

REFERENCES Doctors(DoctorId)

ON DELETE CASCADE
```

---

# Table: DoctorAvailability

Stores the recurring weekly availability schedule for doctors.

## Columns

| Column            | Data Type   | Constraints               |
| ----------------- | ----------- | ------------------------- |
| AvailabilityId    | SERIAL      | Primary Key               |
| DoctorId          | INTEGER     | Foreign Key               |
| DayOfWeek         | INTEGER     | NOT NULL (0=Sun, 6=Sat)   |
| StartTime         | TIME        | NOT NULL                  |
| EndTime           | TIME        | NOT NULL                  |
| SlotDuration      | INTEGER     | NOT NULL (in minutes)     |
| CreatedAt         | TIMESTAMP   | DEFAULT CURRENT_TIMESTAMP |

---

Relationship

```text
Doctors

↓

DoctorAvailability
```

Foreign Key

```sql
FOREIGN KEY(DoctorId)

REFERENCES Doctors(DoctorId)

ON DELETE CASCADE
```

---

# Appointment Status

Allowed values

```text
Pending

Confirmed

Declined

Rescheduled

Completed

Cancelled
```

---

# Relationship Summary

## Users → Patients

Type

One-to-One

---

## Users → Doctors

Type

One-to-One

---

## Patients → Appointments

Type

One-to-Many

---

## Doctors → Appointments

Type

One-to-Many

---

## Doctors → DoctorAvailability

Type

One-to-Many

---

# Normalization

The schema follows Third Normal Form (3NF).

Benefits

* No duplicated user data
* Consistent relationships
* Easier maintenance
* Reduced storage

---

# Indexing Strategy

## Users

```sql
CREATE UNIQUE INDEX idx_users_email
ON Users(Email);
```

---

## Doctors

```sql
CREATE INDEX idx_doctor_specialization
ON Doctors(Specialization);
```

```sql
CREATE INDEX idx_doctor_experience
ON Doctors(Experience);
```

```sql
CREATE INDEX idx_doctor_fee
ON Doctors(ConsultationFee);
```

---

## Appointments

```sql
CREATE INDEX idx_appointment_date
ON Appointments(AppointmentDate);
```

```sql
CREATE INDEX idx_appointment_status
ON Appointments(Status);
```

```sql
CREATE INDEX idx_patient
ON Appointments(PatientId);
```

```sql
CREATE INDEX idx_doctor
ON Appointments(DoctorId);
```

---

# Data Integrity Rules

Users

* Email must be unique.
* Password must always be hashed.
* Role cannot be NULL.

Doctors

* Consultation Fee cannot be negative.
* Experience cannot be negative.
* Qualification required.

Patients

* Date of Birth cannot be in the future.

Appointments

* Appointment Date cannot be in the past.
* Doctor must exist.
* Patient must exist.
* Status must be valid.
* Appointment must belong to one patient and one doctor.

---

# Business Rules

## Patient

One patient can create multiple appointments.

---

## Doctor

One doctor can receive multiple appointments.

---

## Appointment

A patient cannot book the same doctor at the same date and time.

A doctor cannot have two confirmed appointments at the same time.

Completed appointments become read-only.

Cancelled appointments remain in history.

---

# Database Transactions

Transactions are required for

* User Registration
* Doctor Creation
* Appointment Booking
* Appointment Rescheduling
* Appointment Cancellation

Rollback on failure.

---

# SQL Naming Conventions

Tables

Plural

```text
Users

Patients

Doctors

Appointments
```

Columns

PascalCase

```text
DoctorId

PatientId

AppointmentDate

CreatedAt
```

Primary Keys

```text
UserId

DoctorId

PatientId

AppointmentId
```

Foreign Keys

```text
UserId

DoctorId

PatientId
```

---

# Seed Data

Initial Administrator

| Field     | Value                                           |
| --------- | ----------------------------------------------- |
| Full Name | System Administrator                            |
| Email     | [admin@medibook.com](mailto:admin@medibook.com) |
| Password  | Hashed Password                                 |
| Role      | Admin                                           |

Future demo data

* Doctors
* Patients
* Appointments

---

# Future Database Expansion

The schema is designed for future additions without major structural changes.

Potential tables

* MedicalRecords
* Prescriptions
* Payments
* Notifications
* Reviews
* Clinics
* Departments
* DoctorAvailability
* AppointmentHistory
* AuditLogs
* Specializations

---

# Migration Strategy

Version 1

* Create tables
* Add foreign keys
* Add indexes
* Insert administrator

Future versions

* Alter tables when needed
* Preserve backward compatibility
* Use migration scripts with version control

---

# Backup Strategy

* Daily automated database backup
* Weekly full backup
* Monthly archive backup
* Backup verification before deployment

---

# AI Coding Agent Notes

When implementing database features:

* Never write SQL inside Controllers.
* Use Repositories for all database access.
* Always use parameterized SQL.
* Wrap multi-table operations in transactions.
* Validate all foreign keys before insert/update.
* Keep schema changes synchronized with documentation.
* After schema modifications:

  * Test database connectivity.
  * Verify constraints.
  * Update Git.
  * Commit and push changes.

This schema is the single source of truth for the MediBook database and must remain synchronized with the application throughout development.
