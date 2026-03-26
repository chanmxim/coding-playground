using assignment01.Areas.Identity.Models;
using assignment01.Enum;
using Microsoft.AspNetCore.Identity;

namespace assignment01.Data;

public class ContextSeed
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        List<Roles> rolesList = Enum.Roles.GetValues(typeof(Roles))
            .Cast<Roles>()
            .ToList();
        foreach (var role in rolesList)
        {
            if (!await roleManager.RoleExistsAsync(role.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }
        }
    }

    public static async Task SeedSuperAdminUser(RoleManager<IdentityRole> roleManager,
        UserManager<User> userManager)
    {
        var email = "superadmin@gmail.com";
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = "Super",
                LastName = "Admin",
                PhoneNumber = "123456789",
                Address = "123 Main Street",
            };
            var res = await userManager.CreateAsync(user, "123456");
            if (res.Succeeded)
            {
                List<Roles> rolesList = Enum.Roles.GetValues(typeof(Roles))
                    .Cast<Roles>()
                    .ToList();
                foreach (var role in rolesList)
                {
                    await userManager.AddToRoleAsync(user, role.ToString());
                }
            }
        }
    }
}