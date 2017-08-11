using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Octogami.SixDegreesOfNetflix.Application.Domain;
using Octogami.SixDegreesOfNetflix.Application.NetflixRoulette;

namespace Octogami.SixDegreesOfNetflix.Application.Unit.Tests.Domain
{
    public class ActorGraphServiceTests
    {
        private INetflixRouletteClient _netflixClientMock;

        private ActorGraphService _actorGraphService;

        private NetflixRouletteRequest _johnnyRequest;
        private NetflixRouletteRequest _marlaRequest;
        private NetflixRouletteRequest _timothyRequest;
        private NetflixRouletteRequest _laylaRequest;

        private MoviesActedIn _girlsNightOut;
        private MoviesActedIn _comeSailAway;
        private MoviesActedIn _wickedOnWestEnd;
        private MoviesActedIn _jux;

        [SetUp]
        public void SetUp()
        {
            _netflixClientMock = Substitute.For<INetflixRouletteClient>();

            _johnnyRequest = new NetflixRouletteRequest { Actor = "Johnny" };
            _marlaRequest = new NetflixRouletteRequest { Actor = "Marla" };
            _timothyRequest = new NetflixRouletteRequest { Actor = "Timothy" };
            _laylaRequest = new NetflixRouletteRequest { Actor = "Layla" };

            _girlsNightOut = new MoviesActedIn
            {
                MovieTitle = "Girl's Night Out",
                Actors = new List<string> {"Johnny", "Marla"}
            };

            _comeSailAway = new MoviesActedIn
            {
                MovieTitle = "Come Sail Away",
                Actors = new List<string> {"Marla", "Timothy"}
            };

            _wickedOnWestEnd = new MoviesActedIn
            {
                MovieTitle = "Wicked on West End",
                Actors = new List<string> {"Timothy", "Layla"}
            };

            _jux = new MoviesActedIn
            {
                MovieTitle = "Jux",
                Actors = new List<string> {"Layla"}
            };

            SetUpResponse(_johnnyRequest, new List<MoviesActedIn>
            {
                _girlsNightOut
            });

            SetUpResponse(_marlaRequest, new List<MoviesActedIn>
            {
                _comeSailAway,
                _girlsNightOut
            });

            SetUpResponse(_timothyRequest, new List<MoviesActedIn>
            {
                _comeSailAway,
                _wickedOnWestEnd
            });

            SetUpResponse(_laylaRequest, new List<MoviesActedIn>
            {
                _jux,
                _wickedOnWestEnd
            });

            _actorGraphService = new ActorGraphService(_netflixClientMock);
        }


        // Iteration 1: Johnny's Movies
        // Iteration 2: Marla's Movies
        // Iteration 3: Timothy's Movies
        [Test]
        public async Task GetActorForThreeIterations()
        {
            // Arrange

            // Act
            var results = await _actorGraphService.GetActorsFromExternalDataSourceAsync("Johnny", 3);

            // Assert
            var dictionary = results.ToDictionary(x => x.Name, x => x);
            var johnny = dictionary["Johnny"];
            var marla = dictionary["Marla"];
            var timothy = dictionary["Timothy"];
            
            Assert.That(johnny.MoviesActedIn, Is.EquivalentTo(new[] { "Girl's Night Out"}));
            Assert.That(marla.MoviesActedIn, Is.EquivalentTo(new[] { "Come Sail Away", "Girl's Night Out"}));
            Assert.That(timothy.MoviesActedIn, Is.EquivalentTo(new[] { "Come Sail Away", "Wicked on West End"}));
        }

        [Test]
        public async Task NoActorShouldBeRequestedMoreThanOnce()
        {
            // Arrange

            // Act
            var results = await _actorGraphService.GetActorsFromExternalDataSourceAsync("Johnny", 3);

            // Assert
            await _netflixClientMock.Received(1).GetManyAsync(Arg.Is<NetflixRouletteRequest>(x => x.Actor == "Johnny"));
            await _netflixClientMock.Received(1).GetManyAsync(Arg.Is<NetflixRouletteRequest>(x => x.Actor == "Marla"));
            await _netflixClientMock.Received(1).GetManyAsync(Arg.Is<NetflixRouletteRequest>(x => x.Actor == "Timothy"));
            await _netflixClientMock.DidNotReceive().GetManyAsync(Arg.Is<NetflixRouletteRequest>(x => x.Actor == "Layla"));
        }

        private void SetUpResponse(
            NetflixRouletteRequest request,
            List<MoviesActedIn> showsActedIn)
        {
            _netflixClientMock.GetManyAsync(Arg.Is<NetflixRouletteRequest>(x => x.Actor == request.Actor))
                .Returns(Task.FromResult((showsActedIn.Select(x => new NetflixRouletteResponse
                {
                    show_title = x.MovieTitle,
                    show_cast = string.Join(", ", x.Actors)
                }).ToList(), (NetflixRouletteError)null)));
        }

        private class MoviesActedIn
        {
            public string MovieTitle { get; set; }
            public List<string> Actors { get; set; }
        }
    }
}
