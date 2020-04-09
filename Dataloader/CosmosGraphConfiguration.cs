using Microsoft.Extensions.Configuration;

namespace Octogami.SixDegreesOfNetflix.Dataloader
{
    public class CosmosGraphConfiguration
    {
        private readonly IConfiguration _configuration;

        public CosmosGraphConfiguration(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("CosmosGraph");
        }

        public string Host => _configuration["Host"];

        public int Port => int.Parse(_configuration["Port"]);

        public bool UseSSL => bool.Parse(_configuration["UseSSL"]);

        public string Username => DocumentCollectionLink;

        public string Password => _configuration["Password"];

        public string PartitionKey => _configuration["PartitionKey"];

        public string DatabaseName => _configuration["DatabaseName"];

        public string CollectionName => _configuration["CollectionName"];

        public string DocumentCollectionLink => $"/dbs/{DatabaseName}/colls/{CollectionName}";
    }
}