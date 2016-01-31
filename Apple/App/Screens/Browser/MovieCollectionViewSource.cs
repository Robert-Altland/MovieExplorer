using System;
using System.Collections.Generic;

using UIKit;
using Foundation;

using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Configuration;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Movie;
using com.interactiverobert.prototypes.movieexplorer.apple.lib.Resources;
using com.interactiverobert.prototypes.movieexplorer.shared.Services;
using com.interactiverobert.prototypes.movieexplorer.apple.lib.Contracts;

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

		#region Events
		public event EventHandler<Movie> MovieSelected;
		protected void OnMovieSelected (Movie movie) {
			if (this.MovieSelected != null)
				this.MovieSelected (this, movie);
		}
		#endregion

		#region Public methods
		public void Reload (List<Movie> movies) {
			this.movies = movies;
		}
		#endregion

		#region UICollectionViewSource overrides
		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath) {
			var dataItem = this.movies [indexPath.Row];
			var cell = collectionView.DequeueReusableCell (AppleConstants.MovieCollectionViewCell_ReuseKey, indexPath) as IMovieCollectionViewCell;
			cell.Bind (dataItem, this.configuration);
			return cell as UICollectionViewCell;
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
				var cell = this.GetCell (collectionView, indexPath) as IMovieCollectionViewCell;
				cell.SetSelected (true, true, () => {
					collectionView.CollectionViewLayout.InvalidateLayout ();
					this.OnMovieSelected (this.movies [indexPath.Row]);
				});
			}
		}
		#endregion
	} 
}