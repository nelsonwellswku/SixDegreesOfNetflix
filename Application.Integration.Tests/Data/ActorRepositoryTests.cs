using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Graphs;
using Microsoft.Azure.Graphs.Elements;
using NUnit.Framework;
using Octogami.SixDegreesOfNetflix.Application.Data;
using Octogami.SixDegreesOfNetflix.Application.Domain;

namespace Octogami.SixDegreesOfNetflix.Application.Tests.Data
{
    public class ActorRepositoryTests
    {
        private GraphDatabaseConfiguration Config { get; set; }
        private GremlinClient GremlinClient { get; set; }

        [SetUp]
        public void SetUp()
        {
            Config = new GraphDatabaseConfiguration();
            var documentClient = new DocumentClient(Config.Uri, Config.AuthKey);
            documentClient.CreateDatabaseIfNotExistsAsync(new Database {Id = Config.Name}).Wait();
            var graph = documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(Config.Name),
                new DocumentCollection {Id = Config.CollectionName}).Result;

            // Empty out the database so each test starts fresh
            var query = documentClient.CreateGremlinQuery(graph, "g.V().drop()");
            query.ExecuteNextAsync().Wait();

            GremlinClient = new GremlinClient(documentClient, graph);
        }

        [Test]
        public async Task CanAddAnActorToTheGraph()
        {
            // Arrange
            var actorRepository = new ActorRepository(GremlinClient);

            // Act
            var actor = new Actor
            {
                Name = "John Smith",
                MoviesActedIn = new List<string> {"Movie One", "Movie Two"}
            };
            await actorRepository.SaveActorAsync(actor);

            // Assert
            var query = $"g.V('{actor.Id}')";
            var results = await GremlinClient.ExecuteQueryAsync<Vertex>(query);
            Assert.That(results.Single().GetVertexProperties("name").First().Value.Equals("John Smith"));

            query = $"g.V('{actor.Id}').outE('actedIn').inV().hasLabel('movie')";
            results = await GremlinClient.ExecuteQueryAsync<Vertex>(query);
            var moviesList = new List<string>();
            foreach (var vertex in results)
            {
                moviesList.Add((string)vertex.GetVertexProperties("title").First().Value);
            }

            Assert.That(moviesList, Is.EquivalentTo(actor.MoviesActedIn));
        }

        [Test]
        public async Task WillNotCreateDuplicateActorsOrMovies()
        {
            // Arrange
            var actorRepository = new ActorRepository(GremlinClient);

            // Act
            var actor = new Actor
            {
                Name = "Keanu Reeves",
                MoviesActedIn = new List<string> {"The Matrix", "Speed", "Constantine", "John Wick"}
            };

            foreach (var i in Enumerable.Range(0, 10))
            {
                await actorRepository.SaveActorAsync(actor);
            }

            // Assert
            var query = "g.V().count()";
            var results = await GremlinClient.ExecuteQueryAsync<dynamic>(query);
            long vertexCount = results.Single();
            Assert.That(vertexCount, Is.EqualTo(5));

            query = "g.E().count()";
            results = await GremlinClient.ExecuteQueryAsync<dynamic>(query);
            long edgeCount = results.Single();
            Assert.That(edgeCount, Is.EqualTo(4));
        }
    }
}
