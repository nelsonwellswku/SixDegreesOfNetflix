using System;
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

        [Test]
        public async Task SearchByActorWithNoResultsYields404()
        {
            // Arrange
            var request = new NetflixRouletteRequest {Actor = "Wallace Terrible"};

            // Act
            var (response, error) = await _client.GetManyAsync(request);

            // Assert
            Assert.AreEqual("404", error.errorcode);
            Assert.AreEqual("Sorry! We couldn't find any movies with that actor!", error.message);
            Assert.IsNull(response);
        }

        [Test]
        public async Task SearchByDirectorWithNoResultsYields404()
        {
            // Arrange
            var request = new NetflixRouletteRequest {Director = "Jean Ralphio"};

            // Act
            var (response, error) = await _client.GetManyAsync(request);

            // Assert
            Assert.AreEqual("404", error.errorcode);
            Assert.AreEqual("Sorry! We couldn't find any movies directed by that director!", error.message);
            Assert.IsNull(response);
        }

        [Test]
        public async Task SearchByTitleWithNoResultsYields404()
        {
            // Arrange
            var request = new NetflixRouletteRequest {Title = "What's in a Movie?"};

            // Act
            var (response, error) = await _client.GetSingleAsync(request);

            // Assert
            Assert.AreEqual("404", error.errorcode);
            Assert.AreEqual("Sorry! We couldn't find a movie with that title!", error.message);
            Assert.IsNull(response);
        }

        [Test]
        public async Task SearchByTitleAndYearWithNoResultsYields404()
        {
            // Arrange
            var request = new NetflixRouletteRequest {Title = "What's in a movie?", Year = 2010};

            // Act
            var (response, error) = await _client.GetSingleAsync(request);

            // Assert
            Assert.AreEqual("404", error.errorcode);
            Assert.AreEqual("Sorry! We couldn't find a movie with that title!", error.message);
            Assert.IsNull(response);
        }

        [Test]
        public async Task SearchInputWithInvalidCharactersReturnsError()
        {
            // Arrange
            var request = new NetflixRouletteRequest {Actor = "Johnny Depp*"};

            // Act
            var (response, error) = await _client.GetManyAsync(request);

            // Assert
            Assert.AreEqual("400", error.errorcode);
            Assert.AreEqual("Sorry! Your input can only contain letters", error.message);
            Assert.IsNull(response);
        }

        [Test]
        public async Task SearchInputMustBeAtLeastFiveCharacters()
        {
            // Arrange
            var request = new NetflixRouletteRequest {Actor = "Jay"};

            // Act
            var (response, error) = await _client.GetManyAsync(request);

            // Assert
            Assert.AreEqual("400", error.errorcode);
            Assert.AreEqual("Sorry! Your input length must be greater than 5", error.message);
            Assert.IsNull(response);
        }

        [Test]
        public void GetManyAsyncRequiresActorOrDirector()
        {
            // Arrange
            var request = new NetflixRouletteRequest();

            // Act
            AsyncTestDelegate action = async () => await _client.GetManyAsync(request);

            // Assert
            Assert.That(action, Throws.TypeOf<ArgumentException>().With.Message.And.Message.EqualTo("Either actor or director must be provided. Both can not be null."));
        }

        [Test]
        public void GetSingleAsyncRequiresTitle()
        {
            // Arrange
            var request = new NetflixRouletteRequest();

            // Act
            AsyncTestDelegate action = async () => await _client.GetSingleAsync(request);

            // Assert
            Assert.That(action, Throws.TypeOf<ArgumentException>().With.Message.And.Message.EqualTo("Title can not be null."));
        }
    }
}