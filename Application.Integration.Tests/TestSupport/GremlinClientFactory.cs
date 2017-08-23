using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Octogami.SixDegreesOfNetflix.Application.Data;

namespace Octogami.SixDegreesOfNetflix.Application.Tests.TestSupport
{
    public static class GremlinClientFactory
    {
        public static GremlinClient GetNewGremlinClient()
        {
            var config = new GraphDatabaseConfiguration
            {
                AuthKey = Constants.TestAuthKey,
                CollectionName = Constants.TestCollectionName,
                Uri = Constants.TestUri,
                Name = Constants.TestDatabaseName
            };
            var documentClient = new DocumentClient(new Uri(config.Uri), config.AuthKey);
            documentClient.CreateDatabaseIfNotExistsAsync(new Database {Id = config.Name}).Wait();
            var graph = documentClient.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(config.Name),
                new DocumentCollection {Id = config.CollectionName}).Result;

            var gremlinClient = new GremlinClient(documentClient, graph);
            return gremlinClient;
        }
    }
}