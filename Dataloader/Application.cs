using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Octogami.SixDegreesOfNetflix.Application.Feature.LoadRecords;
using Octogami.SixDegreesOfNetflix.Application.Infrastructure.Data;
using Octogami.SixDegreesOfNetflix.Application.Infrastructure.Configuration;
using System.Linq;

namespace Octogami.SixDegreesOfNetflix.Dataloader
{
    public class Application
    {
        private readonly IBulkLoader _bulkLoader;

        private readonly IDatabaseCreator _databaseCreator;

        private readonly GraphConfiguration _graphConfiguration;

        private readonly IMovieInserter _movieInserter;

        private readonly IActorInserter _actorInserter;

        private readonly IMovieRecordReader _movieRecordReader;

        private readonly IMovieActorLinker _movieActorLinker;

        public Application(
            IBulkLoader bulkLoader,
            IDatabaseCreator databaseCreator,
            GraphConfiguration graphConfiguration,
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
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            // await _databaseCreator.EnsureDatabaseCreated();

            // await _databaseCreator.EnsureCollectionCreated();

            Console.WriteLine("Reading input file...");
            var records = _movieRecordReader.ReadRecords(filePath);
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
            stopwatch.Stop();

            Console.WriteLine($"Finished executing after {stopwatch.Elapsed.TotalSeconds} seconds.");
        }
    }
}