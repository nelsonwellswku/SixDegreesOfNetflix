using System;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Octogami.SixDegreesOfNetflix.Dataloader
{
    public class Startup
    {
        public ServiceProvider GetServiceCollection()
        {
            var serviceCollection = new ServiceCollection();

            var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appSettings.json", optional: false);
            var configuration = configurationBuilder.Build();
            serviceCollection.AddSingleton<IConfiguration>(configuration);

            serviceCollection.AddSingleton<CosmosGraphConfiguration>();

            serviceCollection.AddSingleton(x =>
            {
                var cosmosConfiguration = x.GetRequiredService<CosmosGraphConfiguration>();
                ConnectionPolicy connectionPolicy = new ConnectionPolicy
                {
                    ConnectionMode = ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp
                };

                var documentClient = new DocumentClient(
                    new Uri(cosmosConfiguration.Host),
                    cosmosConfiguration.Password,
                    connectionPolicy
                );

                return documentClient;
            });

            serviceCollection.AddSingleton<Application>();
            serviceCollection.AddSingleton<IDatabaseCreator, DatabaseCreator>();
            serviceCollection.AddSingleton<IBulkLoader, BulkLoader>();
            serviceCollection.AddSingleton<IMovieInserter, MovieInserter>();
            serviceCollection.AddSingleton<IActorInserter, ActorInserter>();
            serviceCollection.AddSingleton<IMovieActorLinker, MovieActorLinker>();
            serviceCollection.AddSingleton<IMovieRecordReader, MovieRecordReader>();

            return serviceCollection.BuildServiceProvider(validateScopes: true);
        }
    }
}