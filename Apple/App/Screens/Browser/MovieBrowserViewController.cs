using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Foundation;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieBrowserViewController : UIViewController
	{
		#region Private fields
		private MovieCategoryTableViewSource tableViewSource;
		private List<MovieCategory> data;
		private ConfigurationResponse configuration;
		private Movie selectedMovie;
		private NSObject favoritesChangedNotification;
		#endregion

		#region Constructor
		public MovieBrowserViewController (IntPtr handle) : base (handle) {
			
		}
		#endregion

		#region View lifecycle
		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.tblMovieCategories.ContentInset = this.tblMovieCategories.ScrollIndicatorInsets;
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			if (this.tableViewSource != null)
				this.tableViewSource.MovieSelected += this.tableViewSource_MovieSelected;

			if (this.favoritesChangedNotification == null) {
				this.favoritesChangedNotification = NSNotificationCenter.DefaultCenter.AddObserver (new NSString ("FavoriteListChanged"), (notification) => {
					this.tblMovieCategories.ReloadData();
				});
			}

			Api.Current.GetConfigurationAsync (config => {
				this.configuration = config;
				Api.Current.GetMoviesByCategoryAsync (response => {
					this.data = response;
					if (this.tableViewSource == null) {
						this.tableViewSource = new MovieCategoryTableViewSource (this.configuration, this.data);
						this.tableViewSource.MovieSelected += this.tableViewSource_MovieSelected;
						this.tblMovieCategories.Source = this.tableViewSource;
						this.tblMovieCategories.ReloadData ();
					} else {
						this.tableViewSource.Reload (this.data);
						this.tblMovieCategories.ReloadData ();
					}
				});
			});
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);

			if (this.tableViewSource != null)
				this.tableViewSource.MovieSelected -= this.tableViewSource_MovieSelected;

			if (this.favoritesChangedNotification != null) {
				NSNotificationCenter.DefaultCenter.RemoveObserver (this.favoritesChangedNotification);
				this.favoritesChangedNotification = null;
			}
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
		private void tableViewSource_MovieSelected (object sender, Movie e) {
			this.selectedMovie = e;
			this.PerformSegue ("ShowMovieDetail", this);
		}

		partial void btnSearch_Click (NSObject sender) {
			// TODO: Show search 
		}
		#endregion
	}
}