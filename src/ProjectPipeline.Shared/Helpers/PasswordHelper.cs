using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace ProjectPipeline.Shared.Helpers;

/// <summary>
/// Helper class for password hashing and verification
/// </summary>
public static class PasswordHelper
{
    /// <summary>
    /// Hashes a password using PBKDF2 with a random salt
    /// </summary>
    /// <param name="password">The password to hash</param>
    /// <returns>Base64 encoded hash with salt</returns>
    public static string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        // Generate a 128-bit salt using a secure PRNG
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        // Return salt + hash
        return $"{Convert.ToBase64String(salt)}.{hashed}";
    }

    /// <summary>
    /// Verifies a password against a hash
    /// </summary>
    /// <param name="password">The password to verify</param>
    /// <param name="hashedPassword">The stored hash</param>
    /// <returns>True if password matches</returns>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
            return false;

        try
        {
            var parts = hashedPassword.Split('.');
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var hash = parts[1];

            // Hash the provided password with the same salt
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hash == hashed;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Generates a random password
    /// </summary>
    /// <param name="length">Length of the password</param>
    /// <returns>Random password</returns>
    public static string GenerateRandomPassword(int length = 12)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        using var rng = RandomNumberGenerator.Create();
        var result = new char[length];
        var buffer = new byte[sizeof(uint)];

        for (int i = 0; i < length; i++)
        {
            rng.GetBytes(buffer);
            uint randomValue = BitConverter.ToUInt32(buffer, 0);
            result[i] = chars[(int)(randomValue % chars.Length)];
        }

        return new string(result);
    }
}
