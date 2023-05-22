using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UdemyIdentityServer.API2.Models;

namespace UdemyIdentityServer.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetPicture()
        {
            var pictures = new List<Picture>()
            {
                new Picture() { Id = 1,Name="Doğa resmi",Url="dogaresmi.jpg"},
                new Picture() { Id = 2,Name="Filresmi",Url="dogaresmi.jpg"},
                new Picture() { Id = 3,Name="Aslan resmi",Url="dogaresmi.jpg"},
                new Picture() { Id = 4,Name="Fare resmi",Url="dogaresmi.jpg"},
                new Picture() { Id = 5,Name="Kedi resmi",Url="dogaresmi.jpg"},
                new Picture() { Id = 6,Name="Köpek resmi",Url="dogaresmi.jpg"},
            };
            return Ok(pictures);
        }
    }
}
