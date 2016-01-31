using System;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Movie;

namespace com.interactiverobert.prototypes.movieexplorer.shared
{
	public class FavoriteChangedEventArgs : EventArgs
	{
		public Movie FavoriteMovie { get; set; }
		public bool IsFavorite { get; set; }

		public FavoriteChangedEventArgs () {
			
		}

		public FavoriteChangedEventArgs (Movie movie, bool isFavorite) {
			this.FavoriteMovie = movie;
			this.IsFavorite = isFavorite;
		}
	}
}