using System;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Octogami.SixDegreesOfNetflix.Application.Infrastructure.Configuration;
using Octogami.SixDegreesOfNetflix.Application.Feature.GetPathBetweenActors;

namespace Website
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddMediatR(typeof(GetPathBetweenActorsCommand));

            services.Configure<DocumentConfiguration>(Configuration.GetSection("Cosmos").GetSection("Document"));
            services.Configure<GraphConfiguration>(Configuration.GetSection("Cosmos").GetSection("Graph"));
            services.AddScoped<DocumentConfiguration>(ctx =>
                ctx.GetRequiredService<IOptionsMonitor<DocumentConfiguration>>().CurrentValue);
            services.AddScoped<GraphConfiguration>(ctx =>
                ctx.GetRequiredService<IOptionsMonitor<GraphConfiguration>>().CurrentValue);

            services.AddSingleton(x =>
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

            services.AddScoped<IGremlinClient>(ctx =>
            {
                var graphConfiguration = ctx.GetService<GraphConfiguration>();

                var gremlinServer = new GremlinServer(
                    graphConfiguration.Host,
                    graphConfiguration.Port,
                    enableSsl: graphConfiguration.UseSSL,
                    username: graphConfiguration.Username,
                    password: graphConfiguration.Password
                );

                return new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
