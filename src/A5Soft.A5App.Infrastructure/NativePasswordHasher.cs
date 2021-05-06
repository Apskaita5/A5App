using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.CARMA.Domain;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace A5Soft.A5App.Infrastructure
{
    /// <summary>
    /// <see cref="IPasswordHasher"/> implementation using <see cref="KeyDerivation.Pbkdf2"/>.
    /// </summary>
    [DefaultServiceImplementation(typeof(IPasswordHasher))]
    public class NativePasswordHasher : IPasswordHasher
    {
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create(); // secure PRNG


        /// <inheritdoc cref="IPasswordHasher.HashPassword"/>
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
            return Convert.ToBase64String(HashPasswordV1(password));
        }

        /// <inheritdoc cref="IPasswordHasher.VerifyHashedPassword"/>
        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {

            if (string.IsNullOrWhiteSpace(hashedPassword)) throw new ArgumentNullException(nameof(hashedPassword));
            if (string.IsNullOrWhiteSpace(providedPassword)) throw new ArgumentNullException(nameof(providedPassword));

            // Convert the stored Base64 password to bytes
            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            if (decodedHashedPassword.Length == 0) return false;

                // The first byte indicates the format of the stored hash
            switch (decodedHashedPassword[0])
            {
                case 0x00:
                    return VerifyHashedPasswordV1(decodedHashedPassword, providedPassword);

                default:
                    return false; // unknown format marker
            }

        }


        private byte[] HashPasswordV1(string password)
        {

            var prf = KeyDerivationPrf.HMACSHA256;
            var saltSize = 128 / 8;
            var numBytesRequested = 256 / 8;
            var iterCount = 10000;

            // Produce a version 3 (see comment above) text hash.
            byte[] salt = new byte[saltSize];
            _rng.GetBytes(salt);
            byte[] subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

            var outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x00; // format marker
            WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
            WriteNetworkByteOrder(outputBytes, 5, (uint)iterCount);
            WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
            return outputBytes;
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }


        private bool VerifyHashedPasswordV1(byte[] hashedPassword, string password)
        {

            int iterCount = 0;

            try
            {

                // Read header information
                KeyDerivationPrf prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
                iterCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
                int saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

                // Read the salt: must be >= 128 bits
                if (saltLength < 128 / 8)
                {
                    return false;
                }
                byte[] salt = new byte[saltLength];
                Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

                // Read the subkey (the rest of the payload): must be >= 128 bits
                int subkeyLength = hashedPassword.Length - 13 - salt.Length;
                if (subkeyLength < 128 / 8)
                {
                    return false;
                }
                byte[] expectedSubkey = new byte[subkeyLength];
                Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

                // Hash the incoming password and verify it
                byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subkeyLength);
                return ByteArraysEqual(actualSubkey, expectedSubkey);

            }
            catch (Exception)
            {
                // This should never occur except in the case of a malformed payload, where
                // we might go off the end of the array. Regardless, a malformed payload
                // implies verification failed.
                return false; 
            }

        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset + 0]) << 24)
                | ((uint)(buffer[offset + 1]) << 16)
                | ((uint)(buffer[offset + 2]) << 8)
                | ((uint)(buffer[offset + 3]));
        }

        // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }

    }

}
