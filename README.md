# MediBook – Doctor Appointment Scheduler

## Overview
MediBook is a modern web-based Doctor Appointment Scheduler built using **ASP.NET Core MVC (.NET 8)**. It simplifies appointment booking between patients and healthcare providers by providing dedicated portals for Patients, Doctors, and Administrators.

The application focuses on providing a clean, secure, responsive, and intuitive experience while following a maintainable MVC architecture suitable for real-world healthcare environments.

## Features
- **Role-Based Access Control:** Separate portals for Patients, Doctors, and Admins.
- **Patient Features:** Search for doctors, view profiles, book appointments, and manage appointment history.
- **Doctor Features:** Manage schedules, accept/decline/reschedule appointments, and view appointment history.
- **Admin Features:** Manage doctors, patients, and platform statistics.
- **Security:** Custom authentication, role-based authorization, BCrypt password hashing, and anti-forgery measures.
- **Responsive UI:** Built with Bootstrap 5 and a premium clinical editorial design system.

## Technology Stack
- **Backend:** ASP.NET Core MVC (.NET 8), C#
- **Frontend:** Razor Views, Bootstrap 5, JavaScript
- **Database:** Neon PostgreSQL
- **Database Access:** ADO.NET with Npgsql

## Project Structure
- `MediBook/`: The main ASP.NET Core MVC application.
- `context/`: Project documentation, architecture, and planning files.
- `design/`: UI prototypes and design system reference.

## Setup Instructions
1. Ensure you have the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed.
2. Clone the repository.
3. Configure the PostgreSQL database connection string in `appsettings.json` or `appsettings.Development.json`.
4. Open a terminal in the `MediBook` directory.
5. Run `dotnet restore` to restore dependencies.
6. Run `dotnet run` (or `dotnet watch run` for hot reload) to start the application.
7. Access the application in your browser (typically at `http://localhost:5000` or `https://localhost:5001`).

## Version
**Current Version:** v1.0.0
