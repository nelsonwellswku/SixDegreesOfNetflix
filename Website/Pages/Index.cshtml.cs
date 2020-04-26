using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Octogami.SixDegreesOfNetflix.Application.Feature.GetPathBetweenActors;

namespace Website.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IMediator mediator, ILogger<IndexModel> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public string ActorOne { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ActorTwo { get; set; }

        public ActorPath ActorPath { get; set; }

        public async Task OnGet()
        {
            if (ActorOne != null && ActorTwo != null)
            {
                ActorPath = await _mediator.Send(new GetPathBetweenActorsCommand
                {
                    ActorOne = ActorOne,
                    ActorTwo = ActorTwo
                });
            }
        }
    }
}
