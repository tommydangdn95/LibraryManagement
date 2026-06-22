using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Consts;
using Services.Dtos;
using Services.Enums;
using Services.Models._Users;
using Services.Utils;
using Services.ViewModels._UserViewModels;
using System.Security.Claims;

namespace Services.Applications
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        public UserService(UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<AppUser> GetUserById(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user;
        }

        public async Task<IResultData<Guid>> CreateUser(CreateUser createUser)
        {
            var appUser = new AppUser()
            {
                FullName = createUser.FullName,
                UserName = createUser.Email,
                Email = createUser.Email,
                EmailConfirmed = true,
                IsActive = true,
                CreatedDate = DateTime.Now,
            };

            var parseRole = createUser.RoleId.ToEnum<RoleType>();
            var role = RoleConst.GetRoleName(parseRole!.Value);

            var result = await _userManager.CreateAsync(appUser, createUser.Password);
            if (!result.Succeeded)
            {
                return ResultData<Guid>.Failed($"Create user failed: {result.Errors.ToError()}");
            }

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, role.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            result = await _userManager.AddClaimsAsync(appUser, claims);

            if (!result.Succeeded)
            {
                return ResultData<Guid>.Failed($"Create user failed: {result.Errors.ToError()}");
            }

            result = await _userManager.AddToRoleAsync(appUser, role);

            if (!result.Succeeded)
            {
                return ResultData<Guid>.Failed($"Create user failed: {result.Errors.ToError()}");
            }

            return ResultData<Guid>.SuccessData("Create user successfully", appUser.Id);
        }

        public async Task<IResultData<AppUser>> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ResultData<AppUser>.Failed($"Failed to fetch user");
            }

            return ResultData<AppUser>.SuccessData("Get user successfully", user);
        }
    }
}
