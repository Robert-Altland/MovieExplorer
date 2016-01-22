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
		public static async Task GetMoviesAsync () {
			var client = new HttpClient ();
			client.BaseAddress = new Uri("http://api.themoviedb.org/3/");
			var response = await client.GetAsync ("discover/movie?api_key=727d8ece7a1e09019fa1ffdb43d7577b");
			var json = await response.Content.ReadAsStringAsync ();
			var result = JsonConvert.DeserializeObject<MovieDiscoverResponse> (json);
			OnGetMoviesCompleted (result);
		}

		public static event EventHandler<MovieDiscoverResponse> GetMoviesCompleted;
		public static void OnGetMoviesCompleted (MovieDiscoverResponse response) {
			if (GetMoviesCompleted != null)
				GetMoviesCompleted (null, response);
		}
	}
}