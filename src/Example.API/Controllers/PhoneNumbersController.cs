using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Example.Domain.Enums;
using Example.Domain.Interfaces;
using Example.Domain.Models;
using Example.Domain.Resources.PhoneNumbers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Example.API.Controllers
{
    [Authorize]
    [Route("api/{username}/[controller]")]
    public class PhoneNumbersController : JwtController
    {
        private readonly IPhoneNumbersRepository _repository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PhoneNumbersController(
            IPhoneNumbersRepository repository, 
            IAuthorizationService authorizationService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _authorizationService = authorizationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddPhoneNumberResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var phone = _mapper.Map<AddPhoneNumberResource, PhoneNumber>(resource);

            await _repository.AddPhoneNumberAsync(phone);

            if (!(await _authorizationService.AuthorizeAsync(User, phone.Contact, PolicyEnum.ContactOwner.ToString())).Succeeded)
            {
                return Forbid();
            }

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var phone = await _repository.GetPhoneNumberAsync(id);

            if (phone == null)
            {
                return NotFound();
            }

            if (!(await _authorizationService.AuthorizeAsync(User, phone, PolicyEnum.PhoneNumberOwner.ToString())).Succeeded)
            {
                return Forbid();
            }

            _repository.DeletePhoneNumber(phone);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
