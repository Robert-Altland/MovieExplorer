using System;
using com.interactiverobert.prototypes.movieexplorer.shared;
using System.Collections.Generic;
using UIKit;
using Foundation;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public class MovieCategoryTableViewSource : UITableViewSource
	{
		private MovieBrowserViewController controller;
		private ConfigurationResponse configuration;
		private List<MovieCategory> categories;

		public MovieCategoryTableViewSource (MovieBrowserViewController controller, ConfigurationResponse configuration, List<MovieCategory> categories) {
			this.controller = controller;
			this.configuration = configuration;
			this.categories = categories;
		}


		public void Reload (List<MovieCategory> categories) {
			this.categories = categories;
		}


		public event EventHandler<Movie> MovieSelected;
		protected void OnMovieSelected (Movie movie) {
			if (this.MovieSelected != null)
				this.MovieSelected (this, movie);
		}

		#region implemented abstract members of UITableViewSource
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath) {
			var dataItem = this.categories [indexPath.Row];
			var cell = tableView.DequeueReusableCell (MovieCategoryTableViewCell.ReuseKey) as MovieCategoryTableViewCell;
			cell.Bind (dataItem, this.configuration, (Movie e) => {
				this.OnMovieSelected (e);			
			});
			return cell;
		}

		public override nint RowsInSection (UITableView tableview, nint section) {
			return this.categories.Count;
		}
		#endregion
	}
}

