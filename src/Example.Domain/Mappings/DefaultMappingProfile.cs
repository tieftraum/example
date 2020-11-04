using System;
using System.Linq;
using AutoMapper;
using Example.Domain.Models;
using Example.Domain.Resources.Contacts;
using Example.Domain.Resources.PhoneNumbers;
using Example.Domain.Resources.Users;

namespace Example.Domain.Mappings
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            // Domain to API resources
            CreateMap<Contact, ContactResource>()
                .AfterMap((c, cr) =>
                {
                    var phoneNumbers = c.PhoneNumbers.Select(pn => pn.Phone); // SelectMany(pn => pn.Phone);

                    foreach (var phoneNumber in phoneNumbers)
                    {
                        cr.PhoneNumbers.Add(phoneNumber);
                    }
                });

            // API resources to Domain
            CreateMap<UserRegisterResource, User>();
            CreateMap<AddContactResource, Contact>();
            CreateMap<AddPhoneNumberResource, PhoneNumber>();
        }
    }
}