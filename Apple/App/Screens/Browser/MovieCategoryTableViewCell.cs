using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Movie;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Configuration;
using com.interactiverobert.prototypes.movieexplorer.shared.Services;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieCategoryTableViewCell : UITableViewCell
	{
		#region Private fields
		private UILongPressGestureRecognizer longPressRecognizer;
		private MovieCollectionViewSource collectionViewSource;
		private Action<Movie> selectionAction;
		private ConfigurationResponse configuration;
		private MovieCategory category;
		#endregion

		#region Constructor
		public MovieCategoryTableViewCell (IntPtr handle) : base (handle) {
			
		}
		#endregion

		#region UITableViewCell overrides
		public override void PrepareForReuse () {
			if (this.collectionViewSource != null)
				this.collectionViewSource.MovieSelected -= this.collectionViewSource_MovieSelected;
			Data.Current.FavoriteChanged -= this.favoriteChanged;
			this.collectionViewSource = null;
			this.category = null;
			this.configuration = null;
			this.selectionAction = null;
			this.cvMovies.RemoveGestureRecognizer (this.longPressRecognizer);
			this.longPressRecognizer = null;

			base.PrepareForReuse ();
		}
		#endregion

		#region Public methods
		public void Bind (MovieCategory data, ConfigurationResponse configuration, Action<Movie> selectionAction) {
			this.category = data;
			this.configuration = configuration;
			this.selectionAction = selectionAction;

			this.lblCategoryName.Text = this.category.CategoryName;
			this.collectionViewSource = new MovieCollectionViewSource (this.category.Movies, this.configuration);
			this.collectionViewSource.MovieSelected += this.collectionViewSource_MovieSelected;
			Data.Current.FavoriteChanged += this.favoriteChanged;
			this.longPressRecognizer = new UILongPressGestureRecognizer (() => {
				if (this.longPressRecognizer.NumberOfTouches > 0) {
					var point = this.longPressRecognizer.LocationOfTouch (0, this.cvMovies);
					var indexPath = this.cvMovies.IndexPathForItemAtPoint (point);
					if (indexPath != null) {
						var cell = this.cvMovies.CellForItem (indexPath) as MovieCollectionViewCell;
						if (this.longPressRecognizer.State == UIGestureRecognizerState.Began) {
							cell.SetHighlighted (true, true);
						} else if (this.longPressRecognizer.State == UIGestureRecognizerState.Ended) {
							cell.SetHighlighted (false, true, () => {
								var movie = this.category.Movies [indexPath.Row];
								Data.Current.ToggleFavorite (movie);
							});
						}
					} else {
						foreach (MovieCollectionViewCell cell in this.cvMovies.VisibleCells)
							cell.SetHighlighted (false, false);
					}
				} 
			});
			this.cvMovies.AddGestureRecognizer (this.longPressRecognizer);
			this.cvMovies.Source = this.collectionViewSource;
			this.cvMovies.ReloadData ();
		}
		#endregion

		#region Event handlers
		private void favoriteChanged (object sender, FavoriteChangedEventArgs e) {
			if (this.category.CategoryName == "Your Favorites") {
				this.cvMovies.ReloadData ();
			} else {
				if (this.category.Movies.Find (x => x.Id == e.FavoriteMovie.Id) != null)
					this.cvMovies.ReloadData ();
			}
		}

		private void collectionViewSource_MovieSelected (object sender, Movie e) {
			if (this.selectionAction != null)
				this.selectionAction (e);			
		}
		#endregion
	}
}