using Microsoft.AspNetCore.Mvc;

namespace IdentityPractice.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
