using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyIdentityServer.AuthServer.Seed;

namespace UdemyIdentityServer.AuthServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //1 kerelik çalışıp entityleri databaseye kaydedip kapanacak.Scope kullanarak IOC 'den ulaştık.
            using(var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var context = services.GetRequiredService<ConfigurationDbContext>();
                IdentityServerSeedData.Seed(context);
            }
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
