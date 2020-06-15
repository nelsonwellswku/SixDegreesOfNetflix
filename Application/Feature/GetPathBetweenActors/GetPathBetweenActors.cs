using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Structure;
using MediatR;

namespace Octogami.SixDegreesOfNetflix.Application.Feature.GetPathBetweenActors
{
    public class GetPathBetweenActorsCommand : IRequest<ActorPath>
    {
        public string ActorOne { get; set; }
        public string ActorTwo { get; set; }
    }

    public class ActorPath
    {
        public string Name { get; set; }

        public MoviePath ActedIn { get; set; }
    }

    public class MoviePath
    {
        public string Title { get; set; }

        public ActorPath With { get; set; }
    }

    public class GetPathBetweenActorsCommandHandler : IRequestHandler<GetPathBetweenActorsCommand, ActorPath>
    {
        private readonly IGremlinClient _gremlinClient;

        public GetPathBetweenActorsCommandHandler(IGremlinClient gremlinClient)
        {
            _gremlinClient = gremlinClient;
        }

        private string _getActorByNameQuery = "g.V().has('Name', actorName)";

        private string _getPathBetweenActorsQuery = "g.V(actorOneId).repeat(both().simplePath()).until(hasId(actorTwoId)).path().limit(1)";

        public async Task<ActorPath> Handle(GetPathBetweenActorsCommand request, CancellationToken cancellationToken)
        {
            var actorOneId = await GetActorIdByName(request.ActorOne);
            var actorTwoId = await GetActorIdByName(request.ActorTwo);

            if (actorOneId == null || actorTwoId == null)
            {
                Console.WriteLine("One of the actors was not found.");
                return null;
            }

            ResultSet<dynamic> pathResults = null;
            try
            {
                pathResults = await _gremlinClient.SubmitAsync<dynamic>(_getPathBetweenActorsQuery, new Dictionary<string, object> {
                    { "actorOneId", actorOneId },
                    { "actorTwoId", actorTwoId }
                });
            }
            catch (ResponseException e)
            {
                if (!e.Message.Contains("ServerTimeout"))
                {
                    throw;
                }
            }

            if (pathResults == null || !pathResults.Any())
            {
                return null;
            }

            var currentActorPath = new ActorPath();
            var root = currentActorPath;

            foreach (var pathResult in pathResults)
            {
                foreach (var vertex in pathResult.Objects)
                {
                    var results = await _gremlinClient.SubmitAsync<dynamic>("g.V(vid).properties()", new Dictionary<string, object> { { "vid", vertex.Id } });
                    foreach (var prop in results)
                    {
                        if (vertex.Label == "Actor" && prop.Key == "Name")
                        {
                            currentActorPath.Name = prop.Value;
                            currentActorPath.ActedIn = new MoviePath();
                        }

                        if (vertex.Label == "Movie" && prop.Key == "Title")
                        {
                            currentActorPath.ActedIn.Title = prop.Value;
                            currentActorPath.ActedIn.With = new ActorPath();
                            currentActorPath = currentActorPath.ActedIn.With;
                        }
                    }
                }
            }

            return root;
        }

        private async Task<string> GetActorIdByName(string name)
        {
            var results = await _gremlinClient.SubmitWithSingleResultAsync<Vertex>(
                _getActorByNameQuery,
                 new Dictionary<string, object> { { "actorName", name } });

            return results == null ? null : results.Id.ToString();
        }
    }
}
