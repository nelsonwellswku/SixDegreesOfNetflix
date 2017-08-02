using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Graphs;

namespace Octogami.SixDegreesOfNetflix.Application.Data
{
    public interface IGremlinClient
    {
        Task<IList<T>> ExecuteQueryAsync<T>(string gremlinQuery);
    }

    public class GremlinClient : IGremlinClient
    {
        private readonly DocumentClient _documentClient;
        private readonly DocumentCollection _graph;

        public GremlinClient(DocumentClient documentClient, DocumentCollection graph)
        {
            _documentClient = documentClient;
            _graph = graph;
        }

        public async Task<IList<T>> ExecuteQueryAsync<T>(string gremlinQuery)
        {
            var query = _documentClient.CreateGremlinQuery(_graph, gremlinQuery);

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var feed = await query.ExecuteNextAsync<T>();
                results.AddRange(feed.Select(x => x));
            }
            return results;
        }
    }
}