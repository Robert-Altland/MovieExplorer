using System;
using System.Threading;
using com.interactiverobert.prototypes.movieexplorer.shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace movieexplorer.tests
{
	[TestClass]
	public class ApiTests
	{
		private bool getMoviesCompletedHasBeenCalled;

		[TestCleanup]
		public void Teardown()
		{
			Api.Current = null;
		}

		[TestMethod]
		[ExpectedException (typeof(TypeInitializationException))]
		public void GetMoviesAsync_NoHttpClientFactoryRegistered()
		{
			Api.Current.GetMoviesCompleted += getMoviesCompleted;
			Api.Current.GetMoviesAsync().Wait();
		}

		[TestMethod]
		[ExpectedException(typeof(TypeInitializationException))]
		public void GetMoviesAsync_InvalidApiKey()
		{
			Api.Current.GetMoviesCompleted += getMoviesCompleted;
			Api.Current.GetMoviesAsync().Wait();
			Assert.IsTrue(this.getMoviesCompletedHasBeenCalled, "Api.Current.GetMoviesAsync did not complete.");
		}

		[TestMethod]
		public void GetMoviesAsync_Test()
		{
			DependencyManager.RegisterHttpClientFactory(new HttpClientFactory());
			Api.Current.GetMoviesCompleted += getMoviesCompleted;
			Api.Current.GetMoviesAsync().Wait();
			Assert.IsTrue(this.getMoviesCompletedHasBeenCalled, "Api.Current.GetMoviesAsync did not complete.");
		}

		private void getMoviesCompleted(object sender, MovieDiscoverResponse e)
		{
			this.getMoviesCompletedHasBeenCalled = true;
		}
	}
}
