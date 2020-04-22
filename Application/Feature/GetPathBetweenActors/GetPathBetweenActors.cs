using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using MediatR;
using Newtonsoft.Json;

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

            var pathResults = await _gremlinClient.SubmitAsync<dynamic>(_getPathBetweenActorsQuery, new Dictionary<string, object> {
                { "actorOneId", actorOneId },
                { "actorTwoId", actorTwoId }
            });

            if (!pathResults.Any())
            {
                Console.WriteLine("No match found.");
                return null;
            }

            var serializedQueryResult = JsonConvert.SerializeObject((object)pathResults.First());
            var deserializedQueryResult = JsonConvert.DeserializeObject<RootObject>(serializedQueryResult);

            var currentActorPath = new ActorPath() { Name = "nobody" };
            var root = currentActorPath;

            foreach (var vertex in deserializedQueryResult.objects)
            {
                if (vertex.label == "Actor")
                {
                    currentActorPath.Name = vertex.properties.Name.First().value;
                    currentActorPath.ActedIn = new MoviePath();
                }

                if (vertex.label == "Movie")
                {
                    currentActorPath.ActedIn.Title = vertex.properties.Title.First().value;
                    currentActorPath.ActedIn.With = new ActorPath();
                    currentActorPath = currentActorPath.ActedIn.With;
                }
            }

            return root;
        }

        private async Task<string> GetActorIdByName(string name)
        {
            var results = await _gremlinClient.SubmitWithSingleResultAsync<Dictionary<string, object>>(
                _getActorByNameQuery,
                 new Dictionary<string, object> { { "actorName", name } });

            return results == null ? null : results["id"].ToString();
        }
    }
}
