using System;
using System.Linq;
using System.Threading.Tasks;
using Octogami.SixDegreesOfNetflix.Application.Domain;

namespace Octogami.SixDegreesOfNetflix.Application.Data
{
    public class ActorPathRepository
    {
        private readonly IGremlinClient _gremlinClient;

        public ActorPathRepository(IGremlinClient gremlinClient)
        {
            _gremlinClient = gremlinClient;
        }

        public async Task<ActorPath> GetPathBetweenActors(Guid actorOneId, Guid actorTwoId)
        {
            var pathQuery = $"g.V('{actorOneId}').repeat(both().simplePath()).until(hasId('{actorTwoId}')).path().limit(1)";
            var pathResults = await _gremlinClient.ExecuteQueryAsync<dynamic>(pathQuery);

            var objects = pathResults.FirstOrDefault()?.objects;
            if (objects == null)
            {
                return null;
            }

            var currentActorPath = new ActorPath();
            var root = currentActorPath;

            foreach (var item in objects)
            {
                var name = item.properties?.name?.First?.value?.Value;
                var title = item.properties?.title?.First?.value?.Value;

                if (name != null)
                {
                    currentActorPath.Name = name;
                    currentActorPath.ActedIn = new MoviePath();
                }

                if (title != null)
                {
                    currentActorPath.ActedIn.Title = title;
                    currentActorPath.ActedIn.With = new ActorPath();
                    currentActorPath = currentActorPath.ActedIn.With;
                }
            }

            return root;
        }
    }
}
