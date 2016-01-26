using System;
using System.Collections.Generic;

using UIKit;
using Foundation;

using com.interactiverobert.prototypes.movieexplorer.shared;
using com.interactiverobert.prototypes.movieexplorer.apple.lib.Resources;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public class MovieCollectionViewSource : UICollectionViewSource
	{
		#region Private fields
		private ConfigurationResponse configuration;
		private List<Movie> movies;
		#endregion

		#region Constructor
		public MovieCollectionViewSource (List<Movie> movies, ConfigurationResponse configuration) {
			this.movies = movies;
			this.configuration = configuration;
		}
		#endregion

		public void Reload (List<Movie> movies) {
			this.movies = movies;
		}

		#region Events
		public event EventHandler<Movie> MovieSelected;
		protected void OnMovieSelected (Movie movie) {
			if (this.MovieSelected != null)
				this.MovieSelected (this, movie);
		}
		#endregion

		#region UICollectionViewSource overrides
		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath) {
			var dataItem = this.movies [indexPath.Row];
			var cell = collectionView.DequeueReusableCell (AppleResources.MovieCollectionViewCell_ReuseKey, indexPath) as MovieCollectionViewCell;
			cell.Bind (dataItem, this.configuration);
			return cell;
		}

		public override nint GetItemsCount (UICollectionView collectionView, nint section) {
			if (this.movies == null)
				return 0;
			return this.movies.Count;
		}

		public override nint NumberOfSections (UICollectionView collectionView) {
			return 1;
		}

		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath) {
			if (this.movies != null && this.movies.Count > indexPath.Row) {
				var cell = this.GetCell (collectionView, indexPath) as MovieCollectionViewCell;
				cell.Selected = true;
				collectionView.CollectionViewLayout.InvalidateLayout ();
				this.OnMovieSelected (this.movies [indexPath.Row]);
			}
		}
		#endregion
	}
}