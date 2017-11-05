using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbLib.Client;
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

namespace Octogami.SixDegreesOfNetflix.Application.TMDB
{
    // ReSharper disable once InconsistentNaming
    public class TMDbClientWrapper : ITMDbClient
    {
        private readonly TMDbClient _innerDbClient;

        public TMDbClientWrapper(TMDbClient tmDbClient)
        {
            _innerDbClient = tmDbClient;
        }

        public Uri GetImageUrl(string size, string filePath, bool useSsl)
        {
            return _innerDbClient.GetImageUrl(size, filePath, useSsl);
        }

        public Task<bool> AccountChangeFavoriteStatusAsync(MediaType mediaType, int mediaId, bool isFavorite)
        {
            return _innerDbClient.AccountChangeFavoriteStatusAsync(mediaType, mediaId, isFavorite);
        }

        public Task<bool> AccountChangeWatchlistStatusAsync(MediaType mediaType, int mediaId, bool isOnWatchlist)
        {
            return _innerDbClient.AccountChangeWatchlistStatusAsync(mediaType, mediaId, isOnWatchlist);
        }

        public Task<AccountDetails> AccountGetDetailsAsync()
        {
            return _innerDbClient.AccountGetDetailsAsync();
        }

        public Task<SearchContainer<SearchMovie>> AccountGetFavoriteMoviesAsync(int page, AccountSortBy sortBy,
            SortOrder sortOrder, string language)
        {
            return _innerDbClient.AccountGetFavoriteMoviesAsync(page, sortBy, sortOrder, language);
        }

        public Task<SearchContainer<SearchTv>> AccountGetFavoriteTvAsync(int page, AccountSortBy sortBy,
            SortOrder sortOrder, string language)
        {
            return _innerDbClient.AccountGetFavoriteTvAsync(page, sortBy, sortOrder, language);
        }

        public Task<SearchContainer<AccountList>> AccountGetListsAsync(int page, string language)
        {
            return _innerDbClient.AccountGetListsAsync(page, language);
        }

        public Task<SearchContainer<SearchMovie>> AccountGetMovieWatchlistAsync(int page, AccountSortBy sortBy,
            SortOrder sortOrder, string language)
        {
            return _innerDbClient.AccountGetMovieWatchlistAsync(page, sortBy, sortOrder, language);
        }

        public Task<SearchContainer<SearchMovieWithRating>> AccountGetRatedMoviesAsync(int page, AccountSortBy sortBy,
            SortOrder sortOrder, string language)
        {
            return _innerDbClient.AccountGetRatedMoviesAsync(page, sortBy, sortOrder, language);
        }

        public Task<SearchContainer<AccountSearchTvEpisode>> AccountGetRatedTvShowEpisodesAsync(int page,
            AccountSortBy sortBy, SortOrder sortOrder, string language)
        {
            return _innerDbClient.AccountGetRatedTvShowEpisodesAsync(page, sortBy, sortOrder, language);
        }

        public Task<SearchContainer<AccountSearchTv>> AccountGetRatedTvShowsAsync(int page, AccountSortBy sortBy,
            SortOrder sortOrder, string language)
        {
            return _innerDbClient.AccountGetRatedTvShowsAsync(page, sortBy, sortOrder, language);
        }

        public Task<SearchContainer<SearchTv>> AccountGetTvWatchlistAsync(int page, AccountSortBy sortBy,
            SortOrder sortOrder, string language)
        {
            return _innerDbClient.AccountGetTvWatchlistAsync(page, sortBy, sortOrder, language);
        }

        public Task<GuestSession> AuthenticationCreateGuestSessionAsync()
        {
            return _innerDbClient.AuthenticationCreateGuestSessionAsync();
        }

        public Task<UserSession> AuthenticationGetUserSessionAsync(string initialRequestToken)
        {
            return _innerDbClient.AuthenticationGetUserSessionAsync(initialRequestToken);
        }

        public Task<UserSession> AuthenticationGetUserSessionAsync(string username, string password)
        {
            return _innerDbClient.AuthenticationGetUserSessionAsync(username, password);
        }

        public Task<Token> AuthenticationRequestAutenticationTokenAsync()
        {
            return _innerDbClient.AuthenticationRequestAutenticationTokenAsync();
        }

        public Task AuthenticationValidateUserTokenAsync(string initialRequestToken, string username, string password)
        {
            return _innerDbClient.AuthenticationValidateUserTokenAsync(initialRequestToken, username, password);
        }

        public Task<CertificationsContainer> GetMovieCertificationsAsync()
        {
            return _innerDbClient.GetMovieCertificationsAsync();
        }

        public Task<CertificationsContainer> GetTvCertificationsAsync()
        {
            return _innerDbClient.GetTvCertificationsAsync();
        }

        public Task<SearchContainer<ChangesListItem>> GetChangesMoviesAsync(int page, DateTime? startDate,
            DateTime? endDate)
        {
            return _innerDbClient.GetChangesMoviesAsync(page, startDate, endDate);
        }

        public Task<SearchContainer<ChangesListItem>> GetChangesPeopleAsync(int page, DateTime? startDate,
            DateTime? endDate)
        {
            return _innerDbClient.GetChangesPeopleAsync(page, startDate, endDate);
        }

        public Task<SearchContainer<ChangesListItem>> GetChangesTvAsync(int page, DateTime? startDate,
            DateTime? endDate)
        {
            return _innerDbClient.GetChangesTvAsync(page, startDate, endDate);
        }

        public Task<Collection> GetCollectionAsync(int collectionId, CollectionMethods extraMethods)
        {
            return _innerDbClient.GetCollectionAsync(collectionId, extraMethods);
        }

        public Task<Collection> GetCollectionAsync(int collectionId, string language, CollectionMethods extraMethods)
        {
            return _innerDbClient.GetCollectionAsync(collectionId, language, extraMethods);
        }

        public Task<ImagesWithId> GetCollectionImagesAsync(int collectionId, string language)
        {
            return _innerDbClient.GetCollectionImagesAsync(collectionId, language);
        }

        public Task<Company> GetCompanyAsync(int companyId, CompanyMethods extraMethods)
        {
            return _innerDbClient.GetCompanyAsync(companyId, extraMethods);
        }

        public Task<SearchContainerWithId<SearchMovie>> GetCompanyMoviesAsync(int companyId, int page)
        {
            return _innerDbClient.GetCompanyMoviesAsync(companyId, page);
        }

        public Task<SearchContainerWithId<SearchMovie>> GetCompanyMoviesAsync(int companyId, string language, int page)
        {
            return _innerDbClient.GetCompanyMoviesAsync(companyId, language, page);
        }

        public Task<Credit> GetCreditsAsync(string id)
        {
            return _innerDbClient.GetCreditsAsync(id);
        }

        public Task<Credit> GetCreditsAsync(string id, string language)
        {
            return _innerDbClient.GetCreditsAsync(id, language);
        }

        public DiscoverMovie DiscoverMoviesAsync()
        {
            return _innerDbClient.DiscoverMoviesAsync();
        }

        public DiscoverTv DiscoverTvShowsAsync()
        {
            return _innerDbClient.DiscoverTvShowsAsync();
        }

        public Task<FindContainer> FindAsync(FindExternalSource source, string id)
        {
            return _innerDbClient.FindAsync(source, id);
        }

        public Task<SearchContainerWithId<SearchMovie>> GetGenreMoviesAsync(int genreId, int page,
            bool? includeAllMovies)
        {
            return _innerDbClient.GetGenreMoviesAsync(genreId, page, includeAllMovies);
        }

        public Task<SearchContainerWithId<SearchMovie>> GetGenreMoviesAsync(int genreId, string language, int page,
            bool? includeAllMovies)
        {
            return _innerDbClient.GetGenreMoviesAsync(genreId, language, page, includeAllMovies);
        }

        public Task<List<Genre>> GetMovieGenresAsync()
        {
            return _innerDbClient.GetMovieGenresAsync();
        }

        public Task<List<Genre>> GetMovieGenresAsync(string language)
        {
            return _innerDbClient.GetMovieGenresAsync(language);
        }

        public Task<List<Genre>> GetTvGenresAsync()
        {
            return _innerDbClient.GetTvGenresAsync();
        }

        public Task<List<Genre>> GetTvGenresAsync(string language)
        {
            return _innerDbClient.GetTvGenresAsync(language);
        }

        public Task<SearchContainer<SearchMovieWithRating>> GetGuestSessionRatedMoviesAsync(int page)
        {
            return _innerDbClient.GetGuestSessionRatedMoviesAsync(page);
        }

        public Task<SearchContainer<SearchMovieWithRating>> GetGuestSessionRatedMoviesAsync(string language, int page)
        {
            return _innerDbClient.GetGuestSessionRatedMoviesAsync(language, page);
        }

        public Task<SearchContainer<SearchTvShowWithRating>> GetGuestSessionRatedTvAsync(int page)
        {
            return _innerDbClient.GetGuestSessionRatedTvAsync(page);
        }

        public Task<SearchContainer<SearchTvShowWithRating>> GetGuestSessionRatedTvAsync(string language, int page)
        {
            return _innerDbClient.GetGuestSessionRatedTvAsync(language, page);
        }

        public Task<SearchContainer<TvEpisodeWithRating>> GetGuestSessionRatedTvEpisodesAsync(int page)
        {
            return _innerDbClient.GetGuestSessionRatedTvEpisodesAsync(page);
        }

        public Task<SearchContainer<TvEpisodeWithRating>> GetGuestSessionRatedTvEpisodesAsync(string language, int page)
        {
            return _innerDbClient.GetGuestSessionRatedTvEpisodesAsync(language, page);
        }

        public Task<List<Job>> GetJobsAsync()
        {
            return _innerDbClient.GetJobsAsync();
        }

        public Task<Keyword> GetKeywordAsync(int keywordId)
        {
            return _innerDbClient.GetKeywordAsync(keywordId);
        }

        public Task<SearchContainerWithId<SearchMovie>> GetKeywordMoviesAsync(int keywordId, int page)
        {
            return _innerDbClient.GetKeywordMoviesAsync(keywordId, page);
        }

        public Task<SearchContainerWithId<SearchMovie>> GetKeywordMoviesAsync(int keywordId, string language, int page)
        {
            return _innerDbClient.GetKeywordMoviesAsync(keywordId, language, page);
        }

        public Task<GenericList> GetListAsync(string listId)
        {
            return _innerDbClient.GetListAsync(listId);
        }

        public Task<bool> GetListIsMoviePresentAsync(string listId, int movieId)
        {
            return _innerDbClient.GetListIsMoviePresentAsync(listId, movieId);
        }

        public Task<bool> ListAddMovieAsync(string listId, int movieId)
        {
            return _innerDbClient.ListAddMovieAsync(listId, movieId);
        }

        public Task<bool> ListClearAsync(string listId)
        {
            return _innerDbClient.ListClearAsync(listId);
        }

        public Task<string> ListCreateAsync(string name, string description, string language)
        {
            return _innerDbClient.ListCreateAsync(name, description, language);
        }

        public Task<bool> ListDeleteAsync(string listId)
        {
            return _innerDbClient.ListDeleteAsync(listId);
        }

        public Task<bool> ListRemoveMovieAsync(string listId, int movieId)
        {
            return _innerDbClient.ListRemoveMovieAsync(listId, movieId);
        }

        public Task<AccountState> GetMovieAccountStateAsync(int movieId)
        {
            return _innerDbClient.GetMovieAccountStateAsync(movieId);
        }

        public Task<AlternativeTitles> GetMovieAlternativeTitlesAsync(int movieId)
        {
            return _innerDbClient.GetMovieAlternativeTitlesAsync(movieId);
        }

        public Task<AlternativeTitles> GetMovieAlternativeTitlesAsync(int movieId, string country)
        {
            return _innerDbClient.GetMovieAlternativeTitlesAsync(movieId, country);
        }

        public Task<Movie> GetMovieAsync(int movieId, MovieMethods extraMethods)
        {
            return _innerDbClient.GetMovieAsync(movieId, extraMethods);
        }

        public Task<Movie> GetMovieAsync(string imdbId, MovieMethods extraMethods)
        {
            return _innerDbClient.GetMovieAsync(imdbId, extraMethods);
        }

        public Task<Movie> GetMovieAsync(int movieId, string language, MovieMethods extraMethods)
        {
            return _innerDbClient.GetMovieAsync(movieId, language, extraMethods);
        }

        public Task<Movie> GetMovieAsync(string imdbId, string language, MovieMethods extraMethods)
        {
            return _innerDbClient.GetMovieAsync(imdbId, language, extraMethods);
        }

        public Task<List<Change>> GetMovieChangesAsync(int movieId, DateTime? startDate, DateTime? endDate)
        {
            return _innerDbClient.GetMovieChangesAsync(movieId, startDate, endDate);
        }

        public Task<Credits> GetMovieCreditsAsync(int movieId)
        {
            return _innerDbClient.GetMovieCreditsAsync(movieId);
        }

        public Task<ImagesWithId> GetMovieImagesAsync(int movieId)
        {
            return _innerDbClient.GetMovieImagesAsync(movieId);
        }

        public Task<ImagesWithId> GetMovieImagesAsync(int movieId, string language)
        {
            return _innerDbClient.GetMovieImagesAsync(movieId, language);
        }

        public Task<KeywordsContainer> GetMovieKeywordsAsync(int movieId)
        {
            return _innerDbClient.GetMovieKeywordsAsync(movieId);
        }

        public Task<Movie> GetMovieLatestAsync()
        {
            return _innerDbClient.GetMovieLatestAsync();
        }

        public Task<SearchContainerWithId<ListResult>> GetMovieListsAsync(int movieId, int page)
        {
            return _innerDbClient.GetMovieListsAsync(movieId, page);
        }

        public Task<SearchContainerWithId<ListResult>> GetMovieListsAsync(int movieId, string language, int page)
        {
            return _innerDbClient.GetMovieListsAsync(movieId, language, page);
        }

        public Task<SearchContainer<SearchMovie>> GetMovieRecommendationsAsync(int id, int page)
        {
            return _innerDbClient.GetMovieRecommendationsAsync(id, page);
        }

        public Task<SearchContainer<SearchMovie>> GetMovieRecommendationsAsync(int id, string language, int page)
        {
            return _innerDbClient.GetMovieRecommendationsAsync(id, language, page);
        }

        public Task<SearchContainerWithDates<SearchMovie>> GetMovieNowPlayingListAsync(string language, int page)
        {
            return _innerDbClient.GetMovieNowPlayingListAsync(language, page);
        }

        public Task<SearchContainer<SearchMovie>> GetMoviePopularListAsync(string language, int page)
        {
            return _innerDbClient.GetMoviePopularListAsync(language, page);
        }

        public Task<ResultContainer<ReleaseDatesContainer>> GetMovieReleaseDatesAsync(int movieId)
        {
            return _innerDbClient.GetMovieReleaseDatesAsync(movieId);
        }

        public Task<Releases> GetMovieReleasesAsync(int movieId)
        {
            return _innerDbClient.GetMovieReleasesAsync(movieId);
        }

        public Task<SearchContainerWithId<ReviewBase>> GetMovieReviewsAsync(int movieId, int page)
        {
            return _innerDbClient.GetMovieReviewsAsync(movieId, page);
        }

        public Task<SearchContainerWithId<ReviewBase>> GetMovieReviewsAsync(int movieId, string language, int page)
        {
            return _innerDbClient.GetMovieReviewsAsync(movieId, language, page);
        }

        public Task<SearchContainer<SearchMovie>> GetMovieSimilarAsync(int movieId, int page)
        {
            return _innerDbClient.GetMovieSimilarAsync(movieId, page);
        }

        public Task<SearchContainer<SearchMovie>> GetMovieSimilarAsync(int movieId, string language, int page)
        {
            return _innerDbClient.GetMovieSimilarAsync(movieId, language, page);
        }

        public Task<SearchContainer<SearchMovie>> GetMovieTopRatedListAsync(string language, int page)
        {
            return _innerDbClient.GetMovieTopRatedListAsync(language, page);
        }

        public Task<TranslationsContainer> GetMovieTranslationsAsync(int movieId)
        {
            return _innerDbClient.GetMovieTranslationsAsync(movieId);
        }

        public Task<SearchContainerWithDates<SearchMovie>> GetMovieUpcomingListAsync(string language, int page)
        {
            return _innerDbClient.GetMovieUpcomingListAsync(language, page);
        }

        public Task<ResultContainer<Video>> GetMovieVideosAsync(int movieId)
        {
            return _innerDbClient.GetMovieVideosAsync(movieId);
        }

        public Task<bool> MovieRemoveRatingAsync(int movieId)
        {
            return _innerDbClient.MovieRemoveRatingAsync(movieId);
        }

        public Task<bool> MovieSetRatingAsync(int movieId, double rating)
        {
            return _innerDbClient.MovieSetRatingAsync(movieId, rating);
        }

        public Task<Network> GetNetworkAsync(int networkId)
        {
            return _innerDbClient.GetNetworkAsync(networkId);
        }

        public Task<Person> GetLatestPersonAsync()
        {
            return _innerDbClient.GetLatestPersonAsync();
        }

        public Task<Person> GetPersonAsync(int personId, PersonMethods extraMethods)
        {
            return _innerDbClient.GetPersonAsync(personId, extraMethods);
        }

        public Task<List<Change>> GetPersonChangesAsync(int personId, DateTime? startDate, DateTime? endDate)
        {
            return _innerDbClient.GetPersonChangesAsync(personId, startDate, endDate);
        }

        public Task<ExternalIdsPerson> GetPersonExternalIdsAsync(int personId)
        {
            return _innerDbClient.GetPersonExternalIdsAsync(personId);
        }

        public Task<ProfileImages> GetPersonImagesAsync(int personId)
        {
            return _innerDbClient.GetPersonImagesAsync(personId);
        }

        public Task<SearchContainer<PersonResult>> GetPersonListAsync(PersonListType type, int page)
        {
            return _innerDbClient.GetPersonListAsync(type, page);
        }

        public Task<MovieCredits> GetPersonMovieCreditsAsync(int personId)
        {
            return _innerDbClient.GetPersonMovieCreditsAsync(personId);
        }

        public Task<MovieCredits> GetPersonMovieCreditsAsync(int personId, string language)
        {
            return _innerDbClient.GetPersonMovieCreditsAsync(personId, language);
        }

        public Task<SearchContainerWithId<TaggedImage>> GetPersonTaggedImagesAsync(int personId, int page)
        {
            return _innerDbClient.GetPersonTaggedImagesAsync(personId, page);
        }

        public Task<SearchContainerWithId<TaggedImage>> GetPersonTaggedImagesAsync(int personId, string language,
            int page)
        {
            return _innerDbClient.GetPersonTaggedImagesAsync(personId, language, page);
        }

        public Task<TvCredits> GetPersonTvCreditsAsync(int personId)
        {
            return _innerDbClient.GetPersonTvCreditsAsync(personId);
        }

        public Task<TvCredits> GetPersonTvCreditsAsync(int personId, string language)
        {
            return _innerDbClient.GetPersonTvCreditsAsync(personId, language);
        }

        public Task<Review> GetReviewAsync(string reviewId)
        {
            return _innerDbClient.GetReviewAsync(reviewId);
        }

        public Task<SearchContainer<SearchCollection>> SearchCollectionAsync(string query, int page)
        {
            return _innerDbClient.SearchCollectionAsync(query, page);
        }

        public Task<SearchContainer<SearchCollection>> SearchCollectionAsync(string query, string language, int page)
        {
            return _innerDbClient.SearchCollectionAsync(query, language, page);
        }

        public Task<SearchContainer<SearchCompany>> SearchCompanyAsync(string query, int page)
        {
            return _innerDbClient.SearchCompanyAsync(query, page);
        }

        public Task<SearchContainer<SearchKeyword>> SearchKeywordAsync(string query, int page)
        {
            return _innerDbClient.SearchKeywordAsync(query, page);
        }

        public Task<SearchContainer<SearchList>> SearchListAsync(string query, int page, bool includeAdult)
        {
            return _innerDbClient.SearchListAsync(query, page, includeAdult);
        }

        public Task<SearchContainer<SearchMovie>> SearchMovieAsync(string query, int page, bool includeAdult, int year)
        {
            return _innerDbClient.SearchMovieAsync(query, page, includeAdult, year);
        }

        public Task<SearchContainer<SearchMovie>> SearchMovieAsync(string query, string language, int page,
            bool includeAdult, int year)
        {
            return _innerDbClient.SearchMovieAsync(query, language, page, includeAdult, year);
        }

        public Task<SearchContainer<SearchBase>> SearchMultiAsync(string query, int page, bool includeAdult, int year)
        {
            return _innerDbClient.SearchMultiAsync(query, page, includeAdult, year);
        }

        public Task<SearchContainer<SearchBase>> SearchMultiAsync(string query, string language, int page,
            bool includeAdult, int year)
        {
            return _innerDbClient.SearchMultiAsync(query, language, page, includeAdult, year);
        }

        public Task<SearchContainer<SearchPerson>> SearchPersonAsync(string query, int page, bool includeAdult)
        {
            return _innerDbClient.SearchPersonAsync(query, page, includeAdult);
        }

        public Task<SearchContainer<SearchTv>> SearchTvShowAsync(string query, int page)
        {
            return _innerDbClient.SearchTvShowAsync(query, page);
        }

        public Task<Timezones> GetTimezonesAsync()
        {
            return _innerDbClient.GetTimezonesAsync();
        }

        public Task<TvEpisodeAccountState> GetTvEpisodeAccountStateAsync(int tvShowId, int seasonNumber,
            int episodeNumber)
        {
            return _innerDbClient.GetTvEpisodeAccountStateAsync(tvShowId, seasonNumber, episodeNumber);
        }

        public Task<TvEpisode> GetTvEpisodeAsync(int tvShowId, int seasonNumber, int episodeNumber,
            TvEpisodeMethods extraMethods,
            string language)
        {
            return _innerDbClient.GetTvEpisodeAsync(tvShowId, seasonNumber, episodeNumber, extraMethods, language);
        }

        public Task<ChangesContainer> GetTvEpisodeChangesAsync(int episodeId)
        {
            return _innerDbClient.GetTvEpisodeChangesAsync(episodeId);
        }

        public Task<CreditsWithGuestStars> GetTvEpisodeCreditsAsync(int tvShowId, int seasonNumber, int episodeNumber,
            string language)
        {
            return _innerDbClient.GetTvEpisodeCreditsAsync(tvShowId, seasonNumber, episodeNumber, language);
        }

        public Task<ExternalIdsTvEpisode> GetTvEpisodeExternalIdsAsync(int tvShowId, int seasonNumber,
            int episodeNumber)
        {
            return _innerDbClient.GetTvEpisodeExternalIdsAsync(tvShowId, seasonNumber, episodeNumber);
        }

        public Task<StillImages> GetTvEpisodeImagesAsync(int tvShowId, int seasonNumber, int episodeNumber,
            string language)
        {
            return _innerDbClient.GetTvEpisodeImagesAsync(tvShowId, seasonNumber, episodeNumber, language);
        }

        public Task<ResultContainer<Video>> GetTvEpisodeVideosAsync(int tvShowId, int seasonNumber, int episodeNumber)
        {
            return _innerDbClient.GetTvEpisodeVideosAsync(tvShowId, seasonNumber, episodeNumber);
        }

        public Task<bool> TvEpisodeRemoveRatingAsync(int tvShowId, int seasonNumber, int episodeNumber)
        {
            return _innerDbClient.TvEpisodeRemoveRatingAsync(tvShowId, seasonNumber, episodeNumber);
        }

        public Task<bool> TvEpisodeSetRatingAsync(int tvShowId, int seasonNumber, int episodeNumber, double rating)
        {
            return _innerDbClient.TvEpisodeSetRatingAsync(tvShowId, seasonNumber, episodeNumber, rating);
        }

        public Task<ResultContainer<TvEpisodeAccountStateWithNumber>> GetTvSeasonAccountStateAsync(int tvShowId,
            int seasonNumber)
        {
            return _innerDbClient.GetTvSeasonAccountStateAsync(tvShowId, seasonNumber);
        }

        public Task<TvSeason> GetTvSeasonAsync(int tvShowId, int seasonNumber, TvSeasonMethods extraMethods,
            string language)
        {
            return _innerDbClient.GetTvSeasonAsync(tvShowId, seasonNumber, extraMethods, language);
        }

        public Task<ChangesContainer> GetTvSeasonChangesAsync(int seasonId)
        {
            return _innerDbClient.GetTvSeasonChangesAsync(seasonId);
        }

        public Task<TMDbLib.Objects.TvShows.Credits> GetTvSeasonCreditsAsync(int tvShowId, int seasonNumber,
            string language)
        {
            return _innerDbClient.GetTvSeasonCreditsAsync(tvShowId, seasonNumber, language);
        }

        public Task<ExternalIdsTvSeason> GetTvSeasonExternalIdsAsync(int tvShowId, int seasonNumber)
        {
            return _innerDbClient.GetTvSeasonExternalIdsAsync(tvShowId, seasonNumber);
        }

        public Task<PosterImages> GetTvSeasonImagesAsync(int tvShowId, int seasonNumber, string language)
        {
            return _innerDbClient.GetTvSeasonImagesAsync(tvShowId, seasonNumber, language);
        }

        public Task<ResultContainer<Video>> GetTvSeasonVideosAsync(int tvShowId, int seasonNumber, string language)
        {
            return _innerDbClient.GetTvSeasonVideosAsync(tvShowId, seasonNumber, language);
        }

        public Task<TvShow> GetLatestTvShowAsync()
        {
            return _innerDbClient.GetLatestTvShowAsync();
        }

        public Task<AccountState> GetTvShowAccountStateAsync(int tvShowId)
        {
            return _innerDbClient.GetTvShowAccountStateAsync(tvShowId);
        }

        public Task<ResultContainer<AlternativeTitle>> GetTvShowAlternativeTitlesAsync(int id)
        {
            return _innerDbClient.GetTvShowAlternativeTitlesAsync(id);
        }

        public Task<TvShow> GetTvShowAsync(int id, TvShowMethods extraMethods, string language)
        {
            return _innerDbClient.GetTvShowAsync(id, extraMethods, language);
        }

        public Task<ChangesContainer> GetTvShowChangesAsync(int id)
        {
            return _innerDbClient.GetTvShowChangesAsync(id);
        }

        public Task<ResultContainer<ContentRating>> GetTvShowContentRatingsAsync(int id)
        {
            return _innerDbClient.GetTvShowContentRatingsAsync(id);
        }

        public Task<TMDbLib.Objects.TvShows.Credits> GetTvShowCreditsAsync(int id, string language)
        {
            return _innerDbClient.GetTvShowCreditsAsync(id, language);
        }

        public Task<ExternalIdsTvShow> GetTvShowExternalIdsAsync(int id)
        {
            return _innerDbClient.GetTvShowExternalIdsAsync(id);
        }

        public Task<ImagesWithId> GetTvShowImagesAsync(int id, string language)
        {
            return _innerDbClient.GetTvShowImagesAsync(id, language);
        }

        public Task<ResultContainer<Keyword>> GetTvShowKeywordsAsync(int id)
        {
            return _innerDbClient.GetTvShowKeywordsAsync(id);
        }

        public Task<SearchContainer<SearchTv>> GetTvShowListAsync(TvShowListType list, int page, string timezone)
        {
            return _innerDbClient.GetTvShowListAsync(list, page, timezone);
        }

        public Task<SearchContainer<SearchTv>> GetTvShowListAsync(TvShowListType list, string language, int page,
            string timezone)
        {
            return _innerDbClient.GetTvShowListAsync(list, language, page, timezone);
        }

        public Task<SearchContainer<SearchTv>> GetTvShowPopularAsync(int page, string language)
        {
            return _innerDbClient.GetTvShowPopularAsync(page, language);
        }

        public Task<SearchContainer<SearchTv>> GetTvShowSimilarAsync(int id, int page)
        {
            return _innerDbClient.GetTvShowSimilarAsync(id, page);
        }

        public Task<SearchContainer<SearchTv>> GetTvShowSimilarAsync(int id, string language, int page)
        {
            return _innerDbClient.GetTvShowSimilarAsync(id, language, page);
        }

        public Task<SearchContainer<SearchTv>> GetTvShowRecommendationsAsync(int id, int page)
        {
            return _innerDbClient.GetTvShowRecommendationsAsync(id, page);
        }

        public Task<SearchContainer<SearchTv>> GetTvShowRecommendationsAsync(int id, string language, int page)
        {
            return _innerDbClient.GetTvShowRecommendationsAsync(id, language, page);
        }

        public Task<SearchContainer<SearchTv>> GetTvShowTopRatedAsync(int page, string language)
        {
            return _innerDbClient.GetTvShowTopRatedAsync(page, language);
        }

        public Task<TranslationsContainerTv> GetTvShowTranslationsAsync(int id)
        {
            return _innerDbClient.GetTvShowTranslationsAsync(id);
        }

        public Task<ResultContainer<Video>> GetTvShowVideosAsync(int id)
        {
            return _innerDbClient.GetTvShowVideosAsync(id);
        }

        public Task<bool> TvShowRemoveRatingAsync(int tvShowId)
        {
            return _innerDbClient.TvShowRemoveRatingAsync(tvShowId);
        }

        public Task<bool> TvShowSetRatingAsync(int tvShowId, double rating)
        {
            return _innerDbClient.TvShowSetRatingAsync(tvShowId, rating);
        }

        public AccountDetails ActiveAccount => _innerDbClient.ActiveAccount;

        public string ApiKey => _innerDbClient.ApiKey;
    }
}