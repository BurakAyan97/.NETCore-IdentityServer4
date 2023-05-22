using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UdemyIdentityServer.Client1.Models;

namespace UdemyIdentityServer.Client1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = new List<Product>();
            HttpClient client = new HttpClient();
            //Discovery Endpointi içindeki tüm endpointlere ulaşabilmek için kullanılan metod.
            var discovery = await client.GetDiscoveryDocumentAsync("https://localhost:5001");

            if (discovery.IsError)
            {
                //Loglama yap
            }
            ClientCredentialsTokenRequest request = new ClientCredentialsTokenRequest();

            request.ClientId = _configuration["Client:ClientId"];
            request.ClientSecret = _configuration["Client:ClientSecret"];
            request.Address = discovery.TokenEndpoint;
            //Client credential == ıd ve secret ile giriş yapma muhabbeti(Hatırla!! Üyelik yoksa böyleydi)
            var token = await client.RequestClientCredentialsTokenAsync(request);

            if (token.IsError)
            {
                //loglama yap
            }

            //Client'a tokenımızı verdik => (Bearer {token})
            client.SetBearerToken(token.AccessToken);

            var response = await client.GetAsync("https://localhost:5016/api/products/getproducts");

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
