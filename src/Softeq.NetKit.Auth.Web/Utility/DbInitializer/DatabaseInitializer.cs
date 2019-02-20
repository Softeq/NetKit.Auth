// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Softeq.NetKit.Auth.Domain.Models.Role;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace Softeq.NetKit.Auth.Web.Utility.DbInitializer
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IServiceProvider _serviceProvider;

        public DatabaseInitializer(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _serviceProvider = serviceProvider;
        }

        public async Task SeedAsync()
        {
            await IdentityServerSeedData.EnsureSeedData(_serviceProvider);

            if (File.Exists("SeedValues.json"))
            {
                using (var reader = File.OpenText("SeedValues.json"))
                {
                    var values = JToken.Parse(reader.ReadToEnd());
                    await SeedRoles(values["Roles"].Values<string>());
                    await SeedUsers(values["Users"]);
                }
            }
        }

        private async Task SeedRoles(IEnumerable<string> roleNames)
        {
            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    await _roleManager.CreateAsync(new Role(roleName));
                }
            }
        }

        private async Task SeedUsers(JToken users)
        {
            // TODO: use foreach below only for dev environment
            foreach (var data in users)
            {
                var id = data["Id"].Value<string>();
                var email = data["Email"].Value<string>();
                var role = data["Role"].Value<string>();
                var password = data["Password"].Value<string>();
                var status = data["Status"].Value<int>();

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var newUser = new User
                    {
                        Id = id,
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        StatusId = status
                    };
                    await _userManager.CreateAsync(newUser, password);
                    await _userManager.AddToRoleAsync(newUser, role);
                }
            }
        }
    }
}
