using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.Azure.CosmosDB.BulkExecutor.Graph.Element;

namespace Octogami.SixDegreesOfNetflix.Dataloader
{
    public class Application
    {
        private readonly IBulkLoader _bulkLoader;

        private readonly IDatabaseCreator _databaseCreator;

        private readonly CosmosGraphConfiguration _graphConfiguration;

        public Application(IBulkLoader bulkLoader, IDatabaseCreator databaseCreator, CosmosGraphConfiguration graphConfiguration)
        {
            _bulkLoader = bulkLoader;
            _databaseCreator = databaseCreator;
            _graphConfiguration = graphConfiguration;
        }

        public async Task RunAsync(string filePath)
        {
            await _databaseCreator.EnsureDatabaseCreated();

            await _databaseCreator.EnsureCollectionCreated();

            var records = GetRecords(filePath);

            Console.WriteLine("Inserting movies...");
            await InsertMoviesAsync(records, _bulkLoader, _graphConfiguration);
            Console.WriteLine("Movies inserted.");

            Console.WriteLine("Inserting actors...");
            await InsertActorsAsync(records, _bulkLoader, _graphConfiguration);
            Console.WriteLine("Actors inserted.");

            Console.WriteLine("Inserting edges between nodes...");
            await InsertEdgesAsync(records, _bulkLoader);
            Console.WriteLine("Edges inserted.");
        }

        public static List<MovieAndActorRecord> GetRecords(string filePath)
        {
            Console.WriteLine("Reading data input file...");
            List<MovieAndActorRecord> records = new List<MovieAndActorRecord>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var list = csv.GetRecords<MovieAndActorRecord>().ToList();
                Console.WriteLine("Data input file read.");
                return list;
            }
        }

        public static async Task InsertMoviesAsync(List<MovieAndActorRecord> records, IBulkLoader bulkLoader, CosmosGraphConfiguration cosmosConfiguration)
        {
            var distinctMovies = records
                .GroupBy(x => new
                {
                    x.TitleId,
                    x.MovieTitle,
                    x.Year
                })
                .Select(group => group.First());

            await bulkLoader.BulkInsertAsync(distinctMovies.Select(x =>
            {
                var vertex = new GremlinVertex(x.TitleId, "Movie");
                vertex.AddProperty(cosmosConfiguration.PartitionKey, x.MovieTitle);
                vertex.AddProperty("Title", x.MovieTitle);
                vertex.AddProperty("Year", x.Year);
                return vertex;
            }), CancellationToken.None);
        }

        public static async Task InsertActorsAsync(List<MovieAndActorRecord> records, IBulkLoader bulkLoader, CosmosGraphConfiguration cosmosConfiguration)
        {
            var distinctActors = records
                .GroupBy(x => new
                {
                    x.NameId,
                    x.Actor,
                }).Select(group => group.First());

            await bulkLoader.BulkInsertAsync(distinctActors.Select(x =>
            {
                var vertex = new GremlinVertex(x.NameId, "Actor");
                vertex.AddProperty(cosmosConfiguration.PartitionKey, x.Actor);
                vertex.AddProperty("Name", x.Actor);
                return vertex;
            }), CancellationToken.None);
        }

        public static async Task InsertEdgesAsync(List<MovieAndActorRecord> records, IBulkLoader bulkLoader)
        {
            await bulkLoader.BulkInsertAsync(records.Select(x =>
            {
                var edge = new GremlinEdge(
                    $"{x.NameId}_{x.TitleId}",
                    "ActedIn",
                    x.NameId,
                    x.TitleId,
                    "ActedIn",
                    "HadActor",
                    $"{x.NameId}_{x.TitleId}_out",
                    $"{x.NameId}_{x.TitleId}_in"
                );
                return edge;
            }), CancellationToken.None);
        }
    }
}