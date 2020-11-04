using Example.Domain.Enums;
using Example.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Example.Infrastructure.Auth
{
    public class ContactAuthorizationHandler : AuthorizationHandler<ContactBelongsToUserRequirement, Contact>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            ContactBelongsToUserRequirement requirement, 
            Contact resource)
        {
            if (context.User.FindFirst(ClaimsEnum.userId.ToString()).Value == resource.UserId.ToString())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class ContactBelongsToUserRequirement : IAuthorizationRequirement { }
}
