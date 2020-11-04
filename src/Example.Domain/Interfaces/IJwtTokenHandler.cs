using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Example.Domain.Interfaces
{
    public interface IJwtTokenHandler
    {
        ClaimsPrincipal ValidateToken(string token, TokenValidationParameters tokenValidationParameters);
        string WriteToken(System.IdentityModel.Tokens.Jwt.JwtSecurityToken jwt);
    }
}
