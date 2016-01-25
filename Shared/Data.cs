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

		public Task GetFavoritesAsync () {
			this.OnGetFavoritesCompleted (this.favorites);
			return Task.FromResult (true);
		}

		public event EventHandler<List<Movie>> GetFavoritesCompleted;
		protected void OnGetFavoritesCompleted (List<Movie> response) {
			if (this.GetFavoritesCompleted != null)
				this.GetFavoritesCompleted (null, response);
		}
	}
}

