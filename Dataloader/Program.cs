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
    class Program
    {
        static async Task Main(string[] args)
        {
            var filePath = args[0];
            if (filePath == null)
            {
                Console.WriteLine("No input file provided.");
                return;
            }

            var records = GetRecords(filePath);

            var cosmosConfiguration = new CosmosConfiguration();
            var bulkLoader = new BulkLoader(cosmosConfiguration);

            Console.WriteLine("Inserting movies...");
            await InsertMoviesAsync(records, bulkLoader, cosmosConfiguration);
            Console.WriteLine("Movies inserted.");

            Console.WriteLine("Inserting actors...");
            await InsertActorsAsync(records, bulkLoader, cosmosConfiguration);
            Console.WriteLine("Actors inserted.");

            Console.WriteLine("Inserting edges between nodes...");
            await InsertEdgesAsync(records, bulkLoader);
            Console.WriteLine("Nodes inserted.");
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

        public static async Task InsertMoviesAsync(List<MovieAndActorRecord> records, IBulkLoader bulkLoader, CosmosConfiguration cosmosConfiguration)
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

        public static async Task InsertActorsAsync(List<MovieAndActorRecord> records, IBulkLoader bulkLoader, CosmosConfiguration cosmosConfiguration)
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
