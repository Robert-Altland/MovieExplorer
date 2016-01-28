using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.interactiverobert.prototypes.movieexplorer.shared;
using com.interactiverobert.prototypes.movieexplorer.shared.contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace movieexplorer.tests
{
	[TestClass]
	public class DependencyManagerTests
	{
		[TestInitialize]
		public void Setup()
		{
			DependencyManager.Reset();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void DependencyManager_AccessCacheBeforeRegistration()
		{
			//Act
			var cache = DependencyManager.Cache;

			//Assert
			Assert.Fail("Expected exception not thrown.");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void DependencyManager_AccessHttpClientFactoryBeforeRegistration()
		{
			//Act
			var factory = DependencyManager.HttpClientFactory;
			
			//Assert
			Assert.Fail("Expected exception not thrown.");
		}

		[TestMethod]
		public void DependencyManager_AccessCacheAfterRegistration()
		{
			//Arrange
			DependencyManager.RegisterCache(new Moq.Mock<ICache>().Object);

			//Act
			var cache = DependencyManager.Cache;

			//Assert
			Assert.IsNotNull(cache);
		}

		[TestMethod]
		public void DependencyManager_AccessHttpClientFactoryAfterRegistration()
		{
			//Arrange
			DependencyManager.RegisterHttpClientFactory(new Moq.Mock<IHttpClientFactory>().Object);

			//Act
			var factory = DependencyManager.HttpClientFactory;

			//Assert
			Assert.IsNotNull(factory);
		}

	}
}
