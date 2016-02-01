using System;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Configuration;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Movie;

namespace com.interactiverobert.prototypes.movieexplorer.apple.lib.Contracts
{
	public interface IMovieCell
	{
		void SetSelected (bool isSelected, bool animated, Action completionBlock);
		void SetHighlighted (bool isHighlighted, bool animated = false, Action completionBlock = null);
		void Bind (Movie movie, ConfigurationResponse configuration);
	}
}

