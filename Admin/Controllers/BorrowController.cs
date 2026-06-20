using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Applications;
using Services.Consts;
using Services.ViewModels._BorrowViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Admin.Controllers
{
    [Authorize(Roles = $"{RoleConst.Admin},{RoleConst.Staff}")]
    [Route("[controller]")]
    public class BorrowController : Controller
    {
        private readonly IBorrowService _borrowService;
        public BorrowController(IBorrowService borrowService)
        {
            this._borrowService = borrowService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetListAsync(GetListBorrowRequest request)
        {
            var result = await _borrowService.GetBorrowRequestList(request);
            return PartialView("_ListBorrowRequest", result.Data);
        }
    }
}
