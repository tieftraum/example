using System;

namespace Example.Domain.Models
{
    public class PhoneNumber
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}
