using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyIdentityServer.Client1.Services;

namespace UdemyIdentityServer.Client1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IApiResourceHttpClient, ApiResourceHttpClient>();

            //Auth server ile haberleşmemiz için yazılan client kodu
            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = "Cookies";
                opts.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies", opts =>
            {
                opts.AccessDeniedPath = "/Home/AccessDenied";
            }).AddOpenIdConnect("oidc", opts =>
            {
                opts.SignInScheme = "Cookies";
                opts.Authority = "https://localhost:5001";//Token dağıtan yetkili yer.
                opts.ClientId = "Client1-Mvc";
                opts.ClientSecret = "secret";
                //Auth kodu(string) ve içinde sadece id olan token doğrulama için bir JWT token alıyoruz.
                opts.ResponseType = "code id_token";
                opts.GetClaimsFromUserInfoEndpoint = true;//Userin claimdeki tüm bilgilerini gösterir.Normalde şişirmemek için göstermez
                opts.SaveTokens = true;//Token varsa kaydedilecek demek.
                opts.Scope.Add("api1.read");//İzin verilen scoplara bakar.ORda da yazılıysa izin verir.
                opts.Scope.Add("offline_access");//Access tokenın ömrü bittiğinde yenisinin yapılmasını sağlar refresh token sayesinde.AuthServerda yazılması lazım tabi.
                opts.Scope.Add("CountryAndCity");
                opts.Scope.Add("Roles");
                //Custom claim yaptığımız için maplememiz lazım.Identityserver tanımıyor yoksa.
                opts.ClaimActions.MapUniqueJsonKey("country", "country");
                opts.ClaimActions.MapUniqueJsonKey("city", "city");
                opts.ClaimActions.MapUniqueJsonKey("role", "role");

                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "role" //Rol bazlı yetkilendirme varsa bu arkadaş bulup doğruluyor
                };
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
