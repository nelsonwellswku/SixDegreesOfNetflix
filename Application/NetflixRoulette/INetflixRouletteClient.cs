using System.Collections.Generic;
using System.Threading.Tasks;

namespace Octogami.SixDegreesOfNetflix.Application.NetflixRoulette
{
    public interface INetflixRouletteClient
    {
        /// <summary>
        ///     Get a single response object. Use this when searching by title or title and year.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<(NetflixRouletteResponse, NetflixRouletteError)> GetSingleAsync(
            NetflixRouletteRequest request);

        /// <summary>
        ///     Get many response objects. Use this when searching by actor or director.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<(List<NetflixRouletteResponse>, NetflixRouletteError)> GetManyAsync(
            NetflixRouletteRequest request);
    }
}