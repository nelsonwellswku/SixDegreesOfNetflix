using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octogami.SixDegreesOfNetflix.Application.TMDB;
using TMDbLib.Objects.Movies;

namespace Octogami.SixDegreesOfNetflix.Application.Domain
{
    // ReSharper disable once InconsistentNaming
    public class TMDBActorService : IActorService
    {
        private readonly ITMDbClient _tmdbClient;

        public TMDBActorService(ITMDbClient tmdbClient)
        {
            _tmdbClient = tmdbClient;
        }

        public async Task<IEnumerable<Actor>> GetActorsFromExternalDataSourceAsync(string name, int iterations)
        {
            var actorlookupCache = new ConcurrentDictionary<string, byte>(StringComparer.InvariantCultureIgnoreCase);
            var movieLookupCache = new ConcurrentDictionary<string, byte>(StringComparer.InvariantCultureIgnoreCase);
            
            var results = await ProcessActor(name, iterations, actorlookupCache, movieLookupCache);

            return results;
        }

        private async Task<IEnumerable<Actor>> ProcessActor(
            string name, 
            int iterations,
            ConcurrentDictionary<string, byte> actorLookupCache,
            ConcurrentDictionary<string, byte> movieLookupCache)
        {
            if (iterations == 0 || actorLookupCache.ContainsKey(name) || actorLookupCache.Count > 10)
            {
                return Enumerable.Empty<Actor>();
            }

            var actorResults = await _tmdbClient.SearchPersonAsync(name, 0, false);
            var actorResult = actorResults.Results.FirstOrDefault();
            if (actorResult == null)
            {
                return Enumerable.Empty<Actor>();
            }

            actorLookupCache.TryAdd(actorResult.Name, 0);

            var actorMovieCredits = await _tmdbClient.GetPersonMovieCreditsAsync(actorResult.Id);
            var actor = new Actor { Name = actorResult.Name };
            var moviesActedIn = actorMovieCredits.Cast.Select(x => new { x.Title, x.ReleaseDate }).ToList();
            foreach (var movie in moviesActedIn.Take(15)) // TODO: don't only save 15 movies for the actor... doing this because inserting all the movies is SO SLOW; need to look into that.
            {
                actor.MoviesActedIn.Add(movie.Title);
            }

            IEnumerable<Actor> actors = new List<Actor> { actor };

            foreach (var movie in moviesActedIn.Take(iterations))
            {
                if (movieLookupCache.ContainsKey(movie.Title))
                {
                    continue;
                }

                movieLookupCache.TryAdd(movie.Title, 0);

                var movieResults = await _tmdbClient.SearchMovieAsync(movie.Title, 0, false, movie.ReleaseDate.GetValueOrDefault().Year);
                var movieResult = movieResults.Results.FirstOrDefault();
                if (movieResult == null)
                {
                    continue;
                }

                var movieDetail = await _tmdbClient.GetMovieAsync(movieResult.Id, MovieMethods.Credits);
                foreach (var castMember in movieDetail.Credits.Cast.Where(x => !actorLookupCache.ContainsKey(x.Name)).Take(iterations))
                {
                    actors = actors.Concat(await ProcessActor(castMember.Name, iterations - 1, actorLookupCache, movieLookupCache));
                }
            }

            return actors;
        }
    }
}