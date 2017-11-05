using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Octogami.SixDegreesOfNetflix.Application.Domain;
using Octogami.SixDegreesOfNetflix.Application.Unit.Tests.TestSupport;

namespace Octogami.SixDegreesOfNetflix.Application.Unit.Tests.Domain
{
    // ReSharper disable once InconsistentNaming
    public class TMDbActorServiceTests
    {
        public TMDBActorService ActorService { get; set; }

        [SetUp]
        public void SetUp()
        {
            ActorService = new TMDBActorService(new TMDbClientFake());
        }

        [Test]
        public async Task HappyPath_GetsTheActorsConnectedToJohnny()
        {
            // Arrange

            // Act
            var results = await ActorService.GetActorsFromExternalDataSourceAsync("Johnny", 10);

            // Assert
            Assert.That(results.Count(), Is.EqualTo(4));
        }

        [Test]
        public async Task DoesNotExplodeWhenActorNotFound()
        {
            // Arrange

            // Act
            var results = await ActorService.GetActorsFromExternalDataSourceAsync("Timmy", 10);

            // Assert
            Assert.That(results.Count(), Is.EqualTo(0));
        }
    }
}
