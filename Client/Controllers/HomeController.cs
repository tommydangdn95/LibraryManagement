using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Applications;
using Services.Dtos.Apis;
using Services.ViewModels.Clients._DocumentViewModels;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = new DocumentListQuery();
            var result = await _branchService.GetAllBranchAsync();


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
