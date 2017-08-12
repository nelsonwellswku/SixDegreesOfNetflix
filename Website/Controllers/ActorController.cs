using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Octogami.SixDegreesOfNetflix.Application.Feature;

namespace Octogami.SixDegreesOfNetflix.Website.Controllers
{
    public class ActorController : Controller
    {
        private readonly IMediator _mediator;

        public ActorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Degrees(string actorOne, string actorTwo)
        {
            var command = new PopulateGraphForActorCommand() {ActorName = actorOne};
            await _mediator.Send(command);

            return View();
        }
    }
}