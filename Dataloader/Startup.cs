using System;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
            serviceCollection.Configure<DocumentConfiguration>(configuration.GetSection("Cosmos").GetSection("Document"));
            serviceCollection.Configure<GraphConfiguration>(configuration.GetSection("Cosmos").GetSection("Graph"));
            serviceCollection.AddTransient<DocumentConfiguration>(ctx =>
                ctx.GetRequiredService<IOptionsMonitor<DocumentConfiguration>>().CurrentValue);
            serviceCollection.AddTransient<GraphConfiguration>(ctx =>
                ctx.GetRequiredService<IOptionsMonitor<GraphConfiguration>>().CurrentValue);

            serviceCollection.AddSingleton(x =>
            {
                var documentConfiguration = x.GetRequiredService<DocumentConfiguration>();
                ConnectionPolicy connectionPolicy = new ConnectionPolicy
                {
                    ConnectionMode = ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp
                };

                var documentClient = new DocumentClient(
                    new Uri($"https://{documentConfiguration.Host}:{documentConfiguration.Port}"),
                    documentConfiguration.AuthKey,
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
                var graphConfiguration = ctx.GetService<GraphConfiguration>();
                return new Func<IGremlinClient>(() =>
                {
                    var gremlinServer = new GremlinServer(
                        graphConfiguration.Host,
                        graphConfiguration.Port,
                        enableSsl: graphConfiguration.UseSSL,
                        username: graphConfiguration.Username,
                        password: graphConfiguration.Password
                    );

                    return new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);
                });
            });

            return serviceCollection.BuildServiceProvider(validateScopes: true);
        }
    }
}