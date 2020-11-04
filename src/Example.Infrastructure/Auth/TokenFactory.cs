using System;
using System.Security.Cryptography;
using Example.Domain.Interfaces;

namespace Example.Infrastructure.Auth
{
    public class TokenFactory : ITokenFactory
    {
        public TokenFactory()
        {
        }

        
        public string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
