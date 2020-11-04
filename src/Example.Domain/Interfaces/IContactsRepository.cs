using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Example.Domain.Models;

namespace Example.Domain.Interfaces
{
    public interface IContactsRepository
    {
        Task AddContactAsync(Contact contact, int userId);
        void DeleteContact(Contact contact);
        Task<Contact> GetContactAsync(int contactId);
        IEnumerable<Contact> GetContacts(int userId, bool includePhones = false);
    }
}
