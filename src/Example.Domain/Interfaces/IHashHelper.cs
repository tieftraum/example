using System;
namespace Example.Domain.Interfaces
{
    public interface IHashHelper
    {
        void CreatePasswordHash(string Password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
