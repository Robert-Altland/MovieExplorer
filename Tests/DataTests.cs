using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using com.interactiverobert.prototypes.movieexplorer.shared;
using System.Threading.Tasks;
using com.interactiverobert.prototypes.movieexplorer.shared.contracts;

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
		[ExpectedException (typeof(InvalidOperationException))]
		public void GetTopRatedMoviesAsync_NoHttpClientFactoryRegistered()
		{
			//Act
			Data.Current.GetTopRatedMoviesAsync();

			//Assert
			Assert.Fail("Expected exception not thrown");
		}

		[TestMethod]
		public void GetTopRatedMoviesAsync_Test()
		{
			//Arrange
			DependencyManager.RegisterHttpClientFactory(new HttpClientFactory());
			DependencyManager.RegisterCache(new Moq.Mock<ICache>().Object);
			GetMoviesResponse result = null;

			Task.Run(async () =>
			{
				//Act
				result = await Data.Current.GetTopRatedMoviesAsync();
			}).GetAwaiter().GetResult();


			//Assert
			Assert.IsNotNull(result);
			Assert.IsTrue(result.TotalPages > 0);
		}
	}
}
