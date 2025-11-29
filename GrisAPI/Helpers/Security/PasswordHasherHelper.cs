using System.Security.Cryptography;

namespace GrisAPI.Helpers.Security;

public static class PasswordHasherHelper
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int HashIterations = 600000;
    
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

    public static string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);

        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            HashIterations,
            HashAlgorithm,
            KeySize
        );
        
        return $"{HashIterations}.{HashAlgorithm.Name}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }
    
    public static bool VerifyPassword(
        string password,
        string hashedPassword)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);
        ArgumentException.ThrowIfNullOrEmpty(hashedPassword);
        
        try
        {
            var parts = hashedPassword.Split('.');

            var iterations = int.Parse(parts[0]);
            var algorithmName = new HashAlgorithmName(parts[1]);
            var salt = Convert.FromBase64String(parts[2]);
            var storedHash = Convert.FromBase64String(parts[3]);
             
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                algorithmName,
                storedHash.Length
            );

            return CryptographicOperations.FixedTimeEquals(hash, storedHash);
        }
        catch
        {
            return false;
        }
    }
}