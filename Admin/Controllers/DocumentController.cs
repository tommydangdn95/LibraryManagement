using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Applications;
using Services.Consts;
using Services.Enums;
using Services.Utils;
using Services.ViewModels._DocumentViewModels;
using Services.ViewModels.Clients._DocumentViewModels;
using System.Security.Claims;

namespace Admin.Controllers
{
    [Authorize(Roles = $"{RoleConst.Admin},{RoleConst.Staff}")]
    [Route("[controller]")]
    public class DocumentController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly IBranchService _branchService;
        public DocumentController(IDocumentService documentService, IBranchService branchService)
        {
            this._documentService = documentService;
            this._branchService = branchService;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpGet("create")]
        public async Task<IActionResult> CreateAsync()
        {
            var vm = new CreateDocument();
            vm.ListDocumentType = EnumHelper.ToSelectList<DocumentType>();
            var listBranches = await _branchService.GetAllBranchAsync();
            vm.ListBranches = listBranches.Data.Select(b => new SelectListItem
            {
                Value = b.BranchId.ToString(),
                Text = b.Name
            }).ToList();

            return View(vm);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDocumentAsync(CreateDocument createDocument)
        {
            if (createDocument == null)
            {
                return View(createDocument);
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized("User not authenticated");
            }

            var result = await _documentService.CreateAsync(createDocument, userId);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, $"{result.Message}");
                return View(createDocument);
            }

            return RedirectToAction("Index");
        }


        [HttpGet("edit/{id}")]
        public async Task<IActionResult> UpdateDocumentAsync(Guid id)
        {
            var vm = new UpdateDocument();
            vm.ListDocumentType = EnumHelper.ToSelectList<DocumentType>();
            var listBranches = await _branchService.GetAllBranchAsync();
            vm.ListBranches = listBranches.Data.Select(b => new SelectListItem
            {
                Value = b.BranchId.ToString(),
                Text = b.Name
            }).ToList();

            var resultData = await _documentService.GetByIdAsync(id);
            if (resultData.IsSuccess)
            {
                vm.Title = resultData.Data.Title;
                vm.Description = resultData.Data.Description;
                vm.PublishDate = resultData.Data.PublishDate;
                vm.
            }

            return View(vm);
        }


        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDocumentAsync(Guid id, UpdateDocument updateDocument)
        {
            if (updateDocument == null)
            {
                return View(updateDocument);
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized("User not authenticated");
            }

            updateDocument.DocumenId = id;
            var result = await _documentService.UpdateAsync(updateDocument, userId);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, $"{result.Message}");
                return View(updateDocument);
            }

            return RedirectToAction("Index");
        }


        [HttpGet("getlist")]
        public async Task<IActionResult> GetList(DocumentListAdminQuery query)
        {
            var result = await _documentService.GetListDocumentAdmin(query);

            return PartialView("_ListDocumentAdmin", result.Data);
        }

    }
}
