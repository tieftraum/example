using System;
using System.Security.Claims;

namespace Example.Domain.Interfaces
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signInKey);
    }
}
