using System;
using System.Collections.Generic;
using System.Linq;

namespace com.interactiverobert.prototypes.movieexplorer.shared.Entities.Movie
{
	public class MovieCategory 
	{
		public string CategoryName { get; private set; }
		public List<Movie> Movies { get; private set; }
		public Movie Spotlight { 
			get {
				if (this.Movies == null)
					return null;
				return this.Movies.FirstOrDefault ();
			}
		}

		public MovieCategory (string categoryName, List<Movie> movies) {
			this.CategoryName = categoryName;
			this.Movies = movies;
		}
	}
}