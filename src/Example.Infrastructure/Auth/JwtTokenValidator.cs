using System;
using System.Security.Claims;
using System.Text;
using Example.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Example.Infrastructure.Auth
{
    public class JwtTokenValidator : IJwtTokenValidator
    {
        private readonly IJwtTokenHandler _jwtTokenHandler;

        public JwtTokenValidator(IJwtTokenHandler jwtTokenHandler)
        {
            _jwtTokenHandler = jwtTokenHandler;
        }

        // just for demo
        public ClaimsPrincipal GetPrincipalFromToken(string token, string signInKey)
            => _jwtTokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signInKey)),
                ValidateLifetime = false
            });
    }
}
