using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure;

namespace Octogami.SixDegreesOfNetflix.Application.Feature.LoadRecords
{
    public class TinkerpopMovieActorLinker : IMovieActorLinker
    {
        private readonly Func<IGremlinClient> _gremlinClientFactory;

        private string _getActorByNameQuery = "g.V().has('Name', actorName)";

        private string _getMovieByNameAndYearQuery = "g.V().has('Title', movieTitle).has('Year', year)";

        private string _insertLink = "g.V(actorId).addE('actedIn').to(g.V(titleId)).property('id', edgeId)";

        public TinkerpopMovieActorLinker(Func<IGremlinClient> gremlinClientFactory)
        {
            _gremlinClientFactory = gremlinClientFactory;
        }

        public async Task LinkRecordsAsync(List<MovieAndActorRecord> records)
        {
            using (var gremlinClient = _gremlinClientFactory())
            {
                foreach (var item in records)
                {
                    var actorId = await getActorIdByName(item.Actor, gremlinClient);
                    var titleId = await getTitleIdByName(item.MovieTitle, item.Year, gremlinClient);
                    var edgeId = $"{item.NameId}_{item.TitleId}";
                    try
                    {
                        await gremlinClient.SubmitAsync(_insertLink, new Dictionary<string, object> {
                            { "actorId", actorId },
                            { "titleId", titleId },
                            { "edgeId", edgeId },
                        });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Couldn't write link. \r\n {e.Message}");
                        Console.WriteLine($"Actor Id: {actorId}");
                        Console.WriteLine($"Title Id: {titleId}");
                    }
                }
            }

            return;
        }

        private async Task<string> getTitleIdByName(string movieTitle, string year, IGremlinClient gremlinClient)
        {
            var results = await gremlinClient.SubmitWithSingleResultAsync<Vertex>(
                _getMovieByNameAndYearQuery,
                new Dictionary<string, object>
                {
                    { "movieTitle", movieTitle },
                    { "year", year}
                });

            return results == null ? null : results.Id.ToString();
        }

        private async Task<string> getActorIdByName(string actor, IGremlinClient gremlinClient)
        {
            var results = await gremlinClient.SubmitWithSingleResultAsync<Vertex>(
                _getActorByNameQuery,
                 new Dictionary<string, object> { { "actorName", actor } });

            return results == null ? null : results.Id.ToString();
        }
    }
}