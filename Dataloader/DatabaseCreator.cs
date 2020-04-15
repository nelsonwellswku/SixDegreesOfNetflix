using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Octogami.SixDegreesOfNetflix.Dataloader
{
    public interface IDatabaseCreator
    {
        Task EnsureDatabaseCreated();

        Task EnsureCollectionCreated();
    }

    public class DatabaseCreator : IDatabaseCreator
    {
        private readonly GraphConfiguration _graphConfiguration;

        private readonly DocumentClient _documentClient;

        public DatabaseCreator(DocumentClient documentClient, GraphConfiguration graphConfiguration)
        {
            _documentClient = documentClient;
            _graphConfiguration = graphConfiguration;
        }

        public async Task EnsureDatabaseCreated()
        {
            Console.WriteLine($"Creating database {_graphConfiguration.DatabaseName}...");
            await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = _graphConfiguration.DatabaseName });
            Console.WriteLine("Database creation complete.");
        }

        public async Task EnsureCollectionCreated()
        {
            PartitionKeyDefinition partitionKey = new PartitionKeyDefinition
            {
                Paths = new Collection<string> { $"/{_graphConfiguration.PartitionKey}" }
            };
            DocumentCollection collection = new DocumentCollection { Id = _graphConfiguration.CollectionName, PartitionKey = partitionKey };

            Console.WriteLine($"Creating collection {_graphConfiguration.CollectionName}...");
            await _documentClient
                .CreateDocumentCollectionIfNotExistsAsync($"/dbs/{_graphConfiguration.DatabaseName}", collection);
            Console.WriteLine("Collection creation complete.");
        }
    }
}