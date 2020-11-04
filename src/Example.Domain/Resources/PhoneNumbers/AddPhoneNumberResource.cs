using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Example.Domain.Resources.PhoneNumbers
{
    public class AddPhoneNumberResource
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public int ContactId { get; set; }
    }
}
