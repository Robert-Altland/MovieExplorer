using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.interactiverobert.prototypes.movieexplorer.shared
{
	public class Data
	{
		#region Shared members
		private static Data current;
		/// <summary>
		/// Gets or sets the current instance of <see cref="Data"/>.
		/// </summary>
		/// <value>The current.</value>
		public static Data Current {
			get {
				if (current == null)
					current = new Data ();
				return current;
			}
			set { current = value; }
		}
		#endregion

		#region Private fields
		private List<Movie> favorites = new List<Movie>();
		#endregion

		#region Constructor
		/// <summary>
		/// Create a new instance of the <see cref="Data"/> class.
		/// </summary>
		public Data () {
			
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Determines whether this instance of <see cref="Movie"/> is in the favorites list.
		/// </summary>
		/// <returns><c>true</c> if this instance is in favorites; otherwise, <c>false</c>.</returns>
		/// <param name="movie">Movie.</param>
		public bool IsInFavorites (Movie movie) {
			return this.favorites.FirstOrDefault (x => x.Id == movie.Id) != null;
		}

		/// <summary>
		/// Adds this instance of <see cref="Movie"/> to favorites.
		/// </summary>
		/// <param name="movie">Movie.</param>
		public void AddToFavorites (Movie movie) {
			this.favorites.Add (movie);
		}

		/// <summary>
		/// Removes this instance of <see cref="Movie"/> from favorites.
		/// </summary>
		/// <param name="movie">Movie.</param>
		public void RemoveFromFavorites (Movie movie) {
			this.favorites.Remove (movie);
		}

		/// <summary>
		/// Gets the favorites list asynchronously.
		/// </summary>
		/// <param name="completionHandler">Method to execute when completed</param>
		public void GetFavoritesAsync (Action<List<Movie>> completionHandler) {
			if (completionHandler != null)
				completionHandler.Invoke (this.favorites);
		}
		#endregion
	}
}