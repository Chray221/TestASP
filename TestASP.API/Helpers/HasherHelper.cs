using System;
using Microsoft.AspNetCore.Identity;

namespace TestASP.API.Helpers
{
	public static class HasherHelper
	{
		public static string HashPassword<T>(this T user, string password) where T : class
		{
			var passwordHasher = new PasswordHasher<T>();
            return passwordHasher.HashPassword(user, password);
        }

        public static PasswordVerificationResult GetVerifyPassword<T>(this T user, string password, string hashedPassword) where T : class
        {
            var passwordHasher = new PasswordHasher<T>();
            return passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
        }

        public static bool VerifyPassword<T>(this T user, string password, string hashedPassword) where T : class
        {
            var verifyPassword = GetVerifyPassword(user, password, hashedPassword);
            return verifyPassword == PasswordVerificationResult.Success ||
                   verifyPassword == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}

