// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Softeq.NetKit.Auth.IdentityServer
{
    public static class IdentityServerSeedData
    {
        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                await EnsureSeedData(context);
            }
        }

        private static async Task EnsureSeedData(IConfigurationDbContext context)
        {
            Console.WriteLine("Seeding database...");
            if (!await context.Clients.AnyAsync())
            {
                Console.WriteLine("Clients being populated");
                foreach (var client in Config.GetClients().ToList())
                {
                    await context.Clients.AddAsync(client.ToEntity());
                }

                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Clients already populated");
            }

            if (!await context.IdentityResources.AnyAsync())
            {
                Console.WriteLine("IdentityResources being populated");
                foreach (var resource in Config.GetIdentityResources().ToList())
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("IdentityResources already populated");
            }

            if (!await context.ApiResources.AnyAsync())
            {
                Console.WriteLine("ApiResources being populated");
                foreach (var resource in Config.GetApiResources().ToList())
                {
                    await context.ApiResources.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("ApiResources already populated");
            }
            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }
    }
}