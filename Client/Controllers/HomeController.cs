using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Applications;
using Services.Dtos.Apis;
using Services.Enums;
using Services.Utils;
using Services.ViewModels.Clients._BorrowViewModels;
using Services.ViewModels.Clients._DocumentViewModels;
using System.Security.Claims;

namespace Client.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly IBranchService _branchService;
        private readonly IBorrowService _borrowService;
        public HomeController(IDocumentService documentService, IBranchService branchService, IBorrowService borrowService)
        {
            _documentService = documentService;
            _branchService = branchService;
            _borrowService = borrowService;
        }

        [HttpGet("")]    
        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            var vm = new DocumentListQuery();
            var result = await _branchService.GetListBranchAsync();
            vm.BranchListItem = result.Data.Items.Select(x => new SelectListItem
            {
                Value = x.BranchId.ToString(),
                Text = x.Name
            }).ToList();

            var documentTypes = typeof(DocumentType).ToItemList();
            vm.ListDocumentType = documentTypes;
            vm.ListBorrowStatus.AddRange(typeof(BorrowStatusDisplay).ToItemList());

            return View(vm);
        }

        [HttpPost("GetList")]
        public async Task<IActionResult> GetListAsync([FromBody] GetDocumentListQuery query)
        {
            if (query == null)
            {
                return PartialView("_ListDocument", new DocumentList());
            }

            var result = await _documentService.GetListDocument(query);
            result.Data.Items = result.Data.Items.OrderByDescending(x => x.IsAvaiableBorrowRequest).ToList();
            return PartialView("_ListDocument", result.Data);
        }


        [HttpGet("GetModalBorrowRequest")]
        public async Task<IActionResult> GetModelBorrowRequest(Guid documentId)
        {
            var documentResult = await _documentService.GetDocumentViewItem(documentId);
            if (!documentResult.IsSuccess)
            {
                return PartialView("Error");
            }

            var documentViewItem = documentResult.Data;
            var resultBranch = await _branchService.GetBranchItemByIdAsync(documentResult.Data.BranchId);
            if (resultBranch.IsSuccess)
            {
                documentViewItem.Branch = resultBranch.Data.Name;
                documentViewItem.BranchId = resultBranch.Data.BranchId;
            }

            var borrowRequest = new CreateBorrowRequest()
            {
                DocumentId = documentViewItem.DocumentId,
                BranchId = documentViewItem.BranchId,
                BranchName = documentViewItem.Branch,
                DocumentTitle = documentViewItem.DocumentTitle,
                DocumentType = documentViewItem.DocumentType,
                Note = string.Empty,
                ReturnDate = DateTime.Now
            };

            return PartialView("_BorrowRequestModal", borrowRequest);
        }

        [HttpPost("CreateBorrowRequest")]
        public async Task<IActionResult> CreateBorrowRequest([FromBody] CreateBorrowRequest request)
        {
            var documentResult = await _documentService.GetByIdAsync(request.DocumentId);
            if (!documentResult.IsSuccess)
            {
                return BadRequest(new ApiErrorResponse<string>
                                (
                                    "Could not found document for borrowing",
                                     StatusCodes.Status401Unauthorized
                                ));
            }


            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized(new ApiErrorResponse<string>
                                (
                                    "Could not parse user id",
                                     StatusCodes.Status401Unauthorized
                                ));
            }

            var result = await _borrowService.CreateBorrowRequest(request, userId);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiErrorResponse<string>
                        (
                             result.Message,
                             StatusCodes.Status500InternalServerError
                        ));
            }

            return StatusCode(StatusCodes.Status201Created, new ApiCommandResponse(result.Message, StatusCodes.Status201Created));
        }

    }
}
