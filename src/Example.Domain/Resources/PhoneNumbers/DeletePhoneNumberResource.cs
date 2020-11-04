using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Example.Domain.Resources.PhoneNumbers
{
    public class DeletePhoneNumberResource
    {
        [Required]
        public int ContactId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
