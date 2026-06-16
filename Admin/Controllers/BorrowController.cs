using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Consts;

namespace Admin.Controllers
{
    [Authorize(Roles = $"{RoleConst.Admin},{RoleConst.Staff}")]
    [Route("[controller]")]
    public class BorrowController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
