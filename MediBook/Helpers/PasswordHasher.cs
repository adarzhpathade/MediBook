using BCrypt.Net;

namespace MediBook.Helpers
{
    public static class PasswordHasher
    {
        /// <summary>
        /// Hashes a plain text password using BCrypt.
        /// </summary>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, hashType: HashType.SHA384);
        }

        /// <summary>
        /// Verifies a plain text password against a hashed password.
        /// </summary>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword, hashType: HashType.SHA384);
        }
    }
}
