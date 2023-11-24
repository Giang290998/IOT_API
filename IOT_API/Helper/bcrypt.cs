using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace IOT_API.Helper;

public class Bcrypt
{
    public static string HashPassword(string password)
    {
        // Generate a random salt
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Hash the password with the salt
        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8
        ));

        // Combine salt and hashed password for storage
        string hashedPasswordWithSalt = $"{Convert.ToBase64String(salt)}:{hashedPassword}";

        return hashedPasswordWithSalt;
    }

    public static bool VerifyPassword(string hashedPasswordWithSalt, string password)
    {
        // Extract salt and hashed password from the stored value
        var parts = hashedPasswordWithSalt.Split(':');
        var salt = Convert.FromBase64String(parts[0]);
        var storedHashedPassword = parts[1];

        // Hash the provided password with the stored salt
        var hashedPasswordToVerify = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8
        ));

        // Compare the two hashed passwords
        return storedHashedPassword.Equals(hashedPasswordToVerify);
    }
}
