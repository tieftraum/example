using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Example.Domain.Interfaces;
using Example.Domain.Models;
using Example.Domain.Resources.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Example.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repository;
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repository, IJwtFactory jwtFactory, ITokenFactory tokenFactory, IMapper mapper)
        {
            _repository = repository;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginResource credentials)
        {
            var user = await _repository.LoginAsync(credentials);

            if (user == null)
            {
                return Unauthorized();
            }

            var token = _jwtFactory.GenerateEncodedToken(user.Id);
            var refreshToken = _tokenFactory.GenerateToken();
            // TODO: Save refresh token in db for renew 

            return Ok(new { token, refreshToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserRegisterResource registerResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _repository.UsernameExists(registerResource.Username))
            {
                return BadRequest("username exists");
            }

            var user = _mapper.Map<UserRegisterResource, User>(registerResource);
            await _repository.RegisterAsync(user, registerResource.Password);

            return Ok();
        }

        // TODO: add refresh token functionality
        //[HttpPost("refreshtoken")]
        //public async Task<IActionResult> RefreshToken()
        //{

        //}
    }
}
