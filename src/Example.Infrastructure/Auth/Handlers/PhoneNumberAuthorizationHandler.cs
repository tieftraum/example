using Example.Domain.Enums;
using Example.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Example.Infrastructure.Auth
{
    public class PhoneNumberAuthorizationHandler : AuthorizationHandler<PhoneNumberBelongsToUserRequirement, PhoneNumber>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            PhoneNumberBelongsToUserRequirement requirement, 
            PhoneNumber resource)
        {
            if (context.User.FindFirst(ClaimsEnum.userId.ToString()).Value == resource.Contact.UserId.ToString())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class PhoneNumberBelongsToUserRequirement : IAuthorizationRequirement { }
}
