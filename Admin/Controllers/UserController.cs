using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Applications;
using Services.Consts;
using Services.Enums;
using Services.Utils;
using Services.ViewModels._DocumentViewModels;
using Services.ViewModels._UserViewModels;

namespace Admin.Controllers
{
    [Authorize(Roles = RoleConst.Admin)]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("getlist")]
        public async Task<IActionResult> GetList(GetUserList query)
        {
            var result = await _userService.GetUserList(query);
            return PartialView("_ListUser", result.Data);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var resultData = await this._userService.GetUserViewByIdAsync(id);
            if (!resultData.IsSuccess)
            {
                return View("Error");
            }

            var user = resultData.Data;
            var editUser = new EditUser()
            {
                Email = user.Email,
                FullName = user.FullName,
                IsActive = user.IsActive,
                RoleId = user.RoleId,
                UserId = user.UserId,
                RoleList = EnumHelper.ToSelectList<RoleType>()
            };

            return View("Edit", editUser);
        }

        [HttpGet("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser()
        {
            return RedirectToAction("Index");
        }


        [HttpGet("create")]
        public Task<IActionResult> Create()
        {
            var vm = new CreateUser();
            vm.RoleList = EnumHelper.ToSelectList<RoleType>();
            return Task.FromResult<IActionResult>(View(vm));
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserAsync(CreateUser createUser)
        {
            if (createUser == null)
            {
                return View(createUser);
            }

            var result = await _userService.CreateUser(createUser);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, $"{result.Message}");
                return View(createUser);
            }

            return RedirectToAction("Index");
        }
    }
}
