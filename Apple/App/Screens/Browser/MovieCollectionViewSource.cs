using System;
using System.Collections.Generic;

using UIKit;
using Foundation;

using com.interactiverobert.prototypes.movieexplorer.shared;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public class MovieCollectionViewSource : UICollectionViewSource
	{
		private ConfigurationResponse configuration;
		private List<Movie> movies;

		public MovieCollectionViewSource (List<Movie> movies, ConfigurationResponse configuration) {
			this.movies = movies;
			this.configuration = configuration;
		}

		public event EventHandler<Movie> MovieSelected;
		protected void OnMovieSelected (Movie movie) {
			if (this.MovieSelected != null)
				this.MovieSelected (this, movie);
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath) {
			var dataItem = this.movies [indexPath.Row];
			var cell = collectionView.DequeueReusableCell (MovieCollectionViewCell.ReuseKey, indexPath) as MovieCollectionViewCell;
			cell.Bind (dataItem, this.configuration);
			return cell;
		}

		public override nint GetItemsCount (UICollectionView collectionView, nint section) {
			return this.movies.Count;
		}

		public override nint NumberOfSections (UICollectionView collectionView) {
			return 1;
		}

		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath) {
			this.OnMovieSelected (this.movies [indexPath.Row]);
		}
	}
}