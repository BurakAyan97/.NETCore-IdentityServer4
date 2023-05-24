using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore.Internal;

namespace UdemyIdentityServer.AuthServer.Seed
{
    public static class IdentityServerSeedData
    {
        public static void Seed(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Config.GetClients())
                {
                    context.Clients.Add(client.ToEntity());
                }
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.GetApiResources())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
            }

            if (!context.ApiScopes.Any())
            {
                foreach(var scope in Config.GetApiScopes())
                {
                    context.ApiScopes.Add(scope.ToEntity());
                }
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var item in Config.GetIdentityResources())
                {
                    context.IdentityResources.Add(item.ToEntity());
                }
            }

            context.SaveChanges();
        }
    }
}
