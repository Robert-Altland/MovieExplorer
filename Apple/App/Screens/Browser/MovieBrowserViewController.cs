using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CoreGraphics;
using Foundation;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Configuration;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Movie;
using com.interactiverobert.prototypes.movieexplorer.shared.Services;
using com.interactiverobert.prototypes.movieexplorer.apple.lib.Resources;
using com.interactiverobert.prototypes.movieexplorer.apple.lib.Contracts;
using com.interactiverobert.prototypes.movieexplorer.apple.lib;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieBrowserViewController : UIViewController, IUISearchResultsUpdating
	{
		#region Private fields
		private UISearchController search;
		private MovieCategoryTableViewSource tableViewSource;
		private MovieCollectionViewSource spotlightSource;

		private ConfigurationResponse configuration;
		private List<MovieCategory> categories;
		private List<Movie> spotlight;
		private Movie selectedMovie;

		private UILongPressGestureRecognizer longPressRecognizer;
		#endregion

		#region Constructor
		public MovieBrowserViewController (IntPtr handle) : base (handle) {
			
		}
		#endregion

		#region View lifecycle
		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.longPressRecognizer = new UILongPressGestureRecognizer (() => {
				if (this.longPressRecognizer.NumberOfTouches > 0) {
					var point = this.longPressRecognizer.LocationOfTouch (0, this.cvSpotlight);
					var indexPath = this.cvSpotlight.IndexPathForItemAtPoint (point);
					if (indexPath != null) {
						var cell = this.cvSpotlight.CellForItem (indexPath) as IMovieCell;
						if (this.longPressRecognizer.State == UIGestureRecognizerState.Began) {
							cell.SetHighlighted (true, true);
						} else if (this.longPressRecognizer.State == UIGestureRecognizerState.Ended) {
							cell.SetHighlighted (false, true, () => {
								var movie = this.spotlight [indexPath.Row];
								Data.Current.ToggleFavorite (movie);
							});
						}
					} else {
						foreach (MovieCollectionViewCell cell in this.cvSpotlight.VisibleCells)
							cell.SetHighlighted (false, false);
					}
				} 
			});
			this.cvSpotlight.AddGestureRecognizer (this.longPressRecognizer);

			var searchResults = PresentationUtility.CreateFromStoryboard<SearchResultsTableViewController> ("SearchResults");
			this.search = new UISearchController (searchResults);
			this.search.SearchResultsUpdater = this;
			this.search.DimsBackgroundDuringPresentation = false;
			this.search.DefinesPresentationContext = true;
			searchResults.TableView.TableHeaderView = this.search.SearchBar;
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			Data.Current.FavoriteChanged += this.favoriteChanged;
			this.willAppear ();
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);

			Data.Current.FavoriteChanged -= this.favoriteChanged;
		}

		#endregion

		#region Private methods
		private void updateSpotlightItemSize () {
			this.cvSpotlightHeightConstraint.Constant = this.tblMovieCategories.Frame.Width*9/16;
			if (UIDevice.CurrentDevice.Orientation.IsLandscape ())
				this.cvSpotlightHeightConstraint.Constant = UIScreen.MainScreen.ApplicationFrame.Height / 1.75f;
			var itemSize = new CGSize (this.tblMovieCategories.Frame.Width, this.cvSpotlightHeightConstraint.Constant);
			var flowLayout = this.cvSpotlight.CollectionViewLayout as UICollectionViewFlowLayout;
			flowLayout.ItemSize = itemSize;
			flowLayout.InvalidateLayout ();
		}

		private async void willAppear() {
			this.configuration = await Data.Current.GetConfigurationAsync ();
			this.categories = await Data.Current.GetMoviesByCategoryAsync ();
			this.spotlight = await Data.Current.GetSpotlightMoviesAsync ();

			if (this.tableViewSource == null) {
				this.tableViewSource = new MovieCategoryTableViewSource (this.configuration, this.categories);
				this.tableViewSource.MovieSelected += this.source_MovieSelected;
				this.tblMovieCategories.Source = this.tableViewSource;
			} else {
				this.tableViewSource.Reload (this.categories);
			}
			this.tblMovieCategories.ReloadData ();

			if (this.spotlightSource == null) {
				this.spotlightSource = new MovieCollectionViewSource (this.spotlight, this.configuration);
				this.spotlightSource.MovieSelected += this.source_MovieSelected;
				this.cvSpotlight.Source = this.spotlightSource;

			} else {
				this.spotlightSource.Reload (this.spotlight);
			}

			this.tblMovieCategoriesHeightConstraint.Constant = this.tblMovieCategories.ContentSize.Height;
			this.updateSpotlightItemSize ();
			this.cvSpotlight.ReloadData ();
		}
		#endregion

		public async void UpdateSearchResultsForSearchController (UISearchController searchController) {
			var tableController = this.search.SearchResultsController as SearchResultsTableViewController;
			tableController.Configuration = this.configuration;
			var result = await Data.Current.SearchAsync (searchController.SearchBar.Text);
			tableController.Reload (result);

		}

		#region Rotation handling
		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation) {
			base.DidRotate (fromInterfaceOrientation);

			this.updateSpotlightItemSize ();
			this.cvSpotlight.ReloadData ();
		}
		#endregion

		#region Navigation handling
		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender) {
			var destination = segue.DestinationViewController as MovieDetailViewController;
			destination.MovieDetail = this.selectedMovie;
			destination.Configuration = this.configuration;

			base.PrepareForSegue (segue, sender);
		}
		#endregion

		#region Event handlers
		private void favoriteChanged (object sender, com.interactiverobert.prototypes.movieexplorer.shared.FavoriteChangedEventArgs e) {
			if (this.spotlight.Find (x => x.Id == e.FavoriteMovie.Id) != null)
				this.cvSpotlight.ReloadData ();
		}

		private void source_MovieSelected (object sender, Movie e) {
			this.selectedMovie = e;
			this.PerformSegue (AppleConstants.ShowMovieDetail_SegueName, this);
		}

		partial void btnSearch_Click (NSObject sender) {
			// TODO: Show search 
		}
		#endregion
	}
}