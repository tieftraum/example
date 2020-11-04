using System;
using System.Linq;
using System.Threading.Tasks;
using Example.Domain.Interfaces;
using Example.Domain.Models;
using Example.Domain.Resources.PhoneNumbers;
using Example.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Example.Infrastructure.Repositories
{
    public class PhoneNumbersRepository : IPhoneNumbersRepository
    {
        private readonly ExampleDbContext _context;

        public PhoneNumbersRepository(ExampleDbContext context)
        {
            _context = context;
        }

        public async Task<PhoneNumber> GetPhoneNumberAsync(int id)
        {
            var phone = await _context.PhoneNumbers.Include(pn => pn.Contact)
                .SingleOrDefaultAsync(pn => pn.Id == id);

            return phone;
        }

        public async Task AddPhoneNumberAsync(PhoneNumber phoneNumber)
        {
            await _context.AddAsync(phoneNumber);
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == phoneNumber.ContactId);
            phoneNumber.Contact = contact;
        }

        public void DeletePhoneNumber(PhoneNumber number)
        {
            _context.PhoneNumbers.Remove(number);
        }
    }
}
