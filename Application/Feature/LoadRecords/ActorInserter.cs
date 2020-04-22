using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CosmosDB.BulkExecutor.Graph.Element;
using Octogami.SixDegreesOfNetflix.Application.Infrastructure.Configuration;
using Octogami.SixDegreesOfNetflix.Application.Infrastructure.Data;

namespace Octogami.SixDegreesOfNetflix.Application.Feature.LoadRecords
{
    public interface IActorInserter
    {
        Task InsertActorsAsync(IEnumerable<MovieAndActorRecord> records);
    }

    public class ActorInserter : IActorInserter
    {
        private readonly IBulkLoader _bulkLoader;
        private readonly GraphConfiguration _graphConfiguration;

        public ActorInserter(IBulkLoader bulkLoader, GraphConfiguration graphConfiguration)
        {
            _bulkLoader = bulkLoader;
            _graphConfiguration = graphConfiguration;
        }

        public async Task InsertActorsAsync(IEnumerable<MovieAndActorRecord> records)
        {
            var distinctActors = records
                .GroupBy(x => new
                {
                    x.NameId,
                    x.Actor,
                }).Select(group => group.First());

            await _bulkLoader.BulkInsertAsync(distinctActors.Select(x =>
            {
                var vertex = new GremlinVertex(x.NameId, "Actor");
                vertex.AddProperty(_graphConfiguration.PartitionKey, x.Actor);
                vertex.AddProperty("Name", x.Actor);
                return vertex;
            }), CancellationToken.None);
        }
    }
}