using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Octogami.SixDegreesOfNetflix.Application.Data;
using Octogami.SixDegreesOfNetflix.Application.Domain;

namespace Octogami.SixDegreesOfNetflix.Application.Feature
{
    public class GetPathBetweenActorsCommand : IRequest<ActorPath>
    {
        public string ActorOne { get; set; }
        public string ActorTwo { get; set; }
    }

    public class GetPathBetweenActorsCommandHandler : IAsyncRequestHandler<GetPathBetweenActorsCommand, ActorPath>
    {
        private readonly IActorRepository _actorRepository;
        private readonly IActorPathRepository _actorPathRepository;

        public GetPathBetweenActorsCommandHandler(IActorRepository actorRepository, IActorPathRepository actorPathRepository)
        {
            _actorRepository = actorRepository;
            _actorPathRepository = actorPathRepository;
        }

        public async Task<ActorPath> Handle(GetPathBetweenActorsCommand message)
        {
            var actorOneIdTask = _actorRepository.GetActorIdByName(message.ActorOne);
            var actorTwoIdTask = _actorRepository.GetActorIdByName(message.ActorTwo);

            var actorOneId = await actorOneIdTask;
            var actorTwoId = await actorTwoIdTask;

            if (actorOneId == null || actorTwoId == null)
            {
                return null;
            }

            var actorPath = await _actorPathRepository.GetPathBetweenActors(actorOneId.Value, actorTwoId.Value);
            return actorPath;
        }
    }
}
