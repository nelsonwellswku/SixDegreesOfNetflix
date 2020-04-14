using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace Octogami.SixDegreesOfNetflix.Dataloader
{
    public interface IMovieActorLinker
    {
        Task LinkRecordsAsync(List<MovieAndActorRecord> records);
    }

    public class MovieActorLinker : IMovieActorLinker
    {
        private readonly Func<IGremlinClient> _gremlinClientFactory;
        private string _insertLink = "g.V(actorId).addE('actedIn').to(g.V(titleId)).property('id', edgeId)";

        public MovieActorLinker(Func<IGremlinClient> gremlinClientFactory)
        {
            _gremlinClientFactory = gremlinClientFactory;
        }

        public async Task LinkRecordsAsync(List<MovieAndActorRecord> records)
        {
            using (var gremlinClient = _gremlinClientFactory())
            {
                foreach (var item in records)
                {
                    var actorId = item.NameId;
                    var titleId = item.TitleId;
                    var edgeId = $"{item.NameId}_{item.TitleId}";
                    try
                    {
                        await gremlinClient.SubmitAsync(_insertLink, new Dictionary<string, object> {
                            { "actorId", actorId },
                            { "titleId", titleId },
                            { "edgeId", edgeId },
                        });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Couldn't write link. \r\n {e.Message}");
                        Console.WriteLine($"Actor Id: {actorId}");
                        Console.WriteLine($"Title Id: {titleId}");
                    }
                }
            }

            return;
        }
    }
}