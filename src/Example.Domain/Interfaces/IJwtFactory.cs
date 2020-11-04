using System;
namespace Example.Domain.Interfaces
{
    public interface IJwtFactory
    {
        string GenerateEncodedToken(int userId);
    }
}
