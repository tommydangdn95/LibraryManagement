using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Applications;
using Services.Consts;
using Services.ViewModels._BranchViewModels;
using Services.ViewModels._DocumentViewModels;
using System.Security.Claims;

namespace Admin.Controllers
{
    [Authorize(Roles = RoleConst.Admin)]
    [Route("[controller]")]
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;
        public BranchController(IBranchService branchService)
        {
            this._branchService = branchService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("getlist")]
        public async Task<IActionResult> GetList(GetListBranch query)
        {
            var result = await _branchService.GetListBranchAsync(query);
            return PartialView("_ListBranch", result.Data);
        }

        [HttpGet("create")]
        public IActionResult CreateAsync()
        {
            var vm = new CreateBranch();
            return View(vm);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBranchAsync(CreateBranch createBranch)
        {
            if (createBranch == null)
            {
                return View(createBranch);
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized("User not authenticated");
            }

            var result = await _branchService.CreateBranchAsync(createBranch, userId);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, $"{result.Message}");
                return View(createBranch);
            }

            return RedirectToAction("Index");
        }
    }
}
