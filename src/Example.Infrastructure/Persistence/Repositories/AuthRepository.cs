using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Example.Domain.Interfaces;
using Example.Domain.Models;
using Example.Domain.Resources.Users;
using Example.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Example.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ExampleDbContext _context;
        private readonly IHashHelper _hashHelper;

        public AuthRepository(ExampleDbContext context, IHashHelper hashHelper)
        {
            _context = context;
            _hashHelper = hashHelper;
        }
        public async Task<User> LoginAsync(UserLoginResource credentials)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == credentials.Username.ToLower());

            if (user == null || !_hashHelper.VerifyPasswordHash(credentials.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }
        public async Task<User> RegisterAsync(User user, string password)
        {
            _hashHelper.CreatePasswordHash(password, out byte[] hash, out byte[] salt);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username);
        }
    }
}
