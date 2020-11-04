using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Example.Domain.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public Contact()
        {
            PhoneNumbers = new Collection<PhoneNumber>();
        }
    }
}
