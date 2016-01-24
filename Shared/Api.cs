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
			var response = await client.GetAsync (String.Format(StringResources.GetMoviesUriFormatString, StringResources.ApiKey));
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