using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CosmosDB.BulkExecutor.Graph.Element;

namespace Octogami.SixDegreesOfNetflix.Dataloader
{
    public interface IMovieActorLinker
    {
        Task LinkRecordsAsync(List<MovieAndActorRecord> records);
    }

    public class MovieActorLinker : IMovieActorLinker
    {
        private readonly IBulkLoader _bulkLoader;

        public MovieActorLinker(IBulkLoader bulkLoader)
        {
            _bulkLoader = bulkLoader;
        }

        public Task LinkRecordsAsync(List<MovieAndActorRecord> records)
        {
            return _bulkLoader.BulkInsertAsync(records.Select(x =>
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