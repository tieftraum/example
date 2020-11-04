using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Domain.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Ip { get; set; }
        public string Token { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ValidTill { get; set; }
    }
}
