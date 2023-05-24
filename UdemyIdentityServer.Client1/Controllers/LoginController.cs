using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using UdemyIdentityServer.Client1.Models;

namespace UdemyIdentityServer.Client1.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            var client = new HttpClient();
            var discovery = await client.GetDiscoveryDocumentAsync(_configuration["AuthServerUrl"]);

            if (discovery.IsError)
            {
                //hata yakala ve logla
            }

            PasswordTokenRequest password = new PasswordTokenRequest();
            password.Address = discovery.TokenEndpoint;
            password.UserName = loginViewModel.Email;
            password.Password = loginViewModel.Password;
            password.ClientId = _configuration["ClientResourceOwner:ClientId"];
            password.ClientSecret = _configuration["ClientResourceOwner:ClientSecret"];

            var token = await client.RequestPasswordTokenAsync(password);

            if (token.IsError)
            {
                //hata yakala ve logla
            }

            UserInfoRequest userInfoRequest = new UserInfoRequest();
            userInfoRequest.Token = token.AccessToken;
            userInfoRequest.Address = discovery.UserInfoEndpoint;

            var userInfo = await client.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
            {
                //hata yakala ve logla
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, "Cookies","name","role");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            AuthenticationProperties authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            });

            await HttpContext.SignInAsync("Cookies", claimsPrincipal, authenticationProperties);

            return RedirectToAction("Index", "User");
        }
    }
}
