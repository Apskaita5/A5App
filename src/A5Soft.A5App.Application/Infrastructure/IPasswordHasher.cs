namespace A5Soft.A5App.Application.Infrastructure
{
    /// <summary>
    /// An interface that should be implemented by a particular password (string) hashing implementation.
    /// </summary>
    public interface IPasswordHasher
    {

        /// <summary>
        /// Get a hash value string for the password string.
        /// </summary>
        /// <param name="password">a password to hash</param>
        /// <returns>a hash value string for the password string</returns>
        string HashPassword(string password);

        /// <summary>
        /// Verifies if the password string specified is the same as the hashed password value.
        /// </summary>
        /// <param name="hashedPassword">a password hash value in database</param>
        /// <param name="providedPassword">a password string to check</param>
        /// <returns>true, if the password string specified is the same as the hashed password value; false - otherwise</returns>
        bool VerifyHashedPassword(string hashedPassword, string providedPassword);

    }
}
