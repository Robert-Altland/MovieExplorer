using System;
using System.Net.Http;

namespace com.interactiverobert.prototypes.movieexplorer.shared.Contracts
{
	public interface IHttpClientFactory
	{
		HttpClient Create();
	}
}

