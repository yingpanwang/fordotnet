using IdentityServer4.Models;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForDotNet.Auth
{
    public class MyResourceOwnerValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            await Task.Run(()=> 
            {
                if (DateTime.Now.Minute % 2 == 0)
                {
                    context.Result = new GrantValidationResult(Guid.NewGuid().ToString(), "Customer");
                }
                else
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "认证失败",
                                           new Dictionary<string, object>()
                                       {
                        { "Test","This Is Test" }
                                       });
                }
            });
        }
         
    }
}
