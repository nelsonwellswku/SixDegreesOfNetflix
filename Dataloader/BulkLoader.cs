using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CosmosDB.BulkExecutor.Graph;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Octogami.SixDegreesOfNetflix.Dataloader
{
    public interface IBulkLoader
    {
        Task BulkInsertAsync(IEnumerable<object> graphElements, CancellationToken token = default(CancellationToken));
    }

    public class BulkLoader : IBulkLoader
    {
        private readonly DocumentClient _documentClient;

        private readonly CosmosGraphConfiguration _cosmosConfiguration;

        public BulkLoader(DocumentClient documentClient, CosmosGraphConfiguration cosmosConfiguration)
        {
            _documentClient = documentClient;
            _cosmosConfiguration = cosmosConfiguration;
        }

        public async Task BulkInsertAsync(IEnumerable<object> graphElements, CancellationToken token = default(CancellationToken))
        {
            _documentClient.ConnectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds = 30;
            _documentClient.ConnectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests = 9;

            var resourceResponse = await _documentClient.ReadDocumentCollectionAsync(_cosmosConfiguration.DocumentCollectionLink);

            GraphBulkExecutor executor = new GraphBulkExecutor(_documentClient, resourceResponse.Resource);

            await executor.InitializeAsync();

            _documentClient.ConnectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds = 0;
            _documentClient.ConnectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests = 0;

            await executor.BulkImportAsync(
                graphElements,
                enableUpsert: true,
                cancellationToken: token
            );

            _documentClient.ConnectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds = 30;
            _documentClient.ConnectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests = 9;

            return;
        }
    }
}