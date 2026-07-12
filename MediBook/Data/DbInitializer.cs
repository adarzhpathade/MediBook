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
                var checkSeededCommand = new NpgsqlCommand("SELECT COUNT(*) FROM Users WHERE Email = 'admin.root@medibook.com'", connection, transaction);
                var isSeeded = (long)(await checkSeededCommand.ExecuteScalarAsync() ?? 0) > 0;

                if (!isSeeded)
                {
                    // Wipe existing data
                    var wipeCommand = new NpgsqlCommand("TRUNCATE TABLE Users CASCADE;", connection, transaction);
                    await wipeCommand.ExecuteNonQueryAsync();

                    var rand = new Random(42); // Deterministic seed

                    // Seed Admin
                    string adminHash = MediBook.Helpers.PasswordHasher.HashPassword("Admin123!");
                    var seedAdminCommand = new NpgsqlCommand(@"
                        INSERT INTO Users (FullName, Email, PasswordHash, Role)
                        VALUES ('System Administrator', 'admin.root@medibook.com', @Hash, 'Admin')
                    ", connection, transaction);
                    seedAdminCommand.Parameters.AddWithValue("@Hash", adminHash);
                    await seedAdminCommand.ExecuteNonQueryAsync();

                    // --- GENERATE DOCTORS ---
                    string doctorHash = MediBook.Helpers.PasswordHasher.HashPassword("Doctor123!");
                    var doctorIds = new List<int>();

                    string[] firstNamesM = { "Rajesh", "Amit", "Vikram", "Siddharth", "Manish", "Deepak", "Sanjay", "Karan", "Anil", "Rahul", "Aarav", "Vihaan", "Aditya", "Sai", "Arjun", "Ravi", "Suresh", "Vishal", "Ashok", "Vijay", "Rohit", "Sameer", "Gaurav" };
                    string[] firstNamesF = { "Priya", "Sneha", "Kavita", "Neha", "Anjali", "Pooja", "Ritu", "Meera", "Swati", "Divya", "Anaya", "Diya", "Kavya", "Isha", "Riya", "Aarti", "Kiran", "Nisha", "Shalini", "Sunita", "Shruti", "Sonal", "Radhika" };
                    string[] lastNames = { "Patel", "Sharma", "Kumar", "Singh", "Reddy", "Rao", "Iyer", "Joshi", "Verma", "Agarwal", "Tiwari", "Kapoor", "Gupta", "Malhotra", "Nair", "Choudhury", "Pillai", "Das", "Bose", "Ghosh", "Desai", "Jain", "Bhat", "Menon", "Bansal", "Mehta", "Shah", "Sinha" };
                    string[] specialties = { "Cardiology", "Dermatology", "Neurology", "Pediatrics", "Orthopedics", "Gynecology", "Oncology", "Psychiatry", "Ophthalmology", "Endocrinology", "Gastroenterology", "ENT", "Pulmonology", "Rheumatology", "Urology", "Nephrology", "Plastic Surgery", "Dentistry", "General Surgery", "Family Medicine", "Ayurveda", "Homeopathy" };
                    string[] qualifications = { "MBBS, MD", "MBBS, MS", "MBBS, MD, DM", "MBBS, MS, MCh", "BDS, MDS", "BAMS, MD", "BHMS, MD", "MBBS, DNB" };

                    for (int i = 0; i < 50; i++)
                    {
                        bool isMale = rand.Next(2) == 0;
                        string firstName = isMale ? firstNamesM[rand.Next(firstNamesM.Length)] : firstNamesF[rand.Next(firstNamesF.Length)];
                        string lastName = lastNames[rand.Next(lastNames.Length)];
                        string name = $"Dr. {firstName} {lastName}";
                        string email = $"dr.{firstName.ToLower()}.{lastName.ToLower()}{i}@medibook.com";
                        string spec = specialties[rand.Next(specialties.Length)];
                        string qual = qualifications[rand.Next(qualifications.Length)];
                        int exp = rand.Next(5, 35);
                        decimal fee = rand.Next(4, 21) * 100.00m;
                        string bio = $"Experienced {spec} specialist with {exp} years of practice. Dedicated to providing compassionate and comprehensive care.";

                        var seedDoctorCommand = new NpgsqlCommand(@"
                            INSERT INTO Users (FullName, Email, PasswordHash, Role)
                            VALUES (@Name, @Email, @Hash, 'Doctor')
                            RETURNING UserId;
                        ", connection, transaction);
                        seedDoctorCommand.Parameters.AddWithValue("@Name", name);
                        seedDoctorCommand.Parameters.AddWithValue("@Email", email);
                        seedDoctorCommand.Parameters.AddWithValue("@Hash", doctorHash);
                        
                        var doctorUserId = Convert.ToInt32(await seedDoctorCommand.ExecuteScalarAsync());

                        var seedDoctorProfile = new NpgsqlCommand(@"
                            INSERT INTO Doctors (UserId, Specialization, Qualification, Experience, ConsultationFee, Biography)
                            VALUES (@UserId, @Spec, @Qual, @Exp, @Fee, @Bio)
                            RETURNING DoctorId;
                        ", connection, transaction);
                        seedDoctorProfile.Parameters.AddWithValue("@UserId", doctorUserId);
                        seedDoctorProfile.Parameters.AddWithValue("@Spec", spec);
                        seedDoctorProfile.Parameters.AddWithValue("@Qual", qual);
                        seedDoctorProfile.Parameters.AddWithValue("@Exp", exp);
                        seedDoctorProfile.Parameters.AddWithValue("@Fee", fee);
                        seedDoctorProfile.Parameters.AddWithValue("@Bio", bio);
                        
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

                    // --- GENERATE PATIENTS ---
                    string patientHash = MediBook.Helpers.PasswordHasher.HashPassword("Patient123!");
                    var patientIds = new List<int>();
                    
                    for (int i = 0; i < 500; i++)
                    {
                        bool isMale = rand.Next(2) == 0;
                        string firstName = isMale ? firstNamesM[rand.Next(firstNamesM.Length)] : firstNamesF[rand.Next(firstNamesF.Length)];
                        string lastName = lastNames[rand.Next(lastNames.Length)];
                        string name = $"{firstName} {lastName}";
                        string email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@example.com";
                        string phone = $"+91 9{rand.Next(10000000, 99999999)}{rand.Next(0, 9)}";
                        string gender = isMale ? "Male" : "Female";
                        DateTime dob = new DateTime(1950, 1, 1).AddDays(rand.Next((new DateTime(2015, 1, 1) - new DateTime(1950, 1, 1)).Days));

                        var seedPatientCommand = new NpgsqlCommand(@"
                            INSERT INTO Users (FullName, Email, PasswordHash, Role)
                            VALUES (@Name, @Email, @Hash, 'Patient')
                            RETURNING UserId;
                        ", connection, transaction);
                        seedPatientCommand.Parameters.AddWithValue("@Name", name);
                        seedPatientCommand.Parameters.AddWithValue("@Email", email);
                        seedPatientCommand.Parameters.AddWithValue("@Hash", patientHash);
                        
                        var newUserId = Convert.ToInt32(await seedPatientCommand.ExecuteScalarAsync());

                        var seedPatientProfile = new NpgsqlCommand(@"
                            INSERT INTO Patients (UserId, Phone, Gender, DateOfBirth)
                            VALUES (@UserId, @Phone, @Gender, @Dob)
                            RETURNING PatientId;
                        ", connection, transaction);
                        seedPatientProfile.Parameters.AddWithValue("@UserId", newUserId);
                        seedPatientProfile.Parameters.AddWithValue("@Phone", phone);
                        seedPatientProfile.Parameters.AddWithValue("@Gender", gender);
                        seedPatientProfile.Parameters.AddWithValue("@Dob", dob);
                        
                        var newPatientId = Convert.ToInt32(await seedPatientProfile.ExecuteScalarAsync());
                        patientIds.Add(newPatientId);
                    }

                    // --- GENERATE APPOINTMENTS ---
                    string[] reasons = { "Routine checkup", "Fever and cough", "Stomach pain", "Headache", "Follow-up consultation", "Skin rash", "Joint pain", "Blood pressure check", "Diabetes management", "Child vaccination", "Allergy symptoms", "General fatigue", "Back pain", "Vision problems", "Dental checkup" };
                    
                    for (int i = 0; i < 2000; i++)
                    {
                        int pId = patientIds[rand.Next(patientIds.Count)];
                        int dId = doctorIds[rand.Next(doctorIds.Count)];
                        
                        DateTime apptDate = DateTime.Today.AddDays(rand.Next(-60, 31));
                        int hours = rand.Next(9, 17);
                        int mins = (rand.Next(2) == 0) ? 0 : 30;
                        TimeSpan apptTime = new TimeSpan(hours, mins, 0);
                        string reason = reasons[rand.Next(reasons.Length)];
                        string status = "";

                        if (apptDate < DateTime.Today)
                            status = (rand.Next(10) < 8) ? "Completed" : "Cancelled";
                        else if (apptDate == DateTime.Today)
                            status = (rand.Next(10) < 6) ? "Confirmed" : ((rand.Next(10) < 3) ? "Pending" : "Completed");
                        else
                            status = (rand.Next(10) < 7) ? "Confirmed" : "Pending";

                        var seedApptCommand = new NpgsqlCommand(@"
                            INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, AppointmentTime, Reason, Status)
                            VALUES (@PatientId, @DoctorId, @Date, @Time, @Reason, @Status)
                        ", connection, transaction);
                        seedApptCommand.Parameters.AddWithValue("@PatientId", pId);
                        seedApptCommand.Parameters.AddWithValue("@DoctorId", dId);
                        seedApptCommand.Parameters.AddWithValue("@Date", apptDate);
                        seedApptCommand.Parameters.AddWithValue("@Time", apptTime);
                        seedApptCommand.Parameters.AddWithValue("@Reason", reason);
                        seedApptCommand.Parameters.AddWithValue("@Status", status);
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
