using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Graphs.Elements;
using NUnit.Framework;
using Octogami.SixDegreesOfNetflix.Application.Data;
using Octogami.SixDegreesOfNetflix.Application.Domain;
using Octogami.SixDegreesOfNetflix.Application.Tests.TestSupport;

namespace Octogami.SixDegreesOfNetflix.Application.Tests.Data
{
    public class ActorRepositoryTests
    {
        private GremlinClient GremlinClient { get; set; }

        [SetUp]
        public void SetUp()
        {
            // Empty out the database so each test starts fresh
            var query = "g.V().drop()";
            GremlinClient = GremlinClientFactory.GetNewGremlinClient();
            GremlinClient.ExecuteQueryAsync<dynamic>(query).Wait();
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
                MoviesActedIn = new HashSet<string> {"Movie One", "Movie Two"}
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
                MoviesActedIn = new HashSet<string> {"The Matrix", "Speed", "Constantine", "John Wick"}
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

        [Test]
        public async Task CanGetActorIdByName()
        {
            // Arrange
            var actorRepository = new ActorRepository(GremlinClient);
            var actor = new Actor { Name = "Wilbur" };
            await actorRepository.SaveActorAsync(actor);

            // Act
            var id = await actorRepository.GetActorIdByName(actor.Name);

            // Assert
            Assert.That(id, Is.EqualTo(actor.Id).And.Not.Null);
        }
    }
}
