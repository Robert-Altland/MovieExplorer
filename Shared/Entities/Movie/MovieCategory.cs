using System;
using System.Collections.Generic;

namespace com.interactiverobert.prototypes.movieexplorer.shared
{
	public class MovieCategory 
	{
		public string CategoryName { get; private set; }
		public List<Movie> Movies { get; private set; }

		public MovieCategory (string categoryName, List<Movie> movies) {
			this.CategoryName = categoryName;
			this.Movies = movies;
		}
	}
}