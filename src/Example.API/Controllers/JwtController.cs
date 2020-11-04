using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Example.API.Controllers
{
    [Route("api/[controller]")]
    public class JwtController : Controller
    {
        public JwtController()
        {

        }

        protected int GetUserId()
        {
            var userId = User.FindFirst(ClaimsEnum.userId.ToString());
            if (int.TryParse(userId.Value, out int result))
            {
                return result;
            }
            else
            {
                // just a normal exception for simplicity
                throw new Exception("couldn't get user id");
            }
        }
    }
}
