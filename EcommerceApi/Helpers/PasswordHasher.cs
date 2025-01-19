using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace EcommerceApi.Helpers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            Console.WriteLine($"Verifying password...");
            Console.WriteLine($"Stored hashed password: {hashedPassword}");
            
            var parts = hashedPassword.Split('.');
            Console.WriteLine($"Split parts count: {parts.Length}");
            
            if (parts.Length != 2)
            {
                Console.WriteLine("Invalid hash format - doesn't contain salt and hash parts");
                return false;
            }

            try
            {
                var salt = Convert.FromBase64String(parts[0]);
                var hash = parts[1];

                Console.WriteLine($"Extracted salt (base64): {parts[0]}");
                Console.WriteLine($"Extracted hash: {hash}");

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: providedPassword,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                Console.WriteLine($"Computed hash from provided password: {hashed}");
                Console.WriteLine($"Passwords match: {hash == hashed}");

                return hash == hashed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during password verification: {ex.Message}");
                return false;
            }
        }
    }
}
