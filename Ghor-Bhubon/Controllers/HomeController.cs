using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult TenantDashboard()
    {
        return View();
    }

    public IActionResult LandlordDashboard()
    {
        return View();
    }

    public IActionResult AdminDashboard()
    {
        return View();
    }
}
