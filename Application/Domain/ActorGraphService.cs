using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octogami.SixDegreesOfNetflix.Application.NetflixRoulette;

namespace Octogami.SixDegreesOfNetflix.Application.Domain
{
    public class ActorGraphService
    {
        private readonly INetflixRouletteClient _netflixRouletteClient;

        public ActorGraphService(INetflixRouletteClient netflixRouletteClient)
        {
            _netflixRouletteClient = netflixRouletteClient;
        }

        public Task<IEnumerable<Actor>> GetActorsInGraphAsync(string name, int iterations)
        {
            return Task.FromResult(Enumerable.Empty<Actor>());
        }
    }
}