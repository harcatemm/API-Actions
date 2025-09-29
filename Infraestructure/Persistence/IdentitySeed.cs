using Infraestructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Persistence
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(IServiceProvider provider)
        {
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();

            // Roles iniciales
            string[] roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            List<UserInfo> users = new List<UserInfo>()
            {
                new UserInfo { FullName = "User admin", UserName = "admin@app.com", Password = "Pass123$", Role = "Admin" },
                new UserInfo { FullName = "User default", UserName = "default@app.com", Password = "Pass123$", Role = "User" },
            };
            foreach (var item in users)
            {
                var user = await userManager.FindByEmailAsync(item.UserName);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = item.UserName,
                        Email = item.UserName,
                        FullName = item.FullName,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, item.Password); 

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, item.Role);
                    }
                }

            }            
        }
    }
}

public class UserInfo
{
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}

