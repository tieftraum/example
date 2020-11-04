using Example.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Example.Domain.Interfaces
{
    public interface IPhoneNumbersRepository
    {
        Task AddPhoneNumberAsync(PhoneNumber phoneNumber);
        void DeletePhoneNumber(PhoneNumber number);
        Task<PhoneNumber> GetPhoneNumberAsync(int id);
    }
}
