using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Linq;
using Newtonsoft.Json;

namespace com.interactiverobert.prototypes.movieexplorer.shared
{
	public class Data
	{
		#region Private fields
		private IHttpClientFactory httpClientFactory;
		private ConfigurationResponse configuration;
		private GetMoviesResponse topRatedMovies;
		private GetMoviesResponse popularMovies;
		private GetMoviesResponse nowPlayingMovies;
		private GetMoviesResponse upcomingMovies;
		private List<Movie> favorites = new List<Movie>();
		#endregion

		#region Singleton
		private static Data current;
        public static Data Current
		{
			get
			{
				if (current == null)
					current = new Data (DependencyManager.HttpClientFactory);
				return current;
			}
			set { current = value; }
		}
		#endregion

		#region Constructor
		public Data (IHttpClientFactory httpClientFactory) {
			if (DependencyManager.HttpClientFactory == null)
				throw new TypeInitializationException (typeof(Data).FullName, new ArgumentNullException("An instance of IHttpClientFactory must be registered with DependencyManager before using an instnce of the Api class."));
			this.httpClientFactory = DependencyManager.HttpClientFactory;
		}
		#endregion

		#region Public methods
		public Task<GetMoviesResponse> GetTopRatedMoviesAsync () {
			var tcs = new TaskCompletionSource<GetMoviesResponse> ();
			Task.Factory.StartNew (async () => {
				if (this.topRatedMovies != null) {
					tcs.TrySetResult (this.topRatedMovies);
				} else {
					var client = this.httpClientFactory.Create ();
					var response = await client.GetAsync (String.Format (StringResources.GetTopRatedMoviesUriFormatString, StringResources.ApiKey));
					var json = await response.Content.ReadAsStringAsync ();
					var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
					this.topRatedMovies = result;
					tcs.TrySetResult (this.topRatedMovies);
				}
			});
			return tcs.Task;
		}

		public Task<GetMoviesResponse> GetPopularMoviesAsync () {
			var tcs = new TaskCompletionSource<GetMoviesResponse> ();
			Task.Factory.StartNew (async () => {
				if (this.popularMovies != null) {
					tcs.TrySetResult (this.popularMovies);
				} else {
					var client = this.httpClientFactory.Create ();
					var response = await client.GetAsync (String.Format (StringResources.GetPopularMoviesUriFormatString, StringResources.ApiKey));
					var json = await response.Content.ReadAsStringAsync ();
					var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
					this.popularMovies = result;
					tcs.TrySetResult (this.popularMovies);
				}
			});
			return tcs.Task;
		}

		public Task<GetMoviesResponse> GetNowPlayingMoviesAsync () {
			var tcs = new TaskCompletionSource<GetMoviesResponse> ();
			Task.Factory.StartNew (async () => {
				if (this.nowPlayingMovies != null) {
					tcs.TrySetResult (this.nowPlayingMovies);
				} else {
					var client = this.httpClientFactory.Create ();
					var response = await client.GetAsync (String.Format (StringResources.GetNowPlayingMoviesUriFormatString, StringResources.ApiKey));
					var json = await response.Content.ReadAsStringAsync ();
					var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
					this.nowPlayingMovies = result;
					tcs.TrySetResult (this.nowPlayingMovies);
				}
			});
			return tcs.Task;
		}

		public Task<GetMoviesResponse> GetUpcomingMoviesAsync () {
			var tcs = new TaskCompletionSource<GetMoviesResponse> ();
			Task.Factory.StartNew (async () => {
				if (this.upcomingMovies != null) {
					tcs.TrySetResult (this.upcomingMovies);
				} else {
					var client = this.httpClientFactory.Create ();
					var response = await client.GetAsync (String.Format (StringResources.GetUpcomingMoviesUriFormatString, StringResources.ApiKey));
					var json = await response.Content.ReadAsStringAsync ();
					var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
					this.upcomingMovies = result;
					tcs.TrySetResult (this.upcomingMovies);
				}
			});
			return tcs.Task;
		}

		public Task<ConfigurationResponse> GetConfigurationAsync () {
			var tcs = new TaskCompletionSource<ConfigurationResponse> ();
			Task.Factory.StartNew (async () => {
				if (this.configuration != null) {
					tcs.TrySetResult (this.configuration);
				} else {
					var client = this.httpClientFactory.Create ();
					var response = await client.GetAsync (String.Format (StringResources.GetConfigurationUriFormatString, StringResources.ApiKey));
					var json = await response.Content.ReadAsStringAsync ();
					var result = JsonConvert.DeserializeObject<ConfigurationResponse> (json);
					this.configuration = result;
					tcs.TrySetResult (this.configuration);
				}
			});
			return tcs.Task;
		}

		public Task<GetVideosForMovieResponse> GetVideosForMovieAsync (int movieId) {
			var tcs = new TaskCompletionSource<GetVideosForMovieResponse> ();
			Task.Factory.StartNew (async () => {
				var client = this.httpClientFactory.Create ();
				var response = await client.GetAsync (String.Format (StringResources.GetVidesoForMovieUriFormatString, movieId, StringResources.ApiKey));
				var json = await response.Content.ReadAsStringAsync ();
				var result = JsonConvert.DeserializeObject<GetVideosForMovieResponse> (json);
				tcs.TrySetResult (result);
			});
			return tcs.Task;
		}

		public Task<GetMoviesResponse> GetSimilarForMovieAsync (int movieId) {
			var tcs = new TaskCompletionSource<GetMoviesResponse> ();
			Task.Factory.StartNew (async () => {
				var client = this.httpClientFactory.Create ();
				var response = await client.GetAsync (String.Format (StringResources.GetSimilarForMovieUriFormatString, movieId, StringResources.ApiKey));
				var json = await response.Content.ReadAsStringAsync ();
				var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
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

		public string GetOpenInYoutubeUri (string key) {
			return String.Format (StringResources.OpenInYouTubeFormatString, key);
		}
			
		/// <summary>
		/// Determines whether this instance of <see cref="Movie"/> is in the favorites list.
		/// </summary>
		/// <returns><c>true</c> if this instance is in favorites; otherwise, <c>false</c>.</returns>
		/// <param name="movie">Movie.</param>
		public bool IsInFavorites (Movie movie) {
			return this.favorites.FirstOrDefault (x => x.Id == movie.Id) != null;
		}

		/// <summary>
		/// Adds this instance of <see cref="Movie"/> to favorites.
		/// </summary>
		/// <param name="movie">Movie.</param>
		public void AddToFavorites (Movie movie) {
			var existing = this.favorites.FirstOrDefault (x => x.Id == movie.Id);
			if (existing == null)
				this.favorites.Add (movie);
		}

		/// <summary>
		/// Removes this instance of <see cref="Movie"/> from favorites.
		/// </summary>
		/// <param name="movie">Movie.</param>
		public void RemoveFromFavorites (Movie movie) {
			var existing = this.favorites.FirstOrDefault (x => x.Id == movie.Id);
			if (existing != null)
				this.favorites.Remove (movie);
		}

		/// <summary>
		/// Gets the favorites list asynchronously.
		/// </summary>
		/// <param name="completionHandler">Method to execute when completed</param>
		public Task<List<Movie>> GetFavoritesAsync () {
			var tcs = new TaskCompletionSource<List<Movie>> ();
			Task.Factory.StartNew (() => {
				tcs.TrySetResult (this.favorites);
			});
			return tcs.Task;
		}
		#endregion
	}
}