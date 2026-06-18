using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Applications;
using Services.Enums;
using Services.Utils;
using Services.ViewModels._DocumentViewModels;
using Services.ViewModels.Clients._DocumentViewModels;
using System.Diagnostics;

namespace Client.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly IBranchService _branchService;
        public HomeController(IDocumentService documentService, IBranchService branchService)
        {
            _documentService = documentService;
            _branchService = branchService;
        }

        [HttpGet("")]    
        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            var vm = new DocumentListQuery();
            var result = await _branchService.GetAllBranchAsync();
            vm.BranchListItem = result.Data.Select(x => new SelectListItem
            {
                Value = x.BranchId.ToString(),
                Text = x.Name
            }).ToList();

            var documentTypes = typeof(DocumentType).ToItemList();
            vm.ListDocumentType = documentTypes;
            vm.ListBorrowStatus.AddRange(typeof(BorrowStatusDisplay).ToItemList());

            return View(vm);
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetListAsync([FromBody] DocumentListQuery query)
        {
            var result = await _documentService.GetListDocument(query);
            return PartialView("_ListDocument", result.Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
