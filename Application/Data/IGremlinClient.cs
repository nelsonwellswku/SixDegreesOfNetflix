using System.Collections.Generic;
using System.Threading.Tasks;

namespace Octogami.SixDegreesOfNetflix.Application.Data
{
    public interface IGremlinClient
    {
        Task<IList<T>> ExecuteQueryAsync<T>(string gremlinQuery);
    }
}