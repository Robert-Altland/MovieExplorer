using System.Collections.Generic;

namespace com.interactiverobert.prototypes.movieexplorer.shared.contracts
{
	/// <summary>
	/// Contract for interacting with inexpensive, semi-durable device storage.
	/// </summary>
	public interface ICache
	{
		/// <summary>
		/// Indicates whether an item with the specified key exists in cache.
		/// </summary>
		/// <returns><c>true</c>, if item with key was found, <c>false</c> otherwise.</returns>
		/// <param name="key">Unique key</param>
		bool ContainsKey(string key);
		/// <summary>
		/// Get the item from cache using the specified key
		/// </summary>
		/// <param name="key">Unique key</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		T Get<T>(string key);
		/// <summary>
		/// Sets the specified item to cache with the specified key
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="item">Item.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		void Set<T>(string key, T item);
	}
}