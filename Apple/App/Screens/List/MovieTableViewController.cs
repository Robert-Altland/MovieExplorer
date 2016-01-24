using System;

using Foundation;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieTableViewController : UITableViewController
	{
		private MovieTableViewSource tableViewSource;

		public MovieTableViewController (IntPtr handle) : base (handle) {
			
		}
			
		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.NavigationItem.BackBarButtonItem = new UIBarButtonItem (String.Empty, UIBarButtonItemStyle.Plain, null, null);
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			Api.Current.GetMoviesCompleted += this.getMoviesCompleted;
			Api.Current.GetMoviesAsync ();
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);

			Api.Current.GetMoviesCompleted -= this.getMoviesCompleted;
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender) {
			var destination = segue.DestinationViewController as MovieDetailViewController;
			destination.Data = this.tableViewSource.SelectedMovie;
			base.PrepareForSegue (segue, sender);
		}

		private void getMoviesCompleted (object sender, MovieDiscoverResponse e) {
			if (this.tableViewSource == null) {
				this.tableViewSource = new MovieTableViewSource (this, e.Results);
				this.tblMovies.Source = this.tableViewSource;
				this.tblMovies.ReloadData ();
			} else {
				this.tableViewSource.Reload (e.Results);
				this.tblMovies.ReloadData ();
			}
		}
	}
}