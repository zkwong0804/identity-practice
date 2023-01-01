using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityPractice.Controllers
{
    public class FoodController : Controller
    {
        public IActionResult Index() => View();
        [Authorize]
        public IActionResult Pizza() => View("ViewFood", new Food { Name="Pizza"});
        [Authorize(Roles ="GoodRole")]
        public IActionResult WantonMee() => View("ViewFood", new Food { Name="Wanton mee"});

    }

    public class Food {
        public string Name { get; set; } = string.Empty;
    }
}
