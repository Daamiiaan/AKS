using Microsoft.AspNetCore.Identity;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.Web;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        // Utwórz role jeśli nie istnieją
        var roles = new[]
        {
            new Role("Admin", RoleValue.Admin),
            new Role("Teacher", RoleValue.Teacher),
            new Role("Student", RoleValue.Student),
            new Role("Parent", RoleValue.Parent),
            new Role("User", RoleValue.User),
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name!))
            {
                await roleManager.CreateAsync(role);
            }
        }

        // Utwórz konto Admin jeśli nie istnieje
        var adminEmail = "admin@school.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "Admin",
                RegistrationDate = DateTime.Now,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin1234!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
