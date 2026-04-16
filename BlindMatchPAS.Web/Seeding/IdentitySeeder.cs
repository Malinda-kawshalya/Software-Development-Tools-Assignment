using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using BlindMatchPAS.Web.Data;
using BlindMatchPAS.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BlindMatchPAS.Web.Seeding;

public static class IdentitySeeder
{
    private static readonly string[] Roles =
    [
        "Student",
        "Supervisor",
        "ModuleLeader",
        "SystemAdministrator"
    ];

    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var demoPassword = configuration["SeedUsers:DefaultPassword"];
        if (string.IsNullOrWhiteSpace(demoPassword))
        {
            return;
        }

        await EnsureUserInRoleAsync(db, userManager, configuration["SeedUsers:StudentEmail"], demoPassword, "Student");
        await EnsureUserInRoleAsync(db, userManager, configuration["SeedUsers:SupervisorEmail"], demoPassword, "Supervisor");
        await EnsureUserInRoleAsync(db, userManager, configuration["SeedUsers:ModuleLeaderEmail"], demoPassword, "ModuleLeader");
        await EnsureUserInRoleAsync(db, userManager, configuration["SeedUsers:SystemAdminEmail"], demoPassword, "SystemAdministrator");
    }

    private static async Task EnsureUserInRoleAsync(
        ApplicationDbContext db,
        UserManager<IdentityUser> userManager,
        string? email,
        string password,
        string role)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                return;
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }

        if (role == "Student")
        {
            var hasProfile = await db.StudentProfiles.AnyAsync(p => p.UserId == user.Id);
            if (!hasProfile)
            {
                db.StudentProfiles.Add(new StudentProfile
                {
                    UserId = user.Id,
                    CreatedAtUtc = DateTime.UtcNow
                });
            }
        }

        if (role == "Supervisor")
        {
            var hasProfile = await db.SupervisorProfiles.AnyAsync(p => p.UserId == user.Id);
            if (!hasProfile)
            {
                db.SupervisorProfiles.Add(new SupervisorProfile
                {
                    UserId = user.Id,
                    CreatedAtUtc = DateTime.UtcNow
                });
            }
        }

        await db.SaveChangesAsync();
    }
}
