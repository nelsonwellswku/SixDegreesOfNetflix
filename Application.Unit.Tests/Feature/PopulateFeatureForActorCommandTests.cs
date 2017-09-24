using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Octogami.SixDegreesOfNetflix.Application.Data;
using Octogami.SixDegreesOfNetflix.Application.Domain;
using Octogami.SixDegreesOfNetflix.Application.Feature;

namespace Octogami.SixDegreesOfNetflix.Application.Unit.Tests.Feature
{
    public class PopulateFeatureForActorCommandTests
    {
        private IActorService _actorGraphServiceMock;
        private IActorRepository _actorRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _actorGraphServiceMock = Substitute.For<IActorService>();
            _actorRepositoryMock = Substitute.For<IActorRepository>();

            _actorGraphServiceMock.GetActorsFromExternalDataSourceAsync("Johnny", 6).Returns(new List<Actor>
            {
                new Actor {Name = "Johnny"},
                new Actor {Name = "Marla"}
            });
        }

        [Test]
        public async Task SavesActorsFromExternalDataSource()
        {
            // Arrange
            var handler = new PopulateGraphForActorCommandHandler(_actorGraphServiceMock, _actorRepositoryMock);

            // Act
            await handler.Handle(new PopulateGraphForActorCommand {ActorName = "Johnny"});

            // Assert
            await _actorRepositoryMock.Received(1).SaveActorAsync(Arg.Is<Actor>(x => x.Name == "Johnny"));
            await _actorRepositoryMock.Received(1).SaveActorAsync(Arg.Is<Actor>(x => x.Name == "Marla"));
        }
    }
}
