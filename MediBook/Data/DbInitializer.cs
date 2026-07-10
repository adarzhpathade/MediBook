using Npgsql;

namespace MediBook.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(DbConnectionFactory connectionFactory)
        {
            using var connection = connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // Create Users table
                var createUsersTableCommand = new NpgsqlCommand(@"
                    CREATE TABLE IF NOT EXISTS Users (
                        UserId SERIAL PRIMARY KEY,
                        FullName VARCHAR(150) NOT NULL,
                        Email VARCHAR(150) UNIQUE NOT NULL,
                        PasswordHash TEXT NOT NULL,
                        Role VARCHAR(20) NOT NULL,
                        IsActive BOOLEAN DEFAULT TRUE,
                        CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    );

                    CREATE UNIQUE INDEX IF NOT EXISTS idx_users_email ON Users(Email);

                    CREATE TABLE IF NOT EXISTS Patients (
                        PatientId SERIAL PRIMARY KEY,
                        UserId INTEGER NOT NULL,
                        Phone VARCHAR(20),
                        Gender VARCHAR(20),
                        DateOfBirth DATE,
                        Address TEXT,
                        CONSTRAINT fk_patient_user FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
                    );

                    CREATE TABLE IF NOT EXISTS Doctors (
                        DoctorId SERIAL PRIMARY KEY,
                        UserId INTEGER NOT NULL,
                        Specialization VARCHAR(100) NOT NULL,
                        Qualification VARCHAR(200) NOT NULL,
                        Experience INTEGER,
                        ConsultationFee DECIMAL(10,2),
                        Biography TEXT,
                        AvailableDays TEXT,
                        AvailableTime TEXT,
                        ProfileImage VARCHAR(255),
                        CONSTRAINT fk_doctor_user FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
                    );

                    CREATE INDEX IF NOT EXISTS idx_doctor_specialization ON Doctors(Specialization);
                    CREATE INDEX IF NOT EXISTS idx_doctor_experience ON Doctors(Experience);
                    CREATE INDEX IF NOT EXISTS idx_doctor_fee ON Doctors(ConsultationFee);

                    CREATE TABLE IF NOT EXISTS DoctorAvailability (
                        AvailabilityId SERIAL PRIMARY KEY,
                        DoctorId INTEGER NOT NULL,
                        DayOfWeek INTEGER NOT NULL,
                        StartTime TIME NOT NULL,
                        EndTime TIME NOT NULL,
                        SlotDuration INTEGER NOT NULL,
                        CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        CONSTRAINT fk_availability_doctor FOREIGN KEY (DoctorId) REFERENCES Doctors(DoctorId) ON DELETE CASCADE
                    );
                    CREATE INDEX IF NOT EXISTS idx_doctoravailability_doctor ON DoctorAvailability(DoctorId);

                    CREATE TABLE IF NOT EXISTS Appointments (
                        AppointmentId SERIAL PRIMARY KEY,
                        PatientId INTEGER NOT NULL,
                        DoctorId INTEGER NOT NULL,
                        AppointmentDate DATE NOT NULL,
                        AppointmentTime TIME NOT NULL,
                        Reason TEXT,
                        Status VARCHAR(30) NOT NULL,
                        CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        UpdatedAt TIMESTAMP NULL,
                        CONSTRAINT fk_appointment_patient FOREIGN KEY (PatientId) REFERENCES Patients(PatientId) ON DELETE CASCADE,
                        CONSTRAINT fk_appointment_doctor FOREIGN KEY (DoctorId) REFERENCES Doctors(DoctorId) ON DELETE CASCADE
                    );

                    CREATE INDEX IF NOT EXISTS idx_appointment_date ON Appointments(AppointmentDate);
                    CREATE INDEX IF NOT EXISTS idx_appointment_status ON Appointments(Status);
                    CREATE INDEX IF NOT EXISTS idx_appointment_patient ON Appointments(PatientId);
                    CREATE INDEX IF NOT EXISTS idx_appointment_doctor ON Appointments(DoctorId);

                    CREATE TABLE IF NOT EXISTS AuditLogs (
                        LogId SERIAL PRIMARY KEY,
                        Action VARCHAR(100) NOT NULL,
                        Details TEXT,
                        UserId INTEGER NULL,
                        Timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        CONSTRAINT fk_audit_user FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE SET NULL
                    );
                ", connection, transaction);
                await createUsersTableCommand.ExecuteNonQueryAsync();

                // Check if new seed data exists
                var checkSeededCommand = new NpgsqlCommand("SELECT COUNT(*) FROM Users WHERE Email = 'dr.arjun@medibook.com'", connection, transaction);
                var isSeeded = (long)(await checkSeededCommand.ExecuteScalarAsync() ?? 0) > 0;

                if (!isSeeded)
                {
                    // Wipe existing data
                    var wipeCommand = new NpgsqlCommand("TRUNCATE TABLE Users CASCADE;", connection, transaction);
                    await wipeCommand.ExecuteNonQueryAsync();

                    // Seed Admin
                    string adminHash = MediBook.Helpers.PasswordHasher.HashPassword("Admin123!");
                    var seedAdminCommand = new NpgsqlCommand(@"
                        INSERT INTO Users (FullName, Email, PasswordHash, Role)
                        VALUES ('System Administrator', 'admin@medibook.com', @Hash, 'Admin')
                    ", connection, transaction);
                    seedAdminCommand.Parameters.AddWithValue("@Hash", adminHash);
                    await seedAdminCommand.ExecuteNonQueryAsync();

                    var doctorsToSeed = new List<(string Name, string Email, string Spec, string Qual, int Exp, decimal Fee, string Bio)>
                    {
                        ("Dr. Rajesh Kumar", "dr.rajesh@medibook.com", "Cardiology", "MBBS, MD, DM", 20, 800.00m, "Senior Cardiologist with extensive experience in interventional cardiology."),
                        ("Dr. Priya Sharma", "dr.priya@medibook.com", "Dermatology", "MBBS, MD", 12, 600.00m, "Expert in clinical dermatology and aesthetic procedures."),
                        ("Dr. Amit Patel", "dr.amit@medibook.com", "Neurology", "MBBS, MD, DM", 15, 1000.00m, "Board-certified neurologist with extensive experience in treating complex neurological disorders."),
                        ("Dr. Sneha Desai", "dr.sneha@medibook.com", "Pediatrics", "MBBS, MD", 10, 500.00m, "Compassionate pediatrician dedicated to the health and well-being of infants, children, and adolescents."),
                        ("Dr. Arjun Reddy", "dr.arjun@medibook.com", "Orthopedics", "MBBS, MS", 18, 900.00m, "Specialist in joint replacement and sports injuries."),
                        ("Dr. Kavita Iyer", "dr.kavita@medibook.com", "Gynecology", "MBBS, MD, DGO", 22, 700.00m, "Dedicated gynecologist with expertise in high-risk pregnancies."),
                        ("Dr. Vikram Singh", "dr.vikram@medibook.com", "Oncology", "MBBS, MD, DM", 14, 1200.00m, "Leading oncologist focused on targeted therapies and immunotherapy."),
                        ("Dr. Neha Verma", "dr.neha@medibook.com", "Psychiatry", "MBBS, MD", 11, 800.00m, "Experienced psychiatrist specializing in cognitive behavioral therapy."),
                        ("Dr. Siddharth Rao", "dr.siddharth@medibook.com", "Ophthalmology", "MBBS, MS, DO", 16, 650.00m, "Expert in cataract surgery and laser vision correction."),
                        ("Dr. Anjali Joshi", "dr.anjali@medibook.com", "Endocrinology", "MBBS, MD, DM", 13, 900.00m, "Specializes in diabetes management and thyroid disorders."),
                        ("Dr. Manish Tiwari", "dr.manish@medibook.com", "Gastroenterology", "MBBS, MD, DM", 19, 1000.00m, "Advanced therapeutic endoscopy and hepatology expert."),
                        ("Dr. Pooja Agarwal", "dr.pooja@medibook.com", "ENT", "MBBS, MS", 9, 600.00m, "Otolaryngologist skilled in sinus surgeries and pediatric ENT."),
                        ("Dr. Deepak Kumar", "dr.deepak@medibook.com", "Pulmonology", "MBBS, MD", 17, 850.00m, "Focuses on asthma, COPD, and sleep apnea treatments."),
                        ("Dr. Ritu Kapoor", "dr.ritu@medibook.com", "Rheumatology", "MBBS, MD, DM", 12, 1100.00m, "Expertise in treating autoimmune conditions and arthritis."),
                        ("Dr. Sanjay Gupta", "dr.sanjay@medibook.com", "Urology", "MBBS, MS, MCh", 21, 1000.00m, "Specializes in minimally invasive urologic surgeries."),
                        ("Dr. Meera Reddy", "dr.meera@medibook.com", "Nephrology", "MBBS, MD, DM", 15, 950.00m, "Dedicated to advanced kidney disease management and dialysis."),
                        ("Dr. Karan Malhotra", "dr.karan@medibook.com", "Plastic Surgery", "MBBS, MS, MCh", 14, 1500.00m, "Renowned cosmetic and reconstructive surgeon."),
                        ("Dr. Swati Nair", "dr.swati@medibook.com", "Dentistry", "BDS, MDS", 8, 400.00m, "Expert in aesthetic dentistry and root canal treatments."),
                        ("Dr. Anil Choudhury", "dr.anil@medibook.com", "General Surgery", "MBBS, MS", 25, 800.00m, "Veteran surgeon with decades of experience in trauma care."),
                        ("Dr. Divya Pillai", "dr.divya@medibook.com", "Family Medicine", "MBBS, DNB", 10, 500.00m, "Comprehensive healthcare provider for families and communities.")
                    };

                    string doctorHash = MediBook.Helpers.PasswordHasher.HashPassword("Doctor123!");
                    var doctorIds = new List<int>();

                    foreach (var doc in doctorsToSeed)
                    {
                        var seedDoctorCommand = new NpgsqlCommand(@"
                            INSERT INTO Users (FullName, Email, PasswordHash, Role)
                            VALUES (@Name, @Email, @Hash, 'Doctor')
                            RETURNING UserId;
                        ", connection, transaction);
                        seedDoctorCommand.Parameters.AddWithValue("@Name", doc.Name);
                        seedDoctorCommand.Parameters.AddWithValue("@Email", doc.Email);
                        seedDoctorCommand.Parameters.AddWithValue("@Hash", doctorHash);
                        
                        var doctorUserId = Convert.ToInt32(await seedDoctorCommand.ExecuteScalarAsync());

                        var seedDoctorProfile = new NpgsqlCommand(@"
                            INSERT INTO Doctors (UserId, Specialization, Qualification, Experience, ConsultationFee, Biography)
                            VALUES (@UserId, @Spec, @Qual, @Exp, @Fee, @Bio)
                            RETURNING DoctorId;
                        ", connection, transaction);
                        seedDoctorProfile.Parameters.AddWithValue("@UserId", doctorUserId);
                        seedDoctorProfile.Parameters.AddWithValue("@Spec", doc.Spec);
                        seedDoctorProfile.Parameters.AddWithValue("@Qual", doc.Qual);
                        seedDoctorProfile.Parameters.AddWithValue("@Exp", doc.Exp);
                        seedDoctorProfile.Parameters.AddWithValue("@Fee", doc.Fee);
                        seedDoctorProfile.Parameters.AddWithValue("@Bio", doc.Bio);
                        
                        var newDoctorId = Convert.ToInt32(await seedDoctorProfile.ExecuteScalarAsync());
                        doctorIds.Add(newDoctorId);

                        var seedDoctorAvailability = new NpgsqlCommand(@"
                            INSERT INTO DoctorAvailability (DoctorId, DayOfWeek, StartTime, EndTime, SlotDuration)
                            VALUES 
                            (@DoctorId, 1, '09:00:00', '17:00:00', 30),
                            (@DoctorId, 2, '09:00:00', '17:00:00', 30),
                            (@DoctorId, 3, '09:00:00', '17:00:00', 30),
                            (@DoctorId, 4, '09:00:00', '17:00:00', 30),
                            (@DoctorId, 5, '09:00:00', '17:00:00', 30),
                            (@DoctorId, 6, '10:00:00', '14:00:00', 30)
                        ", connection, transaction);
                        seedDoctorAvailability.Parameters.AddWithValue("@DoctorId", newDoctorId);
                        await seedDoctorAvailability.ExecuteNonQueryAsync();
                    }

                    var patientsToSeed = new List<(string Name, string Email, string Phone, string Gender)>
                    {
                        ("Rahul Gupta", "rahul@medibook.com", "+91 9876543210", "Male"),
                        ("Anjali Desai", "anjali@medibook.com", "+91 8765432109", "Female"),
                        ("Vikram Singh", "vikram@medibook.com", "+91 7654321098", "Male"),
                        ("Neha Verma", "neha@medibook.com", "+91 6543210987", "Female")
                    };

                    string patientHash = MediBook.Helpers.PasswordHasher.HashPassword("Patient123!");
                    var patientIds = new List<int>();

                    foreach (var pat in patientsToSeed)
                    {
                        var seedPatientCommand = new NpgsqlCommand(@"
                            INSERT INTO Users (FullName, Email, PasswordHash, Role)
                            VALUES (@Name, @Email, @Hash, 'Patient')
                            RETURNING UserId;
                        ", connection, transaction);
                        seedPatientCommand.Parameters.AddWithValue("@Name", pat.Name);
                        seedPatientCommand.Parameters.AddWithValue("@Email", pat.Email);
                        seedPatientCommand.Parameters.AddWithValue("@Hash", patientHash);
                        
                        var newUserId = Convert.ToInt32(await seedPatientCommand.ExecuteScalarAsync());

                        var seedPatientProfile = new NpgsqlCommand(@"
                            INSERT INTO Patients (UserId, Phone, Gender, DateOfBirth)
                            VALUES (@UserId, @Phone, @Gender, '1985-06-15')
                            RETURNING PatientId;
                        ", connection, transaction);
                        seedPatientProfile.Parameters.AddWithValue("@UserId", newUserId);
                        seedPatientProfile.Parameters.AddWithValue("@Phone", pat.Phone);
                        seedPatientProfile.Parameters.AddWithValue("@Gender", pat.Gender);
                        
                        var newPatientId = Convert.ToInt32(await seedPatientProfile.ExecuteScalarAsync());
                        patientIds.Add(newPatientId);
                    }

                    // Seed Appointments
                    var appointmentsToSeed = new List<(int PatIndex, int DocIndex, DateTime Date, TimeSpan Time, string Reason, string Status)>
                    {
                        // Past
                        (0, 0, DateTime.Today.AddDays(-14), new TimeSpan(10, 0, 0), "Routine heart checkup", "Completed"),
                        (1, 1, DateTime.Today.AddDays(-10), new TimeSpan(11, 30, 0), "Acne consultation", "Completed"),
                        (2, 2, DateTime.Today.AddDays(-5), new TimeSpan(14, 0, 0), "Migraine follow-up", "Completed"),
                        (3, 3, DateTime.Today.AddDays(-2), new TimeSpan(9, 30, 0), "Child vaccination", "Cancelled"),
                        
                        // Today
                        (0, 1, DateTime.Today, new TimeSpan(10, 0, 0), "Skin rash check", "Confirmed"),
                        (1, 2, DateTime.Today, new TimeSpan(13, 0, 0), "Nerve pain assessment", "Confirmed"),
                        
                        // Future Confirmed
                        (2, 0, DateTime.Today.AddDays(2), new TimeSpan(15, 0, 0), "Blood pressure monitoring", "Confirmed"),
                        (3, 1, DateTime.Today.AddDays(4), new TimeSpan(10, 30, 0), "Eczema treatment", "Confirmed"),
                        (0, 2, DateTime.Today.AddDays(7), new TimeSpan(11, 0, 0), "Headache consultation", "Confirmed"),
                        
                        // Future Pending
                        (1, 0, DateTime.Today.AddDays(5), new TimeSpan(14, 30, 0), "Cholesterol review", "Pending"),
                        (2, 3, DateTime.Today.AddDays(10), new TimeSpan(16, 0, 0), "Child fever", "Pending")
                    };

                    foreach (var appt in appointmentsToSeed)
                    {
                        var seedApptCommand = new NpgsqlCommand(@"
                            INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, AppointmentTime, Reason, Status)
                            VALUES (@PatientId, @DoctorId, @Date, @Time, @Reason, @Status)
                        ", connection, transaction);
                        seedApptCommand.Parameters.AddWithValue("@PatientId", patientIds[appt.PatIndex]);
                        seedApptCommand.Parameters.AddWithValue("@DoctorId", doctorIds[appt.DocIndex]);
                        seedApptCommand.Parameters.AddWithValue("@Date", appt.Date);
                        seedApptCommand.Parameters.AddWithValue("@Time", appt.Time);
                        seedApptCommand.Parameters.AddWithValue("@Reason", appt.Reason);
                        seedApptCommand.Parameters.AddWithValue("@Status", appt.Status);
                        await seedApptCommand.ExecuteNonQueryAsync();
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
