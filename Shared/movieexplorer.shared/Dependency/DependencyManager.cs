using System;
using com.interactiverobert.prototypes.movieexplorer.shared.contracts;

namespace com.interactiverobert.prototypes.movieexplorer.shared
{
	public static class DependencyManager
	{
		private static IHttpClientFactory httpClientFactory;
		public static IHttpClientFactory HttpClientFactory { 
			get {
				if (httpClientFactory == null)
					throw new InvalidOperationException ("An instance of IHttpClientFactory must be registered using the RegisterHttpClientFactory() method before accessing this property.");
				return httpClientFactory;
			}
		}

		private static ICache cacheSvc;
		public static ICache Cache {
			get {
				if (httpClientFactory == null)
					throw new InvalidOperationException ("An instance of ICache must be registered using the RegisterCache() method before accessing this property.");
				return cacheSvc;
			}
		}

		public static void RegisterHttpClientFactory (IHttpClientFactory factory) {
			httpClientFactory = factory;
		}

		public static void RegisterCache (ICache cache) {
			cacheSvc = cache;
		}
	}
}