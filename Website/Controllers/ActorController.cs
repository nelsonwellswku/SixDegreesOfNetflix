using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Octogami.SixDegreesOfNetflix.Application.Feature;
using Octogami.SixDegreesOfNetflix.Website.Models;

namespace Octogami.SixDegreesOfNetflix.Website.Controllers
{
    public class ActorController : Controller
    {
        private readonly IMediator _mediator;

        public ActorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Populate(string actorOne, string actorTwo)
        {
            var commandForActorOne = new PopulateGraphForActorCommand { ActorName = actorOne };
            var commandForActorTwo = new PopulateGraphForActorCommand { ActorName = actorTwo };

            await _mediator.Send(commandForActorOne);
            await _mediator.Send(commandForActorTwo);

            return RedirectToAction("DegreesOfSeparation", new { actorOne, actorTwo });
        }

        [HttpGet]
        public IActionResult DegreesOfSeparation(string actorOne, string actorTwo)
        {
            return View(new ActorsViewModel{ ActorOne = actorOne, ActorTwo = actorTwo });
        }
    }
}