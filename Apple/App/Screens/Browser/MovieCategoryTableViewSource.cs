using System;
using System.Collections.Generic;

using UIKit;
using Foundation;

using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Configuration;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Movie;
using com.interactiverobert.prototypes.movieexplorer.apple.lib.Resources;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public class MovieCategoryTableViewSource : UITableViewSource
	{
		#region Private fields
		private ConfigurationResponse configuration;
		private List<MovieCategory> categories;
		#endregion

		#region Constructor
		public MovieCategoryTableViewSource (ConfigurationResponse configuration, List<MovieCategory> categories) {
			this.configuration = configuration;
			this.categories = categories;
		}
		#endregion

		#region Public methods
		public void Reload (List<MovieCategory> categories) {
			this.categories = categories;
		}
		#endregion
			
		#region implemented abstract members of UITableViewSource
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath) {
			var dataItem = this.categories [indexPath.Row];
			var cell = tableView.DequeueReusableCell (AppleConstants.MovieCategoryTableViewCell_ReuseKey) as MovieCategoryTableViewCell;
			cell.Bind (dataItem, this.configuration, m => {
				this.OnMovieSelected (m);
			});
			return cell;
		}

		public override nint RowsInSection (UITableView tableview, nint section) {
			return this.categories.Count;
		}
		#endregion

		#region Events
		public event EventHandler<Movie> MovieSelected;
		protected void OnMovieSelected (Movie movie) {
			if (this.MovieSelected != null)
				this.MovieSelected (this, movie);
		}
		#endregion
	}
}