using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Applications;

namespace Client.Controllers
{
    [Authorize]
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
    }
}
