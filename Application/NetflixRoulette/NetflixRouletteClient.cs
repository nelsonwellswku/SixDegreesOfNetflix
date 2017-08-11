using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Octogami.SixDegreesOfNetflix.Application.NetflixRoulette
{
    public class NetflixRouletteClient : INetflixRouletteClient
    {
        public NetflixRouletteClient()
        {
            HttpClient = new HttpClient {BaseAddress = new Uri("https://netflixroulette.net")};
        }

        public HttpClient HttpClient { get; set; }

        /// <summary>
        ///     Get a single response object. Use this when searching by title or title and year.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<(NetflixRouletteResponse, NetflixRouletteError)> GetSingleAsync(
            NetflixRouletteRequest request)
        {
            if (request.Title == null)
            {
                throw new ArgumentException("Title can not be null.");
            }

            var url = BuildRequestUrl(request);
            var httpResponse = await HttpClient.GetAsync(url);
            var jsonContent = await httpResponse.Content.ReadAsStringAsync();

            if (httpResponse.IsSuccessStatusCode)
            {
                var successResponse = JsonConvert.DeserializeObject<NetflixRouletteResponse>(jsonContent);
                return (successResponse, null);
            }

            var errorResponse = JsonConvert.DeserializeObject<NetflixRouletteError>(jsonContent);
            return (null, errorResponse);
        }

        /// <summary>
        ///     Get many response objects. Use this when searching by actor or director.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<(List<NetflixRouletteResponse>, NetflixRouletteError)> GetManyAsync(
            NetflixRouletteRequest request)
        {
            if (request.Actor == null && request.Director == null)
            {
                throw new ArgumentException("Either actor or director must be provided. Both can not be null.");
            }

            var url = BuildRequestUrl(request);
            var httpResponse = await HttpClient.GetAsync(url);
            var jsonContent = await httpResponse.Content.ReadAsStringAsync();

            if (httpResponse.IsSuccessStatusCode)
            {
                var successResponse = JsonConvert.DeserializeObject<List<NetflixRouletteResponse>>(jsonContent);
                return (successResponse, null);
            }

            var errorResponse = JsonConvert.DeserializeObject<NetflixRouletteError>(jsonContent);
            return (null, errorResponse);
        }

        private static string BuildRequestUrl(NetflixRouletteRequest request)
        {
            var baseUrl = "/api/api.php?";
            var stringBuilder = new StringBuilder(baseUrl);

            if (request.Title != null)
                stringBuilder.Append($"title={request.Title}");

            if (request.Year != 0)
                stringBuilder.Append($"&year={request.Year}");

            if (request.Actor != null)
                stringBuilder.Append($"actor={request.Actor}");

            if (request.Director != null)
                stringBuilder.Append($"director={request.Director}");

            return stringBuilder.ToString();
        }
    }
}