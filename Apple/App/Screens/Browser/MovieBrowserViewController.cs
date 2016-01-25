using System;

using Foundation;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieBrowserViewController : UIViewController
	{
		private MovieCategoryTableViewSource tableViewSource;
		private ConfigurationResponse configuration;
		private Movie selectedMovie;

		public MovieBrowserViewController (IntPtr handle) : base (handle) {
			
		}
			
		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.tblMovieCategories.RowHeight = 200;
			this.tblMovieCategories.EstimatedRowHeight = 200;
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			Api.Current.GetConfigurationCompleted += this.getConfigurationCompleted;
			Api.Current.GetMoviesCompleted += this.getMoviesCompleted;
			Data.Current.GetFavoritesCompleted += this.getFavoritesCompleted;

			if (this.tableViewSource != null)
				this.tableViewSource.MovieSelected += this.tableViewSource_MovieSelected;

			Api.Current.GetConfigurationAsync ();
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);

			Api.Current.GetConfigurationCompleted -= this.getConfigurationCompleted;
			Api.Current.GetMoviesCompleted -= this.getMoviesCompleted;
			Data.Current.GetFavoritesCompleted -= this.getFavoritesCompleted;

			if (this.tableViewSource != null)
				this.tableViewSource.MovieSelected -= this.tableViewSource_MovieSelected;
		}


		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender) {
			var destination = segue.DestinationViewController as MovieDetailViewController;
			destination.MovieDetail = this.selectedMovie;
			destination.Configuration = this.configuration;
			base.PrepareForSegue (segue, sender);
		}



		private void getConfigurationCompleted (object sender, ConfigurationResponse e) {
			this.configuration = e;
			Api.Current.GetMoviesAsync ();
		}

		private List<MovieCategory> categorizeResults (MovieDiscoverResponse e, List<Movie> list)  {
			var result = new List<MovieCategory> ();
			result.Add (new MovieCategory ("All Movies", e.Results));
			result.Add (new MovieCategory ("Your Favorites", list));
			return result;
		}

		private MovieDiscoverResponse movieDiscoverResponse;
		private void getMoviesCompleted (object sender, MovieDiscoverResponse e) {
			this.movieDiscoverResponse = e;
			Data.Current.GetFavoritesAsync ();
		}

		private void getFavoritesCompleted (object sender, List<Movie> e) {
			var data = this.categorizeResults (this.movieDiscoverResponse, e);
			if (this.tableViewSource == null) {
				this.tableViewSource = new MovieCategoryTableViewSource (this, this.configuration, data);
				this.tableViewSource.MovieSelected += this.tableViewSource_MovieSelected;
				this.tblMovieCategories.Source = this.tableViewSource;
				this.tblMovieCategories.ReloadData ();
			} else {
				this.tableViewSource.Reload (data);
				this.tblMovieCategories.ReloadData ();
			}
		}

		private void tableViewSource_MovieSelected (object sender, Movie e) {
			this.selectedMovie = e;
			this.PerformSegue ("ShowMovieDetail", this);
		}

		partial void btnSearch_Click (NSObject sender) {
			// TODO: Show search 
		}
	}
}
