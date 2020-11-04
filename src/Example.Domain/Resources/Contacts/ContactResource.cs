using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Example.Domain.Resources.Contacts
{
    public class ContactResource
    {
        public int Id { get; set; }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public ICollection<string> PhoneNumbers { get; set; }

        public ContactResource()
        {
            PhoneNumbers = new Collection<string>();
        }
    }
}
