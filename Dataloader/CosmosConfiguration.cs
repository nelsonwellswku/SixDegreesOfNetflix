namespace Octogami.SixDegreesOfNetflix.Dataloader
{
    public class CosmosConfiguration
    {
        public string Host { get; set; } = "https://localhost:8081";

        public int Port { get; set; } = 8901;

        public bool UseSSL { get; set; } = false;

        public string Username => $"/dbs/{DatabaseName}/colls/{CollectionName}";

        public string Password { get; set; } = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        public string PartitionKey { get; set; } = "PartitionKey";

        public string DatabaseName { get; set; } = "sixdegrees";

        public string CollectionName { get; set; } = "data";
    }
}