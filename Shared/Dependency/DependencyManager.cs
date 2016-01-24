using System;

namespace com.interactiverobert.prototypes.movieexplorer.shared
{
	public static class DependencyManager
	{
		public static IHttpClientFactory HttpClientFactory { get; private set; }

		public static void RegisterHttpClientFactory (IHttpClientFactory factory) {
			HttpClientFactory = factory;
		}
	}
}