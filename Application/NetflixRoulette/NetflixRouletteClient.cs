using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Octogami.SixDegreesOfNetflix.Application.NetflixRoulette
{
    public class NetflixRouletteClient
    {
        private readonly HttpClient _httpClient;

        public NetflixRouletteClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Get a single response object. Use this when searching by title or title and year.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<NetflixRouletteResponse> GetSingleAsync(NetflixRouletteRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get many response objects. Use this when searching by actor or director.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<NetflixRouletteResponse>> GetManyAsync(NetflixRouletteRequest request)
        {
            var url = BuildRequestUrl(request);
            var httpResponse = await _httpClient.GetAsync(url);
            var jsonContent = await httpResponse.Content.ReadAsStringAsync();
            var deserialized = JsonConvert.DeserializeObject<List<NetflixRouletteResponse>>(jsonContent);
            return deserialized;
        }

        private static string BuildRequestUrl(NetflixRouletteRequest request)
        {
            var baseUrl = "https://netflixroulette.net/api/api.php?";
            var stringBuilder = new StringBuilder(baseUrl);

            if (request.Title != null)
            {
                stringBuilder.Append($"title={request.Title}");
            }

            if (request.Year != 0)
            {
                stringBuilder.Append($"&year={request.Year}");
            }

            if (request.Actor != null)
            {
                stringBuilder.Append($"actor={request.Actor}");
            }

            if (request.Director != null)
            {
                stringBuilder.Append($"director={request.Director}");
            }

            return stringBuilder.ToString();
        }
    }
}
