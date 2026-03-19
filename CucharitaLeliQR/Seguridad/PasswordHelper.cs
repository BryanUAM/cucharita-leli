using Microsoft.AspNetCore.Identity;

namespace CucharitaLeliQR.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>();
            return hasher.HashPassword(null, password);
        }

        public static bool VerifyPassword(string hashedPassword, string password)
        {
            var hasher = new PasswordHasher<object>();
            var result = hasher.VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}