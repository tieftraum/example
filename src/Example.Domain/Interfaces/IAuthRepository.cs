using System;
using System.Threading.Tasks;
using Example.Domain.Models;
using Example.Domain.Resources.Users;

namespace Example.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> LoginAsync(UserLoginResource credentials);
        Task<User> RegisterAsync(User user, string password);
        Task<bool> UsernameExists(string username);
    }
}
