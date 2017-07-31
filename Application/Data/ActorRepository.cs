using System;
using System.Collections.Generic;
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

        public async Task CreateActorAsync(Actor actor)
        {
            string createActorString = $"g.addV('person').property('name', '{actor.Name}')";
            var createActorQuery = _documentClient.CreateGremlinQuery(_graph, createActorString);

            while (createActorQuery.HasMoreResults)
            {
                var feed = await createActorQuery.ExecuteNextAsync<Vertex>();
                var vertex = feed.Single();
                actor.Id = Guid.Parse((string) vertex.Id);
            }

            Dictionary<string, Guid> movieDictionary = new Dictionary<string, Guid>();
            foreach (var movie in actor.MoviesActedIn)
            {
                string createMovieString = $"g.addV('movie').property('title', '{movie}')";
                var createMovieQuery = _documentClient.CreateGremlinQuery(_graph, createMovieString);

                while (createMovieQuery.HasMoreResults)
                {
                    var feed = await createMovieQuery.ExecuteNextAsync<Vertex>();
                    var vertex = feed.Single();
                    movieDictionary.Add(movie, Guid.Parse((string) vertex.Id));
                }
            }

            foreach (var kv in movieDictionary)
            {
                string createEdgeString = $"g.V('{actor.Id}').addE('actedIn').to(g.V('{kv.Value}'))";
                var createEdgeQuery = _documentClient.CreateGremlinQuery(_graph, createEdgeString);
                while (createEdgeQuery.HasMoreResults)
                {
                    await createEdgeQuery.ExecuteNextAsync();
                }
            }
        }
    }
}
