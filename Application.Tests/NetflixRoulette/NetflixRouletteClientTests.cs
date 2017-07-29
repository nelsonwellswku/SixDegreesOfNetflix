using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Octogami.SixDegreesOfNetflix.Application.NetflixRoulette;

namespace Octogami.SixDegreesOfNetflix.Application.Tests.NetflixRoulette
{
    public class NetflixRouletteClientTests
    {
        private NetflixRouletteClient Client;

        [SetUp]
        public void SetUp()
        {
            Client = new NetflixRouletteClient(new HttpClient());
        }

        [Test]
        public async Task CanSearchByTitle()
        {
            // Arrange
            var request = new NetflixRouletteRequest {Title = "The Boondocks", Year = 2005};

            // Act
            var response = await Client.GetSingleAsync(request);

            // Assert
            Assert.AreEqual("The Boondocks", response.show_title);
        }

        [Test]
        public async Task CanSearchByTitleAndYear()
        {
            // Arrange
            var request = new NetflixRouletteRequest {Title = "The Boondocks", Year = 2005};

            // Act
            var response = await Client.GetSingleAsync(request);

            // Assert
            Assert.AreEqual("The Boondocks", response.show_title);
        }

        [Test]
        public async Task CanSearchByActor()
        {
            // Arrange
            var request = new NetflixRouletteRequest {Actor = "Nicolas Cage"};

            // Act
            var response = await Client.GetManyAsync(request);

            // Assert
            var responseList = response.ToList();
            Assert.Greater(responseList.Count(), 1);
            Assert.IsTrue(responseList.First().show_cast.Contains("Nicolas Cage"));
        }

        [Test]
        public async Task CanSearchByDirector()
        {
            // Arrange
            var request = new NetflixRouletteRequest {Director = "Quentin Tarantino"};

            // Act
            var response = await Client.GetManyAsync(request);

            // Assert
            var responseList = response.ToList();
            Assert.Greater(response.Count(), 1);
            Assert.IsTrue(responseList.First().director.Contains("Tarantino"));
        }
    }
}