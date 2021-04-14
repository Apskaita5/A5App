using A5Soft.CARMA.Domain;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace A5Soft.A5App.Domain.Security
{
    public static class Extensions
    {
    
        /// <summary>
        /// Gets a SHA256 hash value for a string.
        /// </summary>
        /// <param name="rawData">a string to compute SHA256 hash for</param>
        public static string ComputeSha256Hash(this string rawData)
        {
            if (rawData.IsNullOrWhiteSpace()) return string.Empty;
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets a hash of updatedAt for optimistic concurrency control.
        /// </summary>
        /// <typeparam name="T">a domain entity type to create a hash for</typeparam>
        /// <param name="updatedAt">last updated timestamp to create a hash for</param>
        /// <remarks>UpdatedAt value shall not be used as a user might not be authorized
        /// to access audit trail data.</remarks>
        public static string CreateOccHash<T>(this DateTime updatedAt)
        {
            return (typeof(T).FullName + updatedAt.Ticks.ToString()).ComputeSha256Hash();
        }

    }
}
