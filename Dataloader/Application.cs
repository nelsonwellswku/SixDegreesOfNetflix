using System;
using System.Linq;
using System.Threading.Tasks;

namespace Octogami.SixDegreesOfNetflix.Dataloader
{
    public class Application
    {
        private readonly IBulkLoader _bulkLoader;

        private readonly IDatabaseCreator _databaseCreator;

        private readonly CosmosGraphConfiguration _graphConfiguration;

        private readonly IMovieInserter _movieInserter;

        private readonly IActorInserter _actorInserter;

        private readonly IMovieRecordReader _movieRecordReader;

        private readonly IMovieActorLinker _movieActorLinker;

        public Application(
            IBulkLoader bulkLoader,
            IDatabaseCreator databaseCreator,
            CosmosGraphConfiguration graphConfiguration,
            IMovieInserter movieInserter,
            IActorInserter actorInserter,
            IMovieRecordReader movieRecordReader,
            IMovieActorLinker movieActorLinker)
        {
            _bulkLoader = bulkLoader;
            _databaseCreator = databaseCreator;
            _graphConfiguration = graphConfiguration;
            _movieInserter = movieInserter;
            _actorInserter = actorInserter;
            _movieRecordReader = movieRecordReader;
            _movieActorLinker = movieActorLinker;
        }

        public async Task RunAsync(string filePath)
        {
            await _databaseCreator.EnsureDatabaseCreated();

            await _databaseCreator.EnsureCollectionCreated();

            Console.WriteLine("Reading input file...");
            var records = _movieRecordReader.ReadRecords(filePath).Take(200).ToList();
            Console.WriteLine("Input file read.");

            Console.WriteLine("Inserting movies...");
            await _movieInserter.InsertMoviesAsync(records);
            Console.WriteLine("Movies inserted.");

            Console.WriteLine("Inserting actors...");
            await _actorInserter.InsertActorsAsync(records);
            Console.WriteLine("Actors inserted.");

            Console.WriteLine("Inserting edges between nodes...");
            await _movieActorLinker.LinkRecordsAsync(records);
            Console.WriteLine("Edges inserted.");
        }
    }
}