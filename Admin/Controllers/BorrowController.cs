using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Applications;
using Services.Consts;
using Services.Dtos.Apis;
using Services.ViewModels._BorrowViewModels;
using System.Security.Claims;

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
            result.Data.BorrowStatus = request.BorrowStatus;
            return PartialView("_ListBorrowRequest", result.Data);
        }

        [HttpGet("Details")]
        public async Task<IActionResult> GetBorrowDetail(Guid borrowDetailId)
        {
            var resultItem = await _borrowService.GetDetailBorrowRequestItem(borrowDetailId);
            if (!resultItem.IsSuccess)
            {
                return PartialView("Error");
            }
            return PartialView("_BorrowRequestModalDetail", resultItem.Data);
        }

        [HttpPost("UpdateStatus")]
        public async Task<IActionResult> UpdateStatusAsync([FromBody] UpdateBorrowRequest request) 
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized(new ApiErrorResponse<string>
                                (
                                    "Could not parse user id",
                                     StatusCodes.Status401Unauthorized
                                ));
            }

            request.SubmitedUserId = userId;
            var result = await _borrowService.UpdateBrrowStatus(request);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiErrorResponse<string>
                                (
                                     result.Message,
                                     StatusCodes.Status500InternalServerError
                                ));
            }
            return StatusCode(StatusCodes.Status200OK, new ApiCommandResponse(result.Message, StatusCodes.Status200OK));

        }
    }
}
