using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;

using Newtonsoft.Json;

namespace com.interactiverobert.prototypes.movieexplorer.shared
{
	public class Api
	{
		private IHttpClientFactory httpClientFactory;
		private ConfigurationResponse configuration;
		private GetMoviesResponse topRatedMovies;
		private GetMoviesResponse popularMovies;
		private GetMoviesResponse nowPlayingMovies;
		private GetMoviesResponse upcomingMovies;

		private static Api current;
        public static Api Current
		{
			get
			{
				if (current == null)
					current = new Api (DependencyManager.HttpClientFactory);
				return current;
			}
			set { current = value; }
		}

		public Api (IHttpClientFactory httpClientFactory) {
			if (DependencyManager.HttpClientFactory == null)
				throw new TypeInitializationException (typeof(Api).FullName, new ArgumentNullException("An instance of IHttpClientFactory must be registered with DependencyManager before using an instnce of the Api class."));
			this.httpClientFactory = DependencyManager.HttpClientFactory;
		}

		public async void GetTopRatedMoviesAsync (Action<GetMoviesResponse> completionAction) {
			if (this.topRatedMovies != null) {
				if (completionAction != null)
					completionAction.Invoke (this.topRatedMovies);
			} else {
				var client = this.httpClientFactory.Create ();
				var response = await client.GetAsync (String.Format (StringResources.GetTopRatedMoviesUriFormatString, StringResources.ApiKey));
				var json = await response.Content.ReadAsStringAsync ();
				var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
				this.topRatedMovies = result;
				if (completionAction != null)
					completionAction.Invoke (this.topRatedMovies);
			}
		}

		public async void GetPopularMoviesAsync (Action<GetMoviesResponse> completionAction) {
			if (this.popularMovies != null) {
				if (completionAction != null)
					completionAction.Invoke (this.popularMovies);
			} else {
				var client = this.httpClientFactory.Create ();
				var response = await client.GetAsync (String.Format (StringResources.GetPopularMoviesUriFormatString, StringResources.ApiKey));
				var json = await response.Content.ReadAsStringAsync ();
				var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
				this.popularMovies = result;
				if (completionAction != null)
					completionAction.Invoke (this.popularMovies);
			}
		}

		public async void GetNowPlayingMoviesAsync (Action<GetMoviesResponse> completionAction) {
			if (this.nowPlayingMovies != null) {
				if (completionAction != null)
					completionAction.Invoke (this.nowPlayingMovies);
			} else {
				var client = this.httpClientFactory.Create ();
				var response = await client.GetAsync (String.Format (StringResources.GetNowPlayingMoviesUriFormatString, StringResources.ApiKey));
				var json = await response.Content.ReadAsStringAsync ();
				var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
				this.nowPlayingMovies = result;
				if (completionAction != null)
					completionAction.Invoke (this.nowPlayingMovies);
			}
		}

		public async void GetUpcomingMoviesAsync (Action<GetMoviesResponse> completionAction) {
			if (this.upcomingMovies != null) {
				if (completionAction != null)
					completionAction.Invoke (this.upcomingMovies);
			} else {
				var client = this.httpClientFactory.Create ();
				var response = await client.GetAsync (String.Format (StringResources.GetUpcomingMoviesUriFormatString, StringResources.ApiKey));
				var json = await response.Content.ReadAsStringAsync ();
				var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
				this.upcomingMovies = result;
				if (completionAction != null)
					completionAction.Invoke (this.upcomingMovies);
			}
		}

		public async void GetConfigurationAsync (Action<ConfigurationResponse> completionAction) {
			if (this.configuration != null) {
				if (completionAction != null)
					completionAction.Invoke (this.configuration);
			} else {
				var client = this.httpClientFactory.Create ();
				var response = await client.GetAsync (String.Format (StringResources.GetConfigurationUriFormatString, StringResources.ApiKey));
				var json = await response.Content.ReadAsStringAsync ();
				var result = JsonConvert.DeserializeObject<ConfigurationResponse> (json);
				this.configuration = result;
				if (completionAction != null)
					completionAction.Invoke (this.configuration);
			}
		}

		public async void GetVideosForMovieAsync (int movieId, Action<GetVideosForMovieResponse> completionAction) {
			var client = this.httpClientFactory.Create ();
			var response = await client.GetAsync (String.Format (StringResources.GetVidesoForMovieUriFormatString, movieId, StringResources.ApiKey));
			var json = await response.Content.ReadAsStringAsync ();
			var result = JsonConvert.DeserializeObject<GetVideosForMovieResponse> (json);
			if (completionAction != null)
				completionAction.Invoke (result);
		}

		public async void GetSimilarForMovieAsync (int movieId, Action<GetMoviesResponse> completionAction) {
			var client = this.httpClientFactory.Create ();
			var response = await client.GetAsync (String.Format (StringResources.GetSimilarForMovieUriFormatString, movieId, StringResources.ApiKey));
			var json = await response.Content.ReadAsStringAsync ();
			var result = JsonConvert.DeserializeObject<GetMoviesResponse> (json);
			if (completionAction != null)
				completionAction.Invoke (result);
		}

		public async void GetMoviesByCategoryAsync (Action<List<MovieCategory>> completionAction) {
			Api.Current.GetTopRatedMoviesAsync ((topRated) => {
				this.topRatedMovies = topRated;
				Api.Current.GetPopularMoviesAsync(popular => {
					this.popularMovies = popular;
					Api.Current.GetNowPlayingMoviesAsync(nowPlaying => {
						this.nowPlayingMovies = nowPlaying;
						Api.Current.GetUpcomingMoviesAsync(upcoming => {
							this.upcomingMovies = upcoming;
							Data.Current.GetFavoritesAsync (faves => {
								var result = new List<MovieCategory> ();
								result.Add (new MovieCategory ("Top Rated", this.topRatedMovies.Results)); 
								result.Add (new MovieCategory ("Popular", this.popularMovies.Results));
								result.Add (new MovieCategory ("Now Playing", this.nowPlayingMovies.Results));
								result.Add (new MovieCategory ("Upcoming", this.upcomingMovies.Results));
								result.Add (new MovieCategory ("Your Favorites", faves));
								if (completionAction != null)
									completionAction.Invoke (result);
							});
						});
					});
				});
			});
		}

		public string GetOpenInYoutubeUri (string key) {
			return String.Format (StringResources.OpenInYouTubeFormatString, key);
		}
	}
}