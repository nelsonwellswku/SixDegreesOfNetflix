using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CosmosDB.BulkExecutor.Graph.Element;
using Octogami.SixDegreesOfNetflix.Application.Infrastructure.Configuration;
using Octogami.SixDegreesOfNetflix.Application.Infrastructure.Data;

namespace Octogami.SixDegreesOfNetflix.Application.Feature.LoadRecords
{
    public interface IMovieInserter
    {
        Task InsertMoviesAsync(IEnumerable<MovieAndActorRecord> records);
    }

    public class MovieInserter : IMovieInserter
    {
        private readonly IBulkLoader _bulkLoader;

        private readonly GraphConfiguration _graphConfiguration;

        public MovieInserter(IBulkLoader bulkLoader, GraphConfiguration graphConfiguration)
        {
            _bulkLoader = bulkLoader;
            _graphConfiguration = graphConfiguration;
        }

        public async Task InsertMoviesAsync(IEnumerable<MovieAndActorRecord> records)
        {
            var distinctMovies = records
                .GroupBy(x => new
                {
                    x.TitleId,
                    x.MovieTitle,
                    x.Year
                })
                .Select(group => group.First());

            await _bulkLoader.BulkInsertAsync(distinctMovies.Select(x =>
            {
                var vertex = new GremlinVertex(x.TitleId, "Movie");
                vertex.AddProperty(_graphConfiguration.PartitionKey, x.MovieTitle);
                vertex.AddProperty("Title", x.MovieTitle);
                vertex.AddProperty("Year", x.Year);
                return vertex;
            }), CancellationToken.None);
        }
    }
}