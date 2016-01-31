using System;
using System.Net.Http;

using com.interactiverobert.prototypes.movieexplorer.shared.Contracts;

namespace com.interactiverobert.prototypes.movieexplorer.shared
{
	public class HttpClientFactory : IHttpClientFactory
	{
		#region Constructor
		public HttpClientFactory () {
			
		}
		#endregion

		#region Public methods -- IHttpClientFactory implementation
		public HttpClient Create () {
			var client = new HttpClient ();
			client.BaseAddress = new Uri("http://api.themoviedb.org/3/");
			return client;
		}
		#endregion
	}
}