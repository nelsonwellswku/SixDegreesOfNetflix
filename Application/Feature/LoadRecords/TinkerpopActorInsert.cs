using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gremlin.Net.Driver;

namespace Octogami.SixDegreesOfNetflix.Application.Feature.LoadRecords
{
    public class TinkerpopActorInserter : IActorInserter
    {
        private readonly Func<IGremlinClient> _gremlinClientFactory;

        private string _insertMovie = "g.addV('Actor').property('NameId', nameId).property('Name', name)";

        public TinkerpopActorInserter(Func<IGremlinClient> gremlinClientFactory)
        {
            _gremlinClientFactory = gremlinClientFactory;
        }

        public async Task InsertActorsAsync(IEnumerable<MovieAndActorRecord> records)
        {
            var distinctActors = records
                .GroupBy(x => new
                {
                    x.NameId,
                    x.Actor,
                }).Select(group => group.First());

            using (var gremlinClient = _gremlinClientFactory())
            {
                foreach (var actor in distinctActors)
                {
                    var bindings = new Dictionary<string, object> {
                        {"nameId", actor.NameId},
                        {"name", actor.Actor },
                    };

                    await gremlinClient.SubmitAsync(_insertMovie, bindings);
                }
            }
        }
    }
}