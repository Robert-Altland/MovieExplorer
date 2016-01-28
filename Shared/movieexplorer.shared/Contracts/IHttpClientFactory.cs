using System;
using System.Net.Http;

namespace com.interactiverobert.prototypes.movieexplorer.shared
{
	public interface IHttpClientFactory
	{
		HttpClient Create();
	}
}

