using Microsoft.AspNetCore.Mvc;

namespace Octogami.SixDegreesOfNetflix.Website.Controllers
{
    public class ActorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Degrees(string actorOne, string actorTwo)
        {
            return View();
        }
    }
}