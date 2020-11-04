using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.Domain.Interfaces;
using Example.Domain.Models;
using Example.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Example.Infrastructure.Repositories
{
    public class ContactsRepository : IContactsRepository
    {
        private readonly ExampleDbContext _context;

        public ContactsRepository(ExampleDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Contact> GetContacts(int userId, bool includePhones = false)
        {
            if (includePhones)
            {
                return _context.Contacts.Include(c => c.PhoneNumbers) 
                    .Where(c => c.UserId == userId);
            }
            
            return _context.Contacts.Where(c => c.UserId == userId);
        }

        public async Task<Contact> GetContactAsync(int contactId)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId);

            return contact;
        }

        public async Task AddContactAsync(Contact contact, int userId)
        {
            contact.UserId = userId;
            await _context.Contacts.AddAsync(contact);
        }

        public void DeleteContact(Contact contact)
        {
            _context.Contacts.Remove(contact);
        }
    }
}
