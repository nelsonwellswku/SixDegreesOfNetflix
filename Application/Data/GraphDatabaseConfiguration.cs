using System;
using System.Configuration;

namespace Octogami.SixDegreesOfNetflix.Application.Data
{
    public class GraphDatabaseConfiguration
    {
        public Uri Uri => new Uri(ConfigurationManager.AppSettings["graphDbUri"]);

        public string Name => ConfigurationManager.AppSettings["graphDbDatabaseName"];

        public string AuthKey => ConfigurationManager.AppSettings["graphDbAuthKey"];

        public string CollectionName => ConfigurationManager.AppSettings["graphDbCollectionName"];
    }
}