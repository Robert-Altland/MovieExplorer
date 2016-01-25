using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.interactiverobert.prototypes.movieexplorer.shared
{
	public class Data
	{
		private List<Movie> favorites = new List<Movie>();

		private static Data current;
		public static Data Current
		{
			get
			{
				if (current == null)
					current = new Data ();
				return current;
			}
			set { current = value; }
		}

		public Data () {
			
		}

		public bool IsInFavorites (Movie movie) {
			return this.favorites.FirstOrDefault (x => x.Id == movie.Id) != null;
		}

		public void AddToFavorites (Movie movie) {
			this.favorites.Add (movie);
		}

		public void RemoveFromFavorites (Movie movie) {
			this.favorites.Remove (movie);
		}

		public void GetFavoritesAsync (Action<List<Movie>> completionHandler) {
			if (completionHandler != null)
				completionHandler.Invoke (this.favorites);
		}
	}
}