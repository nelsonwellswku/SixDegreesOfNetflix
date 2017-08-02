using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Graphs;
using Microsoft.Azure.Graphs.Elements;

namespace Octogami.SixDegreesOfNetflix.Application.Data
{
    public class ActorRepository
    {
        private readonly DocumentClient _documentClient;
        private readonly DocumentCollection _graph;

        public ActorRepository(DocumentClient documentClient, DocumentCollection graph)
        {
            _documentClient = documentClient;
            _graph = graph;
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
            string doesExistString = $"g.V().has('name', '{actor.Name}')";
            var doesExistQuery = _documentClient.CreateGremlinQuery(_graph, doesExistString);
            while (doesExistQuery.HasMoreResults)
            {
                var feed = await doesExistQuery.ExecuteNextAsync<Vertex>();
                var vertex = feed.SingleOrDefault();
                if (vertex != null)
                {
                    actor.Id = Guid.Parse((string)vertex.Id);
                    return;
                }
            }

            // Actor didn't exist already, so let's create it
            string createActorString = $"g.addV('person').property('name', '{actor.Name}')";
            var createActorQuery = _documentClient.CreateGremlinQuery(_graph, createActorString);

            while (createActorQuery.HasMoreResults)
            {
                var feed = await createActorQuery.ExecuteNextAsync<Vertex>();
                var vertex = feed.Single();
                actor.Id = Guid.Parse((string)vertex.Id);
            }
        }

        private async Task<Guid> CreateMovieIfNotExistsAsync(string movie)
        {
            string doesExistString = $"g.V().has('title', '{movie}')";
            var doesExistQuery = _documentClient.CreateGremlinQuery(_graph, doesExistString);

            while (doesExistQuery.HasMoreResults)
            {
                var feed = await doesExistQuery.ExecuteNextAsync<Vertex>();
                var vertex = feed.SingleOrDefault();
                if (vertex != null)
                {
                    return Guid.Parse((string) vertex.Id);
                }
            }

            // Movie didn't already exist, so let's create it
            string createMovieString = $"g.addV('movie').property('title', '{movie}')";
            var createMovieQuery = _documentClient.CreateGremlinQuery(_graph, createMovieString);

            while (createMovieQuery.HasMoreResults)
            {
                var feed = await createMovieQuery.ExecuteNextAsync<Vertex>();
                var vertex = feed.Single();
                return Guid.Parse((string) vertex.Id);
            }

            // This should never happen
            throw new InvalidOperationException("Unable to find or create a movie vertex.");
        }

        private async Task CreateEdgeIfNotExistsAsync(Guid actorId, Guid movieId)
        {
            string edgeExistsString = $"g.V('{actorId}').outE('actedIn').as('e').inV().hasLabel('movie').has('id', '{movieId}').select('e')";
            var edgeExistsQuery = _documentClient.CreateGremlinQuery(_graph, edgeExistsString);

            while (edgeExistsQuery.HasMoreResults)
            {
                var feed = await edgeExistsQuery.ExecuteNextAsync<Edge>();
                var vertex = feed.SingleOrDefault();
                if (vertex != null)
                {
                    return;
                }
            }

            string createEdgeString = $"g.V('{actorId}').addE('actedIn').to(g.V('{movieId}'))";
            var createEdgeQuery = _documentClient.CreateGremlinQuery(_graph, createEdgeString);
            while (createEdgeQuery.HasMoreResults)
            {
                await createEdgeQuery.ExecuteNextAsync();
            }
        }
    }
}
