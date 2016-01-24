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

		public async Task GetMoviesAsync () {
			var client = this.httpClientFactory.Create ();
			client.BaseAddress = new Uri("http://api.themoviedb.org/3/");
			var response = await client.GetAsync ("discover/movie?api_key=727d8ece7a1e09019fa1ffdb43d7577b");
			var json = await response.Content.ReadAsStringAsync ();
			var result = JsonConvert.DeserializeObject<MovieDiscoverResponse> (json);
			this.OnGetMoviesCompleted (result);
		}

		public event EventHandler<MovieDiscoverResponse> GetMoviesCompleted;
		public void OnGetMoviesCompleted (MovieDiscoverResponse response) {
			if (this.GetMoviesCompleted != null)
				this.GetMoviesCompleted (null, response);
		}
	}
}