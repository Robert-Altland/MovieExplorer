using System;
using System.Collections.Generic;

using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public class MovieTableViewSource : UITableViewSource
	{
		private MovieTableViewController controller;
		private List<Movie> movies;

		public Movie SelectedMovie { get; private set; }


		public MovieTableViewSource (MovieTableViewController controller, List<Movie> movies) {
			this.controller = controller;
			this.movies = movies;
		}

		public void Reload (List<Movie> movies) {
			this.movies = movies;
		}

		#region implemented abstract members of UITableViewSource
		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath) {
			var dataItem = this.movies [indexPath.Row];
			var cell = tableView.DequeueReusableCell (MovieTableViewCell.ReuseKey) as MovieTableViewCell;
			cell.Bind(dataItem);
			return cell;
		}

		public override nint RowsInSection (UITableView tableview, nint section) {
			if (this.movies == null)
				return 0;
			return this.movies.Count;
		}

		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath) {
			this.SelectedMovie = this.movies [indexPath.Row];
			this.controller.PerformSegue ("ShowMovieDetail", this.controller);
		}
		#endregion
	}
}