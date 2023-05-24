using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UdemyIdentityServer.AuthServer.Models;
using UdemyIdentityServer.AuthServer.Repository;
using UdemyIdentityServer.AuthServer.Services;

namespace UdemyIdentityServer.AuthServer
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
            services.AddScoped<ICustomUserRepository, CustomUserRepository>();

            services.AddDbContext<CustomDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("LocalDB")));

            var assemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddConfigurationStore(opts =>
                {
                    //Client resource ve scopelar databaseye kaydedilcek. Identiyserver kendi halledicek bu db özelliği ile.
                    opts.ConfigureDbContext = c => c.UseSqlServer(Configuration.GetConnectionString("LocalDB"), sqlopts => sqlopts.MigrationsAssembly(assemblyName));
                })
                .AddOperationalStore(opts =>
                {
                    //refresh token ve authorize code databaseye kaydedilcek. Identiyserver kendi halledicek bu db özelliği ile.
                    opts.ConfigureDbContext = c => c.UseSqlServer(Configuration.GetConnectionString("LocalDB"), sqlopts => sqlopts.MigrationsAssembly(assemblyName));
                })
                                //Databaseye verileri gönderdikten sonra aşağıdaki metotlara gerek kalmadı. Aşağıdaki metotlar memoryde tutuyordu dataları restart atınca değişiyordu.---------------------------------------------------
                                /*.AddInMemoryApiResources(Config.GetApiResources())
                                .AddInMemoryApiScopes(Config.GetApiScopes())
                                .AddInMemoryClients(Config.GetClients())
                                // Development esnasında Public ve private key oluşturur JWT token için.
                                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                                //.AddTestUsers(Config.GetUsers().ToList())//Databasede gerçek üyeler var diye yoruma aldım.
                                --------------------------------------------------------------------------------------------------*/
                                .AddDeveloperSigningCredential()
                //Claimler bu sayede ekleniyor Databasedeki datalara.
                .AddProfileService<CustomProfileService>()
                //BU akış tipiyle istek yapıldığı zaman metod çalışacak ve ona göre token dönecek veya dönmeyecek
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();


            //.AddSigningCredential(); //Publish ederken bu kodu kullanıcaz.Azurede tutucaz bilgileri.

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

            app.UseIdentityServer();

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
