using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private CosmosConfiguration _cosmosConfiguration;

        public BulkLoader(CosmosConfiguration cosmosConfiguration)
        {
            _cosmosConfiguration = cosmosConfiguration;
        }

        public async Task BulkInsertAsync(IEnumerable<object> graphElements, CancellationToken token = default(CancellationToken))
        {
            ConnectionPolicy connectionPolicy = new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            };

            var documentClient = new DocumentClient(
                new Uri(_cosmosConfiguration.Host),
                _cosmosConfiguration.Password,
                connectionPolicy
            );

            await documentClient.CreateDatabaseIfNotExistsAsync(new Database() { Id = _cosmosConfiguration.DatabaseName });

            PartitionKeyDefinition partitionKey = new PartitionKeyDefinition
            {
                Paths = new Collection<string> { $"/{_cosmosConfiguration.PartitionKey}" }
            };
            DocumentCollection collection = new DocumentCollection { Id = _cosmosConfiguration.CollectionName, PartitionKey = partitionKey };

            var resourceResponse = await documentClient
                .CreateDocumentCollectionIfNotExistsAsync($"/dbs/{_cosmosConfiguration.DatabaseName}", collection);

            documentClient.ConnectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds = 30;
            documentClient.ConnectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests = 9;

            GraphBulkExecutor executor = new GraphBulkExecutor(documentClient, resourceResponse.Resource);

            await executor.InitializeAsync();

            documentClient.ConnectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds = 0;
            documentClient.ConnectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests = 0;

            var response = await executor.BulkImportAsync(
                graphElements,
                enableUpsert: true,
                cancellationToken: token
            );

            Console.WriteLine("Number of documents imported " + response.NumberOfDocumentsImported);
            Console.WriteLine("Number of bad input documents are " + response.BadInputDocuments.Count);

            return;
        }
    }
}