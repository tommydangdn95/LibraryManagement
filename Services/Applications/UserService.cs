using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Consts;
using Services.Dtos;
using Services.Enums;
using Services.Models;
using Services.Models._Users;
using Services.Utils;
using Services.ViewModels._UserViewModels;
using System.Security.Claims;

namespace Services.Applications
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        public UserService(UserManager<AppUser> userManager, AppDbContext appDbContext)
        {
            this._userManager = userManager;
            this._appDbContext = appDbContext;
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

        public async Task<IResultData<UserList>> GetUserList(GetUserList request)
        {
            var query = from user in _appDbContext.Users
                        join userRole in _appDbContext.UserRoles
                           on user.Id equals userRole.UserId
                        join role in _appDbContext.Roles
                            on userRole.RoleId equals role.Id

                        where user.IsActive
                        select new { user, role };

            var count = await query.Select(x => x.user.Id)
                                    .Distinct()
                                    .CountAsync();

            var listUser = await query.Select(x => new UserItem()
            {
                UserId = x.user.Id,
                Email = x.user.Email,
                FullName = x.user.FullName,
                Phone = x.user.PhoneNumber,
                UserName = x.user.UserName,
                Role = x.role.Name
            })
            .Distinct()
            .Skip((request.Page - 1) * request.Page)
            .Take(request.RowsPerPage)
            .ToListAsync();

            var userListView = new UserList()
            {
                Items = listUser,
                Paging = Paging.GetPaging(request.Page, request.RowsPerPage, count)
            };

            return ResultData<UserList>.SuccessData("Get list user successfully", userListView);
        }

        public async Task<IResultData<UserItem>> GetUserViewByIdAsync(Guid userId)
        {
            var query = from user in _appDbContext.Users
                        join userRole in _appDbContext.UserRoles
                           on user.Id equals userRole.UserId
                        join role in _appDbContext.Roles
                            on userRole.RoleId equals role.Id

                        where user.IsActive && user.Id == userId
                        select new { user, role };

            var result = await query.FirstOrDefaultAsync();
            if (result == null)
            {
                return ResultData<UserItem>.Failed("Failed to fetch user");
            }

            var userItem = new UserItem()
            {
                UserId = result.user.Id,
                Email = result.user.Email,
                FullName = result.user.FullName,
                Phone = result.user.PhoneNumber,
                Role = result.role.Name,
                RoleId = result.role.Id,
                UserName = result.user.UserName
            };

            return ResultData<UserItem>.SuccessData("Get user successfully", userItem);
        }
    }
}
