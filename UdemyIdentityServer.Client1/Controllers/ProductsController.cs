using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UdemyIdentityServer.Client1.Models;
using UdemyIdentityServer.Client1.Services;

namespace UdemyIdentityServer.Client1.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IApiResourceHttpClient _apiResourceHttpClient;

        public ProductsController(IConfiguration configuration, IApiResourceHttpClient apiResourceHttpClient)
        {
            _configuration = configuration;
            _apiResourceHttpClient = apiResourceHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient client = await _apiResourceHttpClient.GetHttpClient();
            List<Product> products = new List<Product>();
            
            #region Client Credentals ile token alma işlemi
            ////Discovery Endpointi içindeki tüm endpointlere ulaşabilmek için kullanılan metod.
            //var discovery = await client.GetDiscoveryDocumentAsync("https://localhost:5001");

            //if (discovery.IsError)
            //{
            //    //Loglama yap
            //}
            //ClientCredentialsTokenRequest request = new ClientCredentialsTokenRequest();
            //request.ClientId = _configuration["Client:ClientId"];
            //request.ClientSecret = _configuration["Client:ClientSecret"];
            //request.Address = discovery.TokenEndpoint;
            ////Client credential == ıd ve secret ile giriş yapma muhabbeti(Hatırla!! Üyelik yoksa böyleydi)
            //var token = await client.RequestClientCredentialsTokenAsync(request);
            #endregion
                        
            var response = await client.GetAsync("https://localhost:5016/api/Products/GetProducts");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                products = JsonConvert.DeserializeObject<List<Product>>(content);
            }
            else
            {
                //loglama yap
            }
            return View(products);
        }
    }
}
