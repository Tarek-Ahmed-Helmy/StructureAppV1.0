using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Utilities;

public class RoleSeeder
{
    public static async Task SeedRolesAndAdminUserAsync(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        string adminRoleName = SD.AdminRole;

        var adminRole = await roleManager.FindByNameAsync(adminRoleName);
        if (adminRole == null)
        {
            adminRole = new ApplicationRole { Name = adminRoleName };
            await roleManager.CreateAsync(adminRole);
        }

        var policies = ClaimsStore.GetAllPolicies();
        foreach (var policy in policies)
        {
            var existingClaims = await roleManager.GetClaimsAsync(adminRole);
            if (!existingClaims.Any(c => c.Type == policy))
            {
                await roleManager.AddClaimAsync(adminRole, new Claim(policy, policy));
            }
        }

        var adminSettings = configuration.GetSection("AdminUser").Get<AdminUserSettings>();

        if (adminSettings == null)
        {
            Console.WriteLine("AdminUser settings are missing in appsettings.json!");
            return;
        }

        var adminUser = await userManager.FindByEmailAsync(adminSettings.Email);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminSettings.UserName,
                Email = adminSettings.Email,
                EmailConfirmed = true,
                FirstName = adminSettings.FirstName,
                LastName = adminSettings.LastName,
                Address = adminSettings.Address
            };

            var createResult = await userManager.CreateAsync(adminUser, adminSettings.Password);
            if (createResult.Succeeded)
            {
                Console.WriteLine($"User Created with {adminSettings.Email} and {adminSettings.Password}");
                await userManager.AddToRoleAsync(adminUser, adminRoleName);
            }
            else
            {
                foreach (var error in createResult.Errors)
                {
                    Console.WriteLine($"Error creating admin user: {error.Description}");
                }
            }
        }
    }
}
