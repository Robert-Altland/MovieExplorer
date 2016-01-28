using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

using com.interactiverobert.prototypes.movieexplorer.shared.contracts;

namespace com.interactiverobert.prototypes.movieexplorer.shared.services
{
	public class Cache : ICache 
	{
		#region Private fields
		private object cacheLocker = new object();
		private string cacheDirectory;
		#endregion

		#region Constructor
		public Cache() {
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			var library = Path.Combine (documents, "..", "Library");
			this.cacheDirectory = new DirectoryInfo (Path.Combine (library, "Caches")).FullName;
			Directory.CreateDirectory (this.cacheDirectory);
		}
		#endregion

		#region ICacheService implementation
		public bool ContainsKey (string key) {
			return File.Exists (this.getFilePath (this.cacheDirectory, key));
		}

		public T Get<T> (string key) {
			var result = default(T);
			lock (this.cacheLocker) {
				// Check if item exists in cache
				if (!this.ContainsKey (key))
					return default(T);
				// Read from cache
				var serialized = File.ReadAllText (this.getFilePath (this.cacheDirectory, key));
				result = JsonConvert.DeserializeObject<T>(serialized);
			}
			return result;
		}

		public void Set<T> (string key, T item) {
			// Write item to disk
			lock (this.cacheLocker) {
				try {
					var serialized = JsonConvert.SerializeObject (item);
					File.WriteAllText (this.getFilePath (cacheDirectory, key), serialized);
				} catch (Exception ex) {
					// Can't write, don't crash
					Debug.WriteLine (ex);
				}
			}
		}
		#endregion

		#region Private methods
		private string getFilePath(string directory, string key) {
			return Path.Combine (directory, key + StringResources.SerializedFileExtension);
		}
		#endregion
	}
}