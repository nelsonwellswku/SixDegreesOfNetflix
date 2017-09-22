using System;
using System.Threading.Tasks;
using Octogami.SixDegreesOfNetflix.Application.Domain;

namespace Octogami.SixDegreesOfNetflix.Application.Data
{
    public interface IActorPathRepository
    {
        Task<ActorPath> GetPathBetweenActors(Guid actorOneId, Guid actorTwoId);
    }
}