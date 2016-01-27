using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using com.interactiverobert.prototypes.movieexplorer.shared;

namespace movieexplorer.tests
{
	[TestClass]
	public class DataTests
	{
		[TestCleanup]
		public void Teardown()
		{
			Data.Current = null;
		}

		[TestMethod]
		[ExpectedException (typeof(TypeInitializationException))]
		public void GetTopRatedMoviesAsync_NoHttpClientFactoryRegistered()
		{
			var result = Data.Current.GetTopRatedMoviesAsync().Result;
		}

		[TestMethod]
		[ExpectedException(typeof(TypeInitializationException))]
		public void GetTopRatedMoviesAsync_InvalidApiKey()
		{
			DependencyManager.RegisterHttpClientFactory(new HttpClientFactory());
			var result = Data.Current.GetTopRatedMoviesAsync().Result;
			Assert.IsNotNull(result);
			Assert.IsTrue(result.TotalPages > 0);
		}

		[TestMethod]
		public void GetTopRatedMoviesAsync_Test()
		{
			DependencyManager.RegisterHttpClientFactory(new HttpClientFactory());
			var result = Data.Current.GetTopRatedMoviesAsync().Result;
			Assert.IsNotNull(result);
			Assert.IsTrue(result.TotalPages > 0);
		}
	}
}
