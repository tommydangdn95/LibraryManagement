using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Applications;
using Services.Dtos;
using Services.Dtos.Apis;
using Services.ViewModels.Clients._BorrowViewModels;
using System.Security.Claims;

namespace Client.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class BorrowController : Controller
    {
        private readonly IBorrowService _borrowService;
        public BorrowController(IBorrowService borrowService)
        {
            this._borrowService = borrowService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _borrowService.GetBorrowStatusCount();
            return View(result.Data);
        }

        [HttpPost("GetListBorrow")]
        public async Task<IActionResult> GetListAsync([FromBody] GetListDocumentBorrow query)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return PartialView("Error");
            }

            var pageResult = await _borrowService.GetBorrowDocumentByUser(query, userId);
            if (!pageResult.IsSuccess)
            {
                return PartialView("Error");
            }

            return PartialView("_ListDocumentBorrow", pageResult.Data);
        }
    }
}
