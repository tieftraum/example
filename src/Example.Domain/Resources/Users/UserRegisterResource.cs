using System;
using System.ComponentModel.DataAnnotations;

namespace Example.Domain.Resources.Users
{
    public class UserRegisterResource
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
