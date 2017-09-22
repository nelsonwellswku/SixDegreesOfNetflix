using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Octogami.SixDegreesOfNetflix.Application.Data;
using Octogami.SixDegreesOfNetflix.Application.Domain;
using Octogami.SixDegreesOfNetflix.Application.Tests.TestSupport;

namespace Octogami.SixDegreesOfNetflix.Application.Tests.Data
{
    public class ActorPathRepositoryTests
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
        public async Task GetPathBetweenActors()
        {
            // Arrange
            var keanu = new Actor
            {
                Name = "Keanu",
                MoviesActedIn = new HashSet<string>() { "Speed" }
            };

            var matt = new Actor
            {
                Name = "Matt",
                MoviesActedIn = new HashSet<string> { "Speed", "Slow" }
            };

            var tiffany = new Actor
            {
                Name = "Tiffany",
                MoviesActedIn = new HashSet<string> { "Slow" }
            };

            var actorRepository = new ActorRepository(GremlinClient);
            await actorRepository.SaveActorAsync(keanu);
            await actorRepository.SaveActorAsync(matt);
            await actorRepository.SaveActorAsync(tiffany);

            var actorPathRepository = new ActorPathRepository(GremlinClient);

            // Act
            var path = await actorPathRepository.GetPathBetweenActors(keanu.Id, tiffany.Id);

            // Assert
            Assert.That(path    // keanu
                .ActedIn        // speed
                .With           // matt
                .ActedIn        // slow
                .With           // ...
                .Name, Is.EqualTo("Tiffany"));
        }
    }
}
