using System;
using System.Net.Http;

namespace com.interactiverobert.prototypes.movieexplorer.shared
{
	public class HttpClientFactory : IHttpClientFactory
	{
		public HttpClientFactory () {
			
		}

		public HttpClient Create () {
			var client = new HttpClient ();
			client.BaseAddress = new Uri("http://api.themoviedb.org/3/");
			return client;
		}
	}
}