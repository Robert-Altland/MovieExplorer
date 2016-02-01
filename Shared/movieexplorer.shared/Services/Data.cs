using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

using com.interactiverobert.prototypes.movieexplorer.shared.Contracts;
using com.interactiverobert.prototypes.movieexplorer.shared.Resources;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Configuration;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Movie;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Video;

namespace com.interactiverobert.prototypes.movieexplorer.shared.Services
{
	public class Data
	{
		#region Private fields
		private ICache cache;
		private IHttpClientFactory httpClientFactory;
		private ConfigurationResponse configuration;
		private GetMoviesResponse topRatedMovies;
		private GetMoviesResponse popularMovies;
		private GetMoviesResponse nowPlayingMovies;
		private GetMoviesResponse upcomingMovies;
		private List<Movie> favorites;
		private List<MovieCategory> categories;
		#endregion

		#region Singleton
		private static Data current;
        public static Data Current
		{
			get
			{
				if (current == null)
					current = new Data (DependencyManager.HttpClientFactory, DependencyManager.Cache);
				return current;
			}
			set { current = value; }
		}
		#endregion

		#region Constructor
		public Data (IHttpClientFactory httpClientFactory, ICache cache) {
			this.httpClientFactory = httpClientFactory;
			this.cache = cache;
		}
		#endregion

		#region Private methods
		private string getCacheKey (string urlString) {
			var regex = new Regex ("[^a-z0-9]", RegexOptions.IgnoreCase);
			var result = regex.Replace (urlString, "_");
			result = result.ToLower ();
			return result;
		}
		#endregion

		#region Public methods
		public Task<ConfigurationResponse> GetConfigurationAsync () {
			var tcs = new TaskCompletionSource<ConfigurationResponse> ();
			Task.Factory.StartNew (async () => {
				var urlString = String.Format (SharedConstants.GetConfigurationUriFormatString, SharedConstants.ApiKey);
				var cacheKey = this.getCacheKey (urlString);
				if (this.configuration != null) {
					tcs.TrySetResult (this.configuration);
				} else {
					if (cache.ContainsKey(cacheKey)) {
						var response = cache.Get<ConfigurationResponse>(cacheKey);
						this.configuration = response;
						tcs.TrySetResult (this.configuration);
					} else {
						var client = this.httpClientFactory.Create ();
						var response = await client.GetAsync (urlString);
						var json = await response.Content.ReadAsStringAsync ();
						var result = JsonConvert.DeserializeObject<ConfigurationResponse> (json);
						this.configuration = result;
						cache.Set<ConfigurationResponse> (cacheKey, this.configuration);
						tcs.TrySetResult (this.configuration);
					}
				}
			});
			return tcs.Task;
		}

		public Task<List<Movie>> GetSpotlightMoviesAsync () {
			var tcs = new TaskCompletionSource<List<Movie>> ();
			Task.Factory.StartNew (async () => {
				var categories = await Data.Current.GetMoviesByCategoryAsync ();
				var result = new List<Movie> ();
				foreach (var category in categories) {
					var thisSpotlight = category.Spotlight;
					if (thisSpotlight != null)
						result.Add (thisSpotlight);
				}
				tcs.TrySetResult (result);
			});
			return tcs.Task;
		}

		public Task<List<MovieCategory>> GetMoviesByCategoryAsync () {
			var tcs = new TaskCompletionSource<List<MovieCategory>> ();
			Task.Factory.StartNew (async () => {
				this.topRatedMovies = await Data.Current.GetTopRatedMoviesAsync ();
				this.popularMovies = await Data.Current.GetPopularMoviesAsync ();
				this.nowPlayingMovies = await Data.Current.GetNowPlayingMoviesAsync ();
				this.upcomingMovies = await Data.Current.GetUpcomingMoviesAsync ();
				this.favorites = await Data.Current.GetFavoritesAsync ();
				var result = new List<MovieCategory> ();
				result.Add (new MovieCategory ("Top Rated", this.topRatedMovies.Results)); 
				result.Add (new MovieCategory ("Popular", this.popularMovies.Results));
				result.Add (new MovieCategory ("Now Playing", this.nowPlayingMovies.Results));
				result.Add (new MovieCategory ("Upcoming", this.upcomingMovies.Results));
				result.Add (new MovieCategory ("Your Favorites", this.favorites));
				tcs.TrySetResult (result);
			});
			return tcs.Task;
		}

		public Task<GetMoviesResponse> GetTopRatedMoviesAsync () {
			var tcs = new TaskCompletionSource<GetMoviesResponse> ();
			Task.Factory.StartNew (async () => {
				var urlString = String.Format (SharedConstants.GetTopRatedMoviesUriFormatString, SharedConstants.ApiKey);
				var cacheKey = this.getCacheKey(urlString);
				if (this.topRatedMovies != null) {
					tcs.TrySetResult (this.topRatedMovies);
				} else {
					if (cache.ContainsKey(cacheKey)) {
						var response = cache.Get<GetMoviesResponse>(cacheKey);
						this.topRatedMovies = response;
						tcs.TrySetResult (this.topRatedMovies);
					} else {
						var client = this.httpClientFactory.Create ();
						var response = await client.GetAsync (urlString);
						var json = await response.Content.ReadAsStringAsync ();
						var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
						this.topRatedMovies = result;
						cache.Set<GetMoviesResponse> (cacheKey, this.topRatedMovies);
						tcs.TrySetResult (this.topRatedMovies);
					}
				}
			});
			return tcs.Task;
		}

		public Task<GetMoviesResponse> GetPopularMoviesAsync () {
			var tcs = new TaskCompletionSource<GetMoviesResponse> ();
			Task.Factory.StartNew (async () => {
				var urlString = String.Format (SharedConstants.GetPopularMoviesUriFormatString, SharedConstants.ApiKey);
				var cacheKey = this.getCacheKey(urlString);
				if (this.popularMovies != null) {
					tcs.TrySetResult (this.popularMovies);
				} else {
					if (cache.ContainsKey(cacheKey)) {
						var response = cache.Get<GetMoviesResponse>(cacheKey);
						this.popularMovies = response;
						tcs.TrySetResult (this.popularMovies);
					} else {
						var client = this.httpClientFactory.Create ();
						var response = await client.GetAsync (urlString);
						var json = await response.Content.ReadAsStringAsync ();
						var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
						this.popularMovies = result;
						cache.Set<GetMoviesResponse> (cacheKey, this.popularMovies);
						tcs.TrySetResult (this.popularMovies);
					}
				}
			});
			return tcs.Task;
		}

		public Task<GetMoviesResponse> GetNowPlayingMoviesAsync () {
			var tcs = new TaskCompletionSource<GetMoviesResponse> ();
			Task.Factory.StartNew (async () => {
				var urlString = String.Format (SharedConstants.GetNowPlayingMoviesUriFormatString, SharedConstants.ApiKey);
				var cacheKey = this.getCacheKey (urlString);
				if (this.nowPlayingMovies != null) {
					tcs.TrySetResult (this.nowPlayingMovies);
				} else {
					if (cache.ContainsKey(cacheKey)) {
						var response = cache.Get<GetMoviesResponse>(cacheKey);
						this.nowPlayingMovies = response;
						tcs.TrySetResult (this.nowPlayingMovies);
					} else {
						var client = this.httpClientFactory.Create ();
						var response = await client.GetAsync (urlString);
						var json = await response.Content.ReadAsStringAsync ();
						var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
						this.nowPlayingMovies = result;
						cache.Set<GetMoviesResponse> (cacheKey, this.nowPlayingMovies);
						tcs.TrySetResult (this.nowPlayingMovies);
					}
				}
			});
			return tcs.Task;
		}

		public Task<GetMoviesResponse> GetUpcomingMoviesAsync () {
			var tcs = new TaskCompletionSource<GetMoviesResponse> ();
			Task.Factory.StartNew (async () => {
				var urlString = String.Format (SharedConstants.GetUpcomingMoviesUriFormatString, SharedConstants.ApiKey);
				var cacheKey = this.getCacheKey (urlString);
				if (this.upcomingMovies != null) {
					tcs.TrySetResult (this.upcomingMovies);
				} else {
					if (cache.ContainsKey(cacheKey)) {
						var response = cache.Get<GetMoviesResponse>(cacheKey);
						this.upcomingMovies = response;
						tcs.TrySetResult (this.upcomingMovies);
					} else {
						var client = this.httpClientFactory.Create ();
						var response = await client.GetAsync (urlString);
						var json = await response.Content.ReadAsStringAsync ();
						var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
						this.upcomingMovies = result;
						cache.Set<GetMoviesResponse> (cacheKey, this.upcomingMovies);
						tcs.TrySetResult (this.upcomingMovies);
					}
				}
			});
			return tcs.Task;
		}

		public Task<GetVideosForMovieResponse> GetVideosForMovieAsync (int movieId) {
			var tcs = new TaskCompletionSource<GetVideosForMovieResponse> ();
			Task.Factory.StartNew (async () => {
				var urlString = String.Format (SharedConstants.GetVidesoForMovieUriFormatString, movieId, SharedConstants.ApiKey);
				var cacheKey = this.getCacheKey (urlString);
				if (cache.ContainsKey(cacheKey)) {
					var response = cache.Get<GetVideosForMovieResponse>(cacheKey);
					tcs.TrySetResult (response);
				} else {
					var client = this.httpClientFactory.Create ();
					var response = await client.GetAsync (urlString);
					var json = await response.Content.ReadAsStringAsync ();
					var result = JsonConvert.DeserializeObject<GetVideosForMovieResponse> (json);
					cache.Set<GetVideosForMovieResponse> (cacheKey, result);
					tcs.TrySetResult (result);
				}
			});
			return tcs.Task;
		}

		public Task<GetMoviesResponse> GetSimilarForMovieAsync (int movieId) {
			var tcs = new TaskCompletionSource<GetMoviesResponse> ();
			Task.Factory.StartNew (async () => {
				var urlString = String.Format (SharedConstants.GetSimilarForMovieUriFormatString, movieId, SharedConstants.ApiKey);
				var cacheKey = this.getCacheKey (urlString);
				if (cache.ContainsKey(cacheKey)) {
					var response = cache.Get<GetMoviesResponse>(cacheKey);
					tcs.TrySetResult (response);
				} else {
					var client = this.httpClientFactory.Create ();
					var response = await client.GetAsync (urlString);
					var json = await response.Content.ReadAsStringAsync ();
					var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
					cache.Set<GetMoviesResponse> (cacheKey, result);
					tcs.TrySetResult (result);
				}
			});
			return tcs.Task;
		}

		public string GetOpenInYoutubeUri (string key) {
			return String.Format (SharedConstants.OpenInYouTubeFormatString, key);
		}
			
		/// <summary>
		/// Determines whether this instance of <see cref="Movie"/> is in the favorites list.
		/// </summary>
		/// <returns><c>true</c> if this instance is in favorites; otherwise, <c>false</c>.</returns>
		/// <param name="movie">Movie.</param>
		public bool IsInFavorites (Movie movie) {
			if (this.favorites == null)
				return false;
			return this.favorites.FirstOrDefault (x => x.Id == movie.Id) != null;
		}

		/// <summary>
		/// Adds this instance of <see cref="Movie"/> to favorites.
		/// </summary>
		/// <param name="movie">Movie.</param>
		private void addToFavorites (Movie movie) {
			var existing = this.favorites.FirstOrDefault (x => x.Id == movie.Id);
			if (existing == null) {
				this.favorites.Add (movie);
				this.cache.Set <List<Movie>> (SharedConstants.FavoritesCacheKey, this.favorites);
				this.OnFavoriteChanged (new FavoriteChangedEventArgs(movie, true));
			}
		}

		/// <summary>
		/// Removes this instance of <see cref="Movie"/> from favorites.
		/// </summary>
		/// <param name="movie">Movie.</param>
		public void RemoveFromFavorites (Movie movie) {
			var existing = this.favorites.FirstOrDefault (x => x.Id == movie.Id);
			if (existing != null) {
				this.favorites.Remove (existing);
				this.cache.Set <List<Movie>>(SharedConstants.FavoritesCacheKey, this.favorites);
				this.OnFavoriteChanged (new FavoriteChangedEventArgs(movie, false));
			}
		}

		/// <summary>
		/// Gets the favorites list asynchronously.
		/// </summary>
		/// <param name="completionHandler">Method to execute when completed</param>
		public Task<List<Movie>> GetFavoritesAsync () {
			var tcs = new TaskCompletionSource<List<Movie>> ();
			Task.Factory.StartNew (() => {
				var cacheKey = SharedConstants.FavoritesCacheKey;
				if (this.favorites != null){
					tcs.TrySetResult(this.favorites);
				} else if (cache.ContainsKey(cacheKey)) {
					this.favorites = cache.Get<List<Movie>>(cacheKey);
					tcs.TrySetResult (this.favorites);
				} else {
					this.favorites = new List<Movie>();
					tcs.TrySetResult (this.favorites);
				}
			});
			return tcs.Task;
		}

		/// <summary>
		/// Adds to or removes the specified <see cref="Movie"/> from the user's Favorites list
		/// </summary>
		/// <param name="movie">Movie.</param>
		public void ToggleFavorite (Movie movie) {
			if (this.IsInFavorites (movie))
				this.RemoveFromFavorites (movie);
			else
				this.addToFavorites (movie);
		}

		public Task<List<Movie>> SearchAsync (string searchText) {
			var tcs = new TaskCompletionSource<List<Movie>> ();
			Task.Factory.StartNew (() => {
				var result = new List<Movie>();
				foreach (var category in this.categories) {
					var matches = category.Movies.FindAll (x => x.Title.Contains (searchText)).ToList();
					foreach (var match in matches) {
						if (result.Find (x => x.Id == match.Id) == null)
							result.Add (match);
					}
				}
				tcs.TrySetResult (result);
			});
			return tcs.Task;
		}
        #endregion

		public event EventHandler<FavoriteChangedEventArgs> FavoriteChanged;
		protected void OnFavoriteChanged (FavoriteChangedEventArgs args) {
			if (this.FavoriteChanged != null)
				this.FavoriteChanged (this, args);
		}
	}
}