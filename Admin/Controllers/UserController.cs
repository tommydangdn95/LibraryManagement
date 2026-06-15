using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Applications;
using Services.Consts;
using Services.Enums;
using Services.Utils;
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
