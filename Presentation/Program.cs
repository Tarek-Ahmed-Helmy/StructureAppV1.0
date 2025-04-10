using Domain.Models;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Presentation.Mappers;
using Service.DependencyInjection;
using Utilities;

namespace Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            builder.Services.AddAuthorization(options =>
            {
                foreach (var controller in ClaimsStore.ControllerClaims)
                {
                    foreach (var action in controller.Value)
                    {
                        var policyName = $"{controller.Key}.{action}";
                        options.AddPolicy(policyName, policy => policy.RequireClaim(policyName));
                    }
                }
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var configuration = services.GetRequiredService<IConfiguration>();

                await RoleSeeder.SeedRolesAndAdminUserAsync(roleManager, userManager, configuration);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
            }
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
