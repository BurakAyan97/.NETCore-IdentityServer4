using IdentityModel;
using IdentityServer4.Validation;
using System.Threading.Tasks;
using UdemyIdentityServer.AuthServer.Models;
using UdemyIdentityServer.AuthServer.Repository;

namespace UdemyIdentityServer.AuthServer.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ICustomUserRepository customUserRepository;

        public ResourceOwnerPasswordValidator(ICustomUserRepository customUserRepository)
        {
            this.customUserRepository = customUserRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var isUser = await customUserRepository.Validate(context.UserName, context.Password);
            if (isUser)
            {
                var user = await customUserRepository.FindByEmail(context.UserName);
                context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password);//Akış tipi resource owner cred
            }
        }
    }
}
