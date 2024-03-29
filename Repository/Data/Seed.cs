using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Model.enums;

namespace Repository.data;
public class Seed
{
    public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        var userData = await System.IO.File.ReadAllTextAsync("../Repository/Data/userSeedData.json");
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
        if (users == null) return;

        foreach (var role in AppRoleEnum.GetNames<AppRoleEnum>().Select(role => new AppRole(role)).ToList())
        {
            await roleManager.CreateAsync(role);
        }


        var memberRole = AppRoleEnum.GetName<AppRoleEnum>(AppRoleEnum.Member);
        var modRole = AppRoleEnum.GetName<AppRoleEnum>(AppRoleEnum.Mod);
        var adminRole = AppRoleEnum.GetName<AppRoleEnum>(AppRoleEnum.Admin);

        foreach (var user in users)
        {
            user.UserName = user.UserName.ToLower();
            // await userManager.CreateAsync(user, "P@ss10");
            // await userManager.AddToRoleAsync(user, memberRole);
        }

        var admin = new AppUser
        {
            UserName = "admin"
        };
        await userManager.CreateAsync(admin, "P@ss10");
        await userManager.AddToRolesAsync(admin, (new[] { memberRole, adminRole }));


    }
}