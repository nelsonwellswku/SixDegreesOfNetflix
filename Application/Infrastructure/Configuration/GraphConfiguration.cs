namespace Octogami.SixDegreesOfNetflix.Application.Infrastructure.Configuration
{
    public class GraphConfiguration
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public bool UseSSL { get; set; }

        public string Password { get; set; }

        public string PartitionKey { get; set; }

        public string DatabaseName { get; set; }

        public string CollectionName { get; set; }

        public string Username => DocumentCollectionLink;

        public string DocumentCollectionLink => $"/dbs/{DatabaseName}/colls/{CollectionName}";
    }
}