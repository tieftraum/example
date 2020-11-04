using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Example.Domain.Enums;
using Example.Domain.Interfaces;
using Example.Domain.Models;
using Example.Domain.Resources.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Example.API.Controllers
{
    [Authorize]
    [Route("api/{username}/[controller]")]
    public class ContactsController : JwtController
    {
        private readonly IContactsRepository _repository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactsController(IContactsRepository repository,
            IAuthorizationService authorizationService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _authorizationService = authorizationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<ContactResource> Get()
        {
            int userId = GetUserId();
            var contacts = _repository.GetContacts(userId, true);
            var userContacts = _mapper.Map<IEnumerable<Contact>, IEnumerable<ContactResource>> (contacts);

            return userContacts;
        }

        [HttpGet("{contactId}", Name = "GetContact")]
        public async Task<IActionResult> Get(int contactId)
        {
            var contact = await _repository.GetContactAsync(contactId);

            if (contact == null)
            {
                return NotFound();
            }

            if (!(await _authorizationService.AuthorizeAsync(User, contact, PolicyEnum.ContactOwner.ToString())).Succeeded)
            {
                return Forbid();
            }

            var userContact = _mapper.Map<Contact, ContactResource>(contact);

            return Ok(userContact);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddContactResource contactResource)
        {
            var contact = _mapper.Map<AddContactResource, Contact>(contactResource);
            int userId = GetUserId();
            await _repository.AddContactAsync(contact, userId);
            await _unitOfWork.SaveChangesAsync();

            return Created(Url.RouteUrl(contact.Id), contact);
        }

        [HttpDelete("{contactId}")]
        public async Task<IActionResult> Delete(int contactId)
        {
            var contact = await _repository.GetContactAsync(contactId);

            if(contact == null)
            {
                return NotFound();
            }

            if (!(await _authorizationService.AuthorizeAsync(User, contact, PolicyEnum.ContactOwner.ToString())).Succeeded
                || contact.PhoneNumbers.Count != 0)
            {
                return Forbid();
            }

            _repository.DeleteContact(contact);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
