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

        public async Task<IEnumerable<Actor>> GetActorsInGraphAsync(string name, int iterations)
        {
            var (success, error) = await _netflixRouletteClient.GetManyAsync(new NetflixRouletteRequest {Actor = name});

            return Enumerable.Empty<Actor>();
        }
    }
}