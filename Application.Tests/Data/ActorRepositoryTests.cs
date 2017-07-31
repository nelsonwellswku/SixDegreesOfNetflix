using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Graphs;
using Microsoft.Azure.Graphs.Elements;
using NUnit.Framework;
using Octogami.SixDegreesOfNetflix.Application.Data;

namespace Octogami.SixDegreesOfNetflix.Application.Tests.Data
{
    public class ActorRepositoryTests
    {
        private GraphDatabaseConfiguration Config { get; set; }
        private DocumentClient DocumentClient { get; set; }
        private DocumentCollection Graph { get; set; }

        [SetUp]
        public void SetUp()
        {
            Config = new GraphDatabaseConfiguration();
            DocumentClient = new DocumentClient(Config.Uri, Config.AuthKey);
            DocumentClient.CreateDatabaseIfNotExistsAsync(new Database {Id = Config.Name}).Wait();
            Graph = DocumentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(Config.Name),
                new DocumentCollection {Id = Config.CollectionName}).Result;
        }

        [Test]
        public async Task CanAddAnActorToTheGraph()
        {
            // Arrange
            var actorRepository = new ActorRepository(DocumentClient, Graph);

            // Act
            var actor = new Actor
            {
                Name = "John Smith",
                MoviesActedIn = new List<string> {"Movie One", "Movie Two"}
            };
            await actorRepository.CreateActorAsync(actor);

            // Assert
            var query = DocumentClient.CreateGremlinQuery(Graph, $"g.V('{actor.Id}')");
            while (query.HasMoreResults)
            {
                var feed = await query.ExecuteNextAsync<Vertex>();
                var vertex = feed.Single();
                Assert.That(vertex.GetVertexProperties("name").First().Value.Equals("John Smith"));
            }

            query = DocumentClient.CreateGremlinQuery(Graph, $"g.V('{actor.Id}').outE('actedIn').inV().hasLabel('movie')");
            var moviesList = new List<string>();
            while (query.HasMoreResults)
            {
                foreach (var vertex in await query.ExecuteNextAsync<Vertex>())
                {
                    moviesList.Add((string) vertex.GetVertexProperties("title").First().Value);
                }
            }

            Assert.That(moviesList, Is.EquivalentTo(actor.MoviesActedIn));
        }
    }
}
