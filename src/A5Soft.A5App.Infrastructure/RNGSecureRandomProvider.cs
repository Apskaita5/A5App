using A5Soft.A5App.Application.Infrastructure;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Text;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Infrastructure
{
    /// <summary>
    /// <see cref="ISecureRandomProvider"/> implementation using <see cref="RNGCryptoServiceProvider"/>.
    /// </summary>
    [DefaultServiceImplementation(typeof(ISecureRandomProvider))]
    public class RNGSecureRandomProvider : ISecureRandomProvider
    {
        private static readonly char[] PasswordChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
            .ToCharArray();


        /// <inheritdoc cref="ISecureRandomProvider.GetSecureRandom"/>
        public int GetSecureRandom(int min, int max)
        {
            var threshold = max - min;
            if (threshold != Math.Abs(threshold)) throw new ArithmeticException(
                "The minimum value can't exceed the maximum value!");
            if (threshold == 0) throw new ArithmeticException(
                "The minimum value can't be the same as the maximum value!");

            byte[] bytes = new byte[4];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            return min + (Math.Abs(BitConverter.ToInt32(bytes, 0)) % threshold);
        }

        /// <inheritdoc cref="ISecureRandomProvider.CreateNewPassword"/>
        public string CreateNewPassword(int length)
        {
            var actualLength = length;
            if (length < 6) actualLength = 6;
            if (length > 9) actualLength = 9;

            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[actualLength];
                crypto.GetNonZeroBytes(data);
            }

            StringBuilder result = new StringBuilder(actualLength);
            foreach (byte b in data)
            {
                result.Append(PasswordChars[b % (PasswordChars.Length)]);
            }
            return result.ToString();
        }

        /// <summary>
        /// Creates a new cryptographically secure random token value, e.g. for password reset.
        /// </summary>
        /// <remarks>token format xxxxxx-xxxxxx-xxxxxx-xxxxxx, where xxxxxx - zero padded random int</remarks>
        public string CreateSecureUrlToken()
        {
            var result = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                result.Add(GetSecureRandom(0, 1000000).ToString().PadLeft(6, '0'));
            }

            return string.Join("-", result);
        }

        /// <inheritdoc cref="ISecureRandomProvider.CreateNewTwoFactorToken"/>
        public string CreateNewTwoFactorToken(int length)
        {
            var actualLength = length;
            if (length < 4) actualLength = 4;
            if (length > 9) actualLength = 9;

            var maxValue = 1;
            for (int i = 0; i < actualLength; i++)
            {
                maxValue *= 10;
            }

            var randomInt = GetSecureRandom(0, maxValue);

            return randomInt.ToString().PadLeft(actualLength, '0');
        }

    }
}
