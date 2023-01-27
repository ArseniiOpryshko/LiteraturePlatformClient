using Microsoft.AspNetCore.Mvc;

namespace LiteraturePlatformClient.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
