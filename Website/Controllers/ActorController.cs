using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Octogami.SixDegreesOfNetflix.Application.Feature;
using Octogami.SixDegreesOfNetflix.Website.Models;

namespace Octogami.SixDegreesOfNetflix.Website.Controllers
{
    public class ActorController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ActorController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
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
        public async Task<IActionResult> DegreesOfSeparation(GetPathBetweenActorsCommand command)
        {
            var path = await _mediator.Send(command);
            var pathViewModel = _mapper.Map<ActorPathViewModel>(path);

            return View(new ActorsViewModel{ ActorOne = command.ActorOne, ActorTwo = command.ActorTwo, ActorPath = pathViewModel });
        }
    }
}