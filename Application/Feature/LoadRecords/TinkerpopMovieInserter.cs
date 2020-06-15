using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gremlin.Net.Driver;

namespace Octogami.SixDegreesOfNetflix.Application.Feature.LoadRecords
{
    public class TinkerpopMovieInserter : IMovieInserter
    {
        private readonly Func<IGremlinClient> _gremlinClientFactory;

        private string _insertMovie = @"g.addV('Movie').property('TitleId', titleId).property('Title', title).property('Year', year)";

        public TinkerpopMovieInserter(Func<IGremlinClient> gremlinClientFactory)
        {
            _gremlinClientFactory = gremlinClientFactory;
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

            using (var gremlinClient = _gremlinClientFactory())
            {
                foreach (var movie in distinctMovies)
                {
                    var bindings = new Dictionary<string, object> {
                        {"titleId", movie.TitleId},
                        {"title", movie.MovieTitle },
                        {"year", movie.Year }
                    };

                    await gremlinClient.SubmitAsync(_insertMovie, bindings);
                }
            }
        }
    }
}