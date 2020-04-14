using System;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
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
                    new Uri($"https://{cosmosConfiguration.Host}:8081"),
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
            serviceCollection.AddSingleton<Func<IGremlinClient>>(ctx =>
            {
                var graphConfiguration = ctx.GetService<CosmosGraphConfiguration>();
                return new Func<IGremlinClient>(() =>
                {
                    Console.WriteLine(graphConfiguration.Port);
                    var gremlinServer = new GremlinServer(
                        graphConfiguration.Host,
                        graphConfiguration.Port,
                        enableSsl: graphConfiguration.UseSSL,
                        username: graphConfiguration.Username,
                        password: graphConfiguration.Password
                    );
                    Console.WriteLine(gremlinServer.Uri);

                    return new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);
                });
            });

            return serviceCollection.BuildServiceProvider(validateScopes: true);
        }
    }
}