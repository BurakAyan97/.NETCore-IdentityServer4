using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using UdemyIdentityServer.AuthServer.Repository;

namespace UdemyIdentityServer.AuthServer.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly ICustomUserRepository _userRepository;

        public CustomProfileService(ICustomUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subId = context.Subject.GetSubjectId();

            var user = await _userRepository.FindById(int.Parse(subId));

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("name",user.UserName),//ClaimTypes.Name yazınca görmüyor Dikkat!!
                new Claim("city", user.City),
            };

            if (user.Id == 1)
                claims.Add(new Claim("role", "admin"));
            else
                claims.Add(new Claim("role", "customer"));
            
            context.AddRequestedClaims(claims);//Userinfo endpointinden alır claimi.Configde izin verilenleri alır sadece

            /*context.IssuedClaims = claims;*/ //JWT token içerisine yazar bilgileri ama uygun değildir.
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = context.Subject.GetSubjectId();

            var user = await _userRepository.FindById(int.Parse(userId));

            context.IsActive = user != null ? true : false;
        }
    }
}
