using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octogami.SixDegreesOfNetflix.Application.TMDB;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.Certifications;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.Credit;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.Find;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Timezones;
using TMDbLib.Objects.TvShows;
using Credits = TMDbLib.Objects.Movies.Credits;

namespace Octogami.SixDegreesOfNetflix.Application.Unit.Tests.TestSupport
{
    // ReSharper disable once InconsistentNaming
    public class TMDbClientFake : ITMDbClient
    {
        public TMDbClientFake()
        {
            Actors = new List<PersonT>();
            Movies = new List<MovieT>();

            Initialize();
        }

        private List<PersonT> Actors { get; set; }

        private List<MovieT> Movies { get; set; }

        public Task<SearchContainer<SearchPerson>> SearchPersonAsync(string query, int page, bool includeAdult)
        {
            var personT = Actors.FirstOrDefault(x => x.Name == query);
            if (personT == null)
            {
                return Task.FromResult(new SearchContainer<SearchPerson>() {Results = new List<SearchPerson>()});
            }

            return Task.FromResult(new SearchContainer<SearchPerson>()
            {
                Results = new List<SearchPerson> {new SearchPerson {Id = personT.Id, Name = personT.Name } }
            });
        }

        public Task<MovieCredits> GetPersonMovieCreditsAsync(int personId)
        {
            var personT = Actors.FirstOrDefault(x => x.Id == personId);
            if (personT == null)
            {
                return Task.FromResult(new MovieCredits {Cast = new List<MovieRole>()});
            }

            var movieCredits = new MovieCredits
            {
                Cast = personT.CastedIn.Select(x => new MovieRole
                {
                    Title = x.Title
                }).ToList()
            };

            return Task.FromResult(movieCredits);
        }

        public Task<SearchContainer<SearchMovie>> SearchMovieAsync(string query, int page, bool includeAdult, int year)
        {
            var movieT = Movies.FirstOrDefault(x => x.Title == query);
            if (movieT == null)
            {
                return Task.FromResult(new SearchContainer<SearchMovie> {Results = new List<SearchMovie>()});
            }

            return Task.FromResult(new SearchContainer<SearchMovie>()
            {
                Results = new List<SearchMovie>
                {
                    new SearchMovie {Id = movieT.Id, Title = movieT.Title}
                }
            });
        }

        public Task<Movie> GetMovieAsync(int movieId, MovieMethods extraMethods)
        {
            var movieT = Movies.FirstOrDefault(x => x.Id == movieId);
            if (movieT == null)
            {
                return Task.FromResult<Movie>(null);
            }

            var movie = new Movie
            {
                Id = movieT.Id,
                Title = movieT.Title
            };

            if (extraMethods.HasFlag(MovieMethods.Credits))
            {
                movie.Credits = new Credits
                {
                   Cast = movieT.Cast.Select(x => new TMDbLib.Objects.Movies.Cast
                   {
                       Id = x.Id,
                       Name =  x.Name
                   }).ToList()
                };
            }

            return Task.FromResult(movie);
        }

        private void Initialize()
        {
            var johnny = new PersonT
            {
                Id = 1,
                Name = "Johnny"
            };

            var mary = new PersonT
            {
                Id = 2,
                Name = "Mary"
            };

            var rita = new PersonT
            {
                Id = 3,
                Name = "Rita"
            };

            var billy = new PersonT
            {
                Id = 4,
                Name = "Billy"
            };

            var monsterMash = new MovieT
            {
                Id = 1,
                Title = "Monster Mash"
            };

            var cannonball = new MovieT
            {
                Id = 2,
                Title = "Cannonball"
            };

            var partyCity = new MovieT
            {
                Id = 3,
                Title = "Party City"
            };

            var hangtime = new MovieT
            {
                Id = 4,
                Title = "Hangtime"
            };

            monsterMash.AddCastMember(johnny);
            monsterMash.AddCastMember(mary);

            cannonball.AddCastMember(mary);
            cannonball.AddCastMember(rita);

            partyCity.AddCastMember(rita);
            partyCity.AddCastMember(billy);

            hangtime.AddCastMember(billy);

            Movies.AddRange(new[] {monsterMash, cannonball, partyCity, hangtime});
            Actors.AddRange(new[] {johnny, mary, rita, billy});
        }

        #region Not Implemented

        // Nothing below here needs to be implemented yet

        public Uri GetImageUrl(string size, string filePath, bool useSsl)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AccountChangeFavoriteStatusAsync(MediaType mediaType, int mediaId, bool isFavorite)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AccountChangeWatchlistStatusAsync(MediaType mediaType, int mediaId, bool isOnWatchlist)
        {
            throw new NotImplementedException();
        }

        public Task<AccountDetails> AccountGetDetailsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchMovie>> AccountGetFavoriteMoviesAsync(int page, AccountSortBy sortBy,
            SortOrder sortOrder, string language)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchTv>> AccountGetFavoriteTvAsync(int page, AccountSortBy sortBy,
            SortOrder sortOrder, string language)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<AccountList>> AccountGetListsAsync(int page, string language)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchMovie>> AccountGetMovieWatchlistAsync(int page, AccountSortBy sortBy,
            SortOrder sortOrder, string language)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchMovieWithRating>> AccountGetRatedMoviesAsync(int page, AccountSortBy sortBy,
            SortOrder sortOrder, string language)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<AccountSearchTvEpisode>> AccountGetRatedTvShowEpisodesAsync(int page,
            AccountSortBy sortBy, SortOrder sortOrder, string language)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<AccountSearchTv>> AccountGetRatedTvShowsAsync(int page, AccountSortBy sortBy,
            SortOrder sortOrder, string language)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchTv>> AccountGetTvWatchlistAsync(int page, AccountSortBy sortBy,
            SortOrder sortOrder, string language)
        {
            throw new NotImplementedException();
        }

        public Task<GuestSession> AuthenticationCreateGuestSessionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserSession> AuthenticationGetUserSessionAsync(string initialRequestToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserSession> AuthenticationGetUserSessionAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Token> AuthenticationRequestAutenticationTokenAsync()
        {
            throw new NotImplementedException();
        }

        public Task AuthenticationValidateUserTokenAsync(string initialRequestToken, string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<CertificationsContainer> GetMovieCertificationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CertificationsContainer> GetTvCertificationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<ChangesListItem>> GetChangesMoviesAsync(int page, DateTime? startDate,
            DateTime? endDate)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<ChangesListItem>> GetChangesPeopleAsync(int page, DateTime? startDate,
            DateTime? endDate)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<ChangesListItem>> GetChangesTvAsync(int page, DateTime? startDate,
            DateTime? endDate)
        {
            throw new NotImplementedException();
        }

        public Task<Collection> GetCollectionAsync(int collectionId, CollectionMethods extraMethods)
        {
            throw new NotImplementedException();
        }

        public Task<Collection> GetCollectionAsync(int collectionId, string language, CollectionMethods extraMethods)
        {
            throw new NotImplementedException();
        }

        public Task<ImagesWithId> GetCollectionImagesAsync(int collectionId, string language)
        {
            throw new NotImplementedException();
        }

        public Task<Company> GetCompanyAsync(int companyId, CompanyMethods extraMethods)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainerWithId<SearchMovie>> GetCompanyMoviesAsync(int companyId, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainerWithId<SearchMovie>> GetCompanyMoviesAsync(int companyId, string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<Credit> GetCreditsAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Credit> GetCreditsAsync(string id, string language)
        {
            throw new NotImplementedException();
        }

        public DiscoverMovie DiscoverMoviesAsync()
        {
            throw new NotImplementedException();
        }

        public DiscoverTv DiscoverTvShowsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<FindContainer> FindAsync(FindExternalSource source, string id)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainerWithId<SearchMovie>> GetGenreMoviesAsync(int genreId, int page,
            bool? includeAllMovies)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainerWithId<SearchMovie>> GetGenreMoviesAsync(int genreId, string language, int page,
            bool? includeAllMovies)
        {
            throw new NotImplementedException();
        }

        public Task<List<Genre>> GetMovieGenresAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Genre>> GetMovieGenresAsync(string language)
        {
            throw new NotImplementedException();
        }

        public Task<List<Genre>> GetTvGenresAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Genre>> GetTvGenresAsync(string language)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchMovieWithRating>> GetGuestSessionRatedMoviesAsync(int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchMovieWithRating>> GetGuestSessionRatedMoviesAsync(string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchTvShowWithRating>> GetGuestSessionRatedTvAsync(int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchTvShowWithRating>> GetGuestSessionRatedTvAsync(string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<TvEpisodeWithRating>> GetGuestSessionRatedTvEpisodesAsync(int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<TvEpisodeWithRating>> GetGuestSessionRatedTvEpisodesAsync(string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<List<Job>> GetJobsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Keyword> GetKeywordAsync(int keywordId)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainerWithId<SearchMovie>> GetKeywordMoviesAsync(int keywordId, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainerWithId<SearchMovie>> GetKeywordMoviesAsync(int keywordId, string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<GenericList> GetListAsync(string listId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetListIsMoviePresentAsync(string listId, int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ListAddMovieAsync(string listId, int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ListClearAsync(string listId)
        {
            throw new NotImplementedException();
        }

        public Task<string> ListCreateAsync(string name, string description, string language)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ListDeleteAsync(string listId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ListRemoveMovieAsync(string listId, int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<AccountState> GetMovieAccountStateAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<AlternativeTitles> GetMovieAlternativeTitlesAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<AlternativeTitles> GetMovieAlternativeTitlesAsync(int movieId, string country)
        {
            throw new NotImplementedException();
        }


        public Task<Movie> GetMovieAsync(string imdbId, MovieMethods extraMethods)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetMovieAsync(int movieId, string language, MovieMethods extraMethods)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetMovieAsync(string imdbId, string language, MovieMethods extraMethods)
        {
            throw new NotImplementedException();
        }

        public Task<List<Change>> GetMovieChangesAsync(int movieId, DateTime? startDate, DateTime? endDate)
        {
            throw new NotImplementedException();
        }

        public Task<Credits> GetMovieCreditsAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<ImagesWithId> GetMovieImagesAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<ImagesWithId> GetMovieImagesAsync(int movieId, string language)
        {
            throw new NotImplementedException();
        }

        public Task<KeywordsContainer> GetMovieKeywordsAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetMovieLatestAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainerWithId<ListResult>> GetMovieListsAsync(int movieId, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainerWithId<ListResult>> GetMovieListsAsync(int movieId, string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchMovie>> GetMovieRecommendationsAsync(int id, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchMovie>> GetMovieRecommendationsAsync(int id, string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainerWithDates<SearchMovie>> GetMovieNowPlayingListAsync(string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchMovie>> GetMoviePopularListAsync(string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<ResultContainer<ReleaseDatesContainer>> GetMovieReleaseDatesAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<Releases> GetMovieReleasesAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainerWithId<ReviewBase>> GetMovieReviewsAsync(int movieId, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainerWithId<ReviewBase>> GetMovieReviewsAsync(int movieId, string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchMovie>> GetMovieSimilarAsync(int movieId, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchMovie>> GetMovieSimilarAsync(int movieId, string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchMovie>> GetMovieTopRatedListAsync(string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<TranslationsContainer> GetMovieTranslationsAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainerWithDates<SearchMovie>> GetMovieUpcomingListAsync(string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<ResultContainer<Video>> GetMovieVideosAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MovieRemoveRatingAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MovieSetRatingAsync(int movieId, double rating)
        {
            throw new NotImplementedException();
        }

        public Task<Network> GetNetworkAsync(int networkId)
        {
            throw new NotImplementedException();
        }

        public Task<Person> GetLatestPersonAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Person> GetPersonAsync(int personId, PersonMethods extraMethods)
        {
            throw new NotImplementedException();
        }

        public Task<List<Change>> GetPersonChangesAsync(int personId, DateTime? startDate, DateTime? endDate)
        {
            throw new NotImplementedException();
        }

        public Task<ExternalIdsPerson> GetPersonExternalIdsAsync(int personId)
        {
            throw new NotImplementedException();
        }

        public Task<ProfileImages> GetPersonImagesAsync(int personId)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<PersonResult>> GetPersonListAsync(PersonListType type, int page)
        {
            throw new NotImplementedException();
        }


        public Task<MovieCredits> GetPersonMovieCreditsAsync(int personId, string language)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainerWithId<TaggedImage>> GetPersonTaggedImagesAsync(int personId, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainerWithId<TaggedImage>> GetPersonTaggedImagesAsync(int personId, string language,
            int page)
        {
            throw new NotImplementedException();
        }

        public Task<TvCredits> GetPersonTvCreditsAsync(int personId)
        {
            throw new NotImplementedException();
        }

        public Task<TvCredits> GetPersonTvCreditsAsync(int personId, string language)
        {
            throw new NotImplementedException();
        }

        public Task<Review> GetReviewAsync(string reviewId)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchCollection>> SearchCollectionAsync(string query, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchCollection>> SearchCollectionAsync(string query, string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchCompany>> SearchCompanyAsync(string query, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchKeyword>> SearchKeywordAsync(string query, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchList>> SearchListAsync(string query, int page, bool includeAdult)
        {
            throw new NotImplementedException();
        }


        public Task<SearchContainer<SearchMovie>> SearchMovieAsync(string query, string language, int page,
            bool includeAdult, int year)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchBase>> SearchMultiAsync(string query, int page, bool includeAdult, int year)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchBase>> SearchMultiAsync(string query, string language, int page,
            bool includeAdult, int year)
        {
            throw new NotImplementedException();
        }


        public Task<SearchContainer<SearchTv>> SearchTvShowAsync(string query, int page)
        {
            throw new NotImplementedException();
        }

        public Task<Timezones> GetTimezonesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TvEpisodeAccountState> GetTvEpisodeAccountStateAsync(int tvShowId, int seasonNumber,
            int episodeNumber)
        {
            throw new NotImplementedException();
        }

        public Task<TvEpisode> GetTvEpisodeAsync(int tvShowId, int seasonNumber, int episodeNumber,
            TvEpisodeMethods extraMethods,
            string language)
        {
            throw new NotImplementedException();
        }

        public Task<ChangesContainer> GetTvEpisodeChangesAsync(int episodeId)
        {
            throw new NotImplementedException();
        }

        public Task<CreditsWithGuestStars> GetTvEpisodeCreditsAsync(int tvShowId, int seasonNumber, int episodeNumber,
            string language)
        {
            throw new NotImplementedException();
        }

        public Task<ExternalIdsTvEpisode> GetTvEpisodeExternalIdsAsync(int tvShowId, int seasonNumber,
            int episodeNumber)
        {
            throw new NotImplementedException();
        }

        public Task<StillImages> GetTvEpisodeImagesAsync(int tvShowId, int seasonNumber, int episodeNumber,
            string language)
        {
            throw new NotImplementedException();
        }

        public Task<ResultContainer<Video>> GetTvEpisodeVideosAsync(int tvShowId, int seasonNumber, int episodeNumber)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TvEpisodeRemoveRatingAsync(int tvShowId, int seasonNumber, int episodeNumber)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TvEpisodeSetRatingAsync(int tvShowId, int seasonNumber, int episodeNumber, double rating)
        {
            throw new NotImplementedException();
        }

        public Task<ResultContainer<TvEpisodeAccountStateWithNumber>> GetTvSeasonAccountStateAsync(int tvShowId,
            int seasonNumber)
        {
            throw new NotImplementedException();
        }

        public Task<TvSeason> GetTvSeasonAsync(int tvShowId, int seasonNumber, TvSeasonMethods extraMethods,
            string language)
        {
            throw new NotImplementedException();
        }

        public Task<ChangesContainer> GetTvSeasonChangesAsync(int seasonId)
        {
            throw new NotImplementedException();
        }

        public Task<TMDbLib.Objects.TvShows.Credits> GetTvSeasonCreditsAsync(int tvShowId, int seasonNumber,
            string language)
        {
            throw new NotImplementedException();
        }

        public Task<ExternalIdsTvSeason> GetTvSeasonExternalIdsAsync(int tvShowId, int seasonNumber)
        {
            throw new NotImplementedException();
        }

        public Task<PosterImages> GetTvSeasonImagesAsync(int tvShowId, int seasonNumber, string language)
        {
            throw new NotImplementedException();
        }

        public Task<ResultContainer<Video>> GetTvSeasonVideosAsync(int tvShowId, int seasonNumber, string language)
        {
            throw new NotImplementedException();
        }

        public Task<TvShow> GetLatestTvShowAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AccountState> GetTvShowAccountStateAsync(int tvShowId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultContainer<AlternativeTitle>> GetTvShowAlternativeTitlesAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TvShow> GetTvShowAsync(int id, TvShowMethods extraMethods, string language)
        {
            throw new NotImplementedException();
        }

        public Task<ChangesContainer> GetTvShowChangesAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultContainer<ContentRating>> GetTvShowContentRatingsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TMDbLib.Objects.TvShows.Credits> GetTvShowCreditsAsync(int id, string language)
        {
            throw new NotImplementedException();
        }

        public Task<ExternalIdsTvShow> GetTvShowExternalIdsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ImagesWithId> GetTvShowImagesAsync(int id, string language)
        {
            throw new NotImplementedException();
        }

        public Task<ResultContainer<Keyword>> GetTvShowKeywordsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchTv>> GetTvShowListAsync(TvShowListType list, int page, string timezone)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchTv>> GetTvShowListAsync(TvShowListType list, string language, int page,
            string timezone)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchTv>> GetTvShowPopularAsync(int page, string language)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchTv>> GetTvShowSimilarAsync(int id, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchTv>> GetTvShowSimilarAsync(int id, string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchTv>> GetTvShowRecommendationsAsync(int id, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchTv>> GetTvShowRecommendationsAsync(int id, string language, int page)
        {
            throw new NotImplementedException();
        }

        public Task<SearchContainer<SearchTv>> GetTvShowTopRatedAsync(int page, string language)
        {
            throw new NotImplementedException();
        }

        public Task<TranslationsContainerTv> GetTvShowTranslationsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultContainer<Video>> GetTvShowVideosAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TvShowRemoveRatingAsync(int tvShowId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TvShowSetRatingAsync(int tvShowId, double rating)
        {
            throw new NotImplementedException();
        }

        public AccountDetails ActiveAccount { get; }
        public string ApiKey { get; }

        #endregion
    }

    internal class PersonT
    {
        public PersonT()
        {
            CastedIn = new List<MovieT>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<MovieT> CastedIn { get; set; }
    }

    internal class MovieT
    {
        public MovieT()
        {
            Cast = new List<PersonT>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public List<PersonT> Cast { get; }

        public void AddCastMember(PersonT person)
        {
            Cast.Add(person);
            person.CastedIn.Add(this);
        }
    }
}