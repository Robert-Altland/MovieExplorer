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

		[TestMethod]
		[ExpectedException (typeof(TypeInitializationException))]
		public void GetMoviesAsync_NoHttpClientFactoryRegistered()
		{
			Api.Current.GetMoviesCompleted += getMoviesCompleted;
			Api.Current.GetMoviesAsync().Wait();
		}

		[TestMethod]
		public void GetMoviesAsync_Test()
		{
			DependencyManager.RegisterHttpClientFactory(new HttpClientFactory());
			Api.Current.GetMoviesCompleted += getMoviesCompleted;
			Api.Current.GetMoviesAsync().Wait();
			Assert.IsTrue(this.getMoviesCompletedHasBeenCalled, "Api.Current.GetMoviesAsync did not complete successfully.");
		}

		private void getMoviesCompleted(object sender, MovieDiscoverResponse e)
		{
			this.getMoviesCompletedHasBeenCalled = true;
		}
	}
}
