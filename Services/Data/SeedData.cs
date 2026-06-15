using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Consts;
using Services.Models;
using Services.Models._Users;
using Services.Utils;
using System.Security.Claims;

namespace Services.Data
{
    public class SeedData
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly AppDbContext _dbContext;
        public SeedData(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = context;
        }
        public async Task SeedingDataAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();
            await SeedingRoleDataAsync();
            await SeedingUserDataAsync();
        }


        private async Task SeedingRoleDataAsync()
        {
            if (!await _dbContext.Roles.AnyAsync())
            {
                var roleNames = new string[] { RoleConst.User, RoleConst.Admin, RoleConst.Staff };
                foreach (var role in roleNames)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        var appRole = new AppRole()
                        {
                            Name = role
                        };

                        await _roleManager.CreateAsync(appRole);
                    }
                }
            }
        }

        private async Task SeedingUserDataAsync()
        {
            if (!await _dbContext.Users.AnyAsync())
            {
                var admin = await _userManager.FindByEmailAsync("admin@newspaper.com");
                if (admin == null)
                {
                    admin = new AppUser()
                    {
                        FullName = "Admin Newspaper",
                        UserName = "admin@newspaper.com",
                        Email = "admin@newspaper.com",
                        EmailConfirmed = true,
                        Department = "Admin",
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        PhoneNumber = "+84901234567"
                    };

                    var result = await _userManager.CreateAsync(admin, "Admin@123456");
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.ToError());
                    }

                    var claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Name, RoleConst.Admin.ToString()),
                        new Claim(ClaimTypes.Role, RoleConst.Admin)
                    };

                    result = await _userManager.AddClaimsAsync(admin, claims);

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.ToError());
                    }

                    result = await _userManager.AddToRoleAsync(admin, RoleConst.Admin);

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.ToError());
                    }
                }
            }
        }
    }
}
