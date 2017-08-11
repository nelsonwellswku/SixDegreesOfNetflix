using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Graphs.Elements;
using Octogami.SixDegreesOfNetflix.Application.Domain;

namespace Octogami.SixDegreesOfNetflix.Application.Data
{
    public class ActorRepository
    {
        private readonly GremlinClient _gremlinClient;

        public ActorRepository(GremlinClient gremlinClient)
        {
            _gremlinClient = gremlinClient;
        }

        public async Task SaveActorAsync(Actor actor)
        {
            await CreateActorIfNotExists(actor);

            foreach (var movie in actor.MoviesActedIn)
            {
                var movieId = await CreateMovieIfNotExistsAsync(movie);
                await CreateEdgeIfNotExistsAsync(actor.Id, movieId);
            }
        }

        private async Task CreateActorIfNotExists(Actor actor)
        {
            var doesExistQuery = $"g.V().has('name', '{actor.Name}')";
            var actorSearchResults = await _gremlinClient.ExecuteQueryAsync<Vertex>(doesExistQuery);
            var actorSearchVertex = actorSearchResults.SingleOrDefault();
            if (actorSearchVertex != null)
            {
                actor.Id = Guid.Parse((string) actorSearchVertex.Id);
                return;
            }

            // Actor didn't exist already, so let's create it
            var createActorString = $"g.addV('person').property('name', '{actor.Name}')";
            var createActorResults = await _gremlinClient.ExecuteQueryAsync<Vertex>(createActorString);
            actor.Id = createActorResults.Single().IdGuid();
        }

        private async Task<Guid> CreateMovieIfNotExistsAsync(string movie)
        {
            var doesExistQuery = $"g.V().has('title', '{movie}')";
            var doesExistResults = await _gremlinClient.ExecuteQueryAsync<Vertex>(doesExistQuery);
            var vertex = doesExistResults.SingleOrDefault();
            if (vertex != null)
                return vertex.IdGuid();

            // Movie didn't already exist, so let's create it
            var createMovieQuery = $"g.addV('movie').property('title', '{movie}')";
            var createMovieResults = await _gremlinClient.ExecuteQueryAsync<Vertex>(createMovieQuery);
            return createMovieResults.Single().IdGuid();
        }

        private async Task CreateEdgeIfNotExistsAsync(Guid actorId, Guid movieId)
        {
            var edgeExistsQuery =
                $"g.V('{actorId}').outE('actedIn').as('e').inV().hasLabel('movie').has('id', '{movieId}').select('e')";
            var edgeExistsResults = await _gremlinClient.ExecuteQueryAsync<Edge>(edgeExistsQuery);
            if (edgeExistsResults.Any())
                return;

            var createEdgeString = $"g.V('{actorId}').addE('actedIn').to(g.V('{movieId}'))";
            await _gremlinClient.ExecuteQueryAsync<dynamic>(createEdgeString);
        }
    }
}