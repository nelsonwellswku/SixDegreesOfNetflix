using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Octogami.SixDegreesOfNetflix.Application.NetflixRoulette;

namespace Octogami.SixDegreesOfNetflix.Application.Tests.NetflixRoulette
{
    public class NetflixRouletteClientTests
    {
        private NetflixRouletteClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new NetflixRouletteClient();
        }

        [Test]
        public async Task CanSearchByTitle()
        {
            // Arrange
            var request = new NetflixRouletteRequest {Title = "The Boondocks", Year = 2005};

            // Act
            var (response, error) = await _client.GetSingleAsync(request);

            // Assert
            Assert.AreEqual("The Boondocks", response.show_title);
            Assert.IsNull(error);
        }

        [Test]
        public async Task CanSearchByTitleAndYear()
        {
            // Arrange
            var request = new NetflixRouletteRequest {Title = "The Boondocks", Year = 2005};

            // Act
            var (response, error) = await _client.GetSingleAsync(request);

            // Assert
            Assert.AreEqual("The Boondocks", response.show_title);
            Assert.IsNull(error);
        }

        [Test]
        public async Task CanSearchByActor()
        {
            // Arrange
            var request = new NetflixRouletteRequest {Actor = "Nicolas Cage"};

            // Act
            var (response, error) = await _client.GetManyAsync(request);

            // Assert
            var responseList = response.ToList();
            Assert.Greater(responseList.Count, 1);
            Assert.IsTrue(responseList.First().show_cast.Contains("Nicolas Cage"));
            Assert.IsNull(error);
        }

        [Test]
        public async Task CanSearchByDirector()
        {
            // Arrange
            var request = new NetflixRouletteRequest {Director = "Quentin Tarantino"};

            // Act
            var (response, error) = await _client.GetManyAsync(request);

            // Assert
            var responseList = response.ToList();
            Assert.Greater(response.Count, 1);
            Assert.IsTrue(responseList.First().director.Contains("Tarantino"));
            Assert.IsNull(error);
        }
    }
}