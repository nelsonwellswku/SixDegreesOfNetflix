using System.Threading.Tasks;
using MediatR;
using Octogami.SixDegreesOfNetflix.Application.Data;
using Octogami.SixDegreesOfNetflix.Application.Domain;

namespace Octogami.SixDegreesOfNetflix.Application.Feature
{
    public class PopulateGraphForActorCommand : IRequest
    {
        public string ActorName { get; set; }
        public int Depth => 6;

        public class PopulateGraphForActorCommandHandler : IAsyncRequestHandler<PopulateGraphForActorCommand>
        {
            private readonly IActorGraphService _actorGraphService;
            private readonly IActorRepository _actorRepository;

            public PopulateGraphForActorCommandHandler(IActorGraphService actorGraphService,
                IActorRepository actorRepository)
            {
                _actorGraphService = actorGraphService;
                _actorRepository = actorRepository;
            }

            public async Task Handle(PopulateGraphForActorCommand message)
            {
                var actors = await _actorGraphService.GetActorsFromExternalDataSourceAsync(message.ActorName, message.Depth);

                foreach (var actor in actors)
                {
                    await _actorRepository.SaveActorAsync(actor);
                }
            }
        }
    }
}