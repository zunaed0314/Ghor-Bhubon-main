using Microsoft.AspNetCore.Mvc;

namespace Ghor_Bhubon.Controllers
{
    public class AccountController : Controller
    {
        // Login page
        public IActionResult Login()
        {
            return View();
        }

        // Register page
        public IActionResult Register()
        {
            return View();
        }
    }
}
