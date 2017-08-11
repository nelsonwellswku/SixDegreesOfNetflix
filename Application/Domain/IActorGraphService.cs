using System.Collections.Generic;
using System.Threading.Tasks;

namespace Octogami.SixDegreesOfNetflix.Application.Domain
{
    public interface IActorGraphService
    {
        /// <summary>
        /// Get all of the actors in all of the movies up to a depth "iterations" from the actor passed in.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="iterations"></param>
        /// <remarks>This is not a great interface.</remarks>
        /// <returns></returns>
        Task<IEnumerable<Actor>> GetActorsFromExternalDataSourceAsync(string name, int iterations);
    }
}