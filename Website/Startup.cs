using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octogami.SixDegreesOfNetflix.Application.Data;
using Octogami.SixDegreesOfNetflix.Application.Domain;
using Octogami.SixDegreesOfNetflix.Application.Feature;
using Octogami.SixDegreesOfNetflix.Application.NetflixRoulette;
using Octogami.SixDegreesOfNetflix.Website.Mapping;

namespace Octogami.SixDegreesOfNetflix.Website
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddMediatR(typeof(PopulateGraphForActorCommand).Assembly);

            services.AddOptions();

            var dbConfiguration = Configuration.GetSection("Database");
            services.Configure<GraphDatabaseConfiguration>(dbConfiguration);

            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IActorPathRepository, ActorPathRepository>();
            services.AddScoped<IActorService, ActorService>();
            services.AddScoped<IGremlinClient, GremlinClient>();
            services.AddScoped<INetflixRouletteClient, NetflixRouletteClient>();

            services.AddSingleton<IMapper>(ctx =>
            {
                var mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<ActorPathProfile>();
                });

                var mapper = new Mapper(mapperConfiguration);

                ((IMapper) mapper).ConfigurationProvider.AssertConfigurationIsValid();

                return mapper;
            });

            var dbCreated = false;
            services.AddSingleton(typeof(DocumentClient), ctx =>
            {
                var config = ctx.GetService<IOptions<GraphDatabaseConfiguration>>();
                var client = new DocumentClient(new Uri(config.Value.Uri), config.Value.AuthKey);

                if (!dbCreated)
                {
                    client.CreateDatabaseIfNotExistsAsync(new Database { Id = config.Value.Name }).Wait();
                    dbCreated = true;
                }

                return client;
            });

            var collectionCreated = false;
            services.AddScoped(typeof(DocumentCollection), ctx =>
            {
                var config = ctx.GetService<IOptions<GraphDatabaseConfiguration>>();
                var documentClient = ctx.GetService<DocumentClient>();

                if (!collectionCreated)
                {
                   documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(config.Value.Name),
                        new DocumentCollection { Id = config.Value.CollectionName }).Wait();
                    collectionCreated = true;
                }

                var collectionUri = UriFactory.CreateDocumentCollectionUri(config.Value.Name, config.Value.CollectionName);
                var collection = documentClient.ReadDocumentCollectionAsync(collectionUri).Result;
                return collection.Resource;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
