using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Models;
using GymManagement.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymManagement.DAL.DataSeeding
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedIdentityDataAsync(
            UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager,
            ILogger logger
            , CancellationToken ct = default)
        {
            try
            {
                // Implement your identity data seeding logic here
                var hasUsers = await userManager.Users.AnyAsync();
                var hasRole = await roleManager.Roles.AnyAsync();
                if (hasUsers && hasRole) return;
                if (!hasRole)
                {
                    var roles = new List<IdentityRole>
                {
                    new IdentityRole { Name = "SuperAdmin" },
                    new IdentityRole { Name = "Admin" }
                };
                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role.Name))
                        {
                            var result = await roleManager.CreateAsync(role);
                            if (!result.Succeeded) return;
                        }
                    }
                }

                if (!hasUsers)
                {
                    var superAdmin = new ApplicationUser
                    {
                        FirstName = "Fathy",
                        LastName = "Hosny",
                        UserName = "FathyHosny",
                        Email = "FathyHosny@gmail.com",
                    };

                    await userManager.CreateAsync(superAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");

                    var admin = new ApplicationUser
                    {
                        FirstName = "Ahmed",
                        LastName = "Safe",
                        UserName = "AhmedSafe",
                        Email = "AhmedSafe@gmail.com",
                        PhoneNumber = "01128949255"
                    };

                    await userManager.CreateAsync(admin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }


            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return;
            }




            }
        }
    }

