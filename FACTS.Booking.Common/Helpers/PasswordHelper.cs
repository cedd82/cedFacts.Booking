using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FACTS.GenericBooking.Common.Helpers
{
    public class PasswordHelper
    {
        /// <summary>
        ///     Generates a password using a salted hash.
        /// </summary>
        /// <param name="password">The password string to convert into a hash</param>
        /// <param name="passwordHash">The hash used to store the password</param>
        /// <param name="passwordSalt">The hash used to salt the password hash</param>
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using HMACSHA512 hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        /// <summary>
        ///     Verifies the password given with the stored hash and salt in the system. It will return true if the hash returned
        ///     is valid.
        /// </summary>
        /// <param name="password">The password to verify</param>
        /// <param name="storedHash">The stored hash to compare against</param>
        /// <param name="storedSalt">The stored salt to compare against</param>
        /// <returns>Returns whether the password is valid or not.</returns>
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (storedHash.Length != 64)
            {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(storedHash));
            }

            if (storedSalt.Length != 128)
            {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(storedSalt));
            }

            using HMACSHA512 hmac = new HMACSHA512(storedSalt);
            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return !computedHash.Where((t, i) => t != storedHash[i]).Any();
        }
    }
}
