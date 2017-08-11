using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Octogami.SixDegreesOfNetflix.Application.NetflixRoulette;

namespace Octogami.SixDegreesOfNetflix.Application.Domain
{
    public class ActorGraphService : IActorGraphService
    {
        private readonly INetflixRouletteClient _netflixRouletteClient;

        public ActorGraphService(INetflixRouletteClient netflixRouletteClient)
        {
            _netflixRouletteClient = netflixRouletteClient;
        }

        public async Task<IEnumerable<Actor>> GetActorsFromExternalDataSourceAsync(string name, int iterations)
        {
            var actorLookup = new ConcurrentDictionary<string, Actor>(StringComparer.InvariantCultureIgnoreCase);
            var requestCache = new ConcurrentDictionary<string, byte>();

            await ProcessAsync(name, iterations, actorLookup, requestCache);

            return actorLookup.Values;
        }

        // Using a concurrent dictionary as the request cache because there's no built-in concurrent set. The value is a throw-away.
        private async Task ProcessAsync(string name, int iterations, ConcurrentDictionary<string, Actor> dict, ConcurrentDictionary<string, byte> requestCache)
        {
            if (iterations <= 0 || requestCache.ContainsKey(name))
            {
                return;
            }

            var (success, error) = await _netflixRouletteClient.GetManyAsync(new NetflixRouletteRequest {Actor = name});
            requestCache.TryAdd(name, 0);

            if (error != null)
            {
                return;
            }

            foreach (var movie in success)
            {
                var title = movie.show_title;
                foreach (var castMember in movie.CastMembers)
                {
                    if (!dict.ContainsKey(castMember))
                    {
                        var actor = new Actor
                        {
                            Name = castMember
                        };
                        dict[castMember] = actor;
                    }

                    dict[castMember].MoviesActedIn.Add(title);

                    await ProcessAsync(castMember, iterations - 1, dict, requestCache);
                }
            }
        }
    }
}