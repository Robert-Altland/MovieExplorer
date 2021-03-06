// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Movie;
using System.Collections.Generic;
using com.interactiverobert.prototypes.movieexplorer.apple.lib.Resources;
using com.interactiverobert.prototypes.movieexplorer.apple.lib.Contracts;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Configuration;
using com.interactiverobert.prototypes.movieexplorer.apple.lib;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class SearchResultsTableViewController : UITableViewController
	{
		private SearchResultsTableViewSource tableViewSource;

		public ConfigurationResponse Configuration { get; set; }

		public SearchResultsTableViewController (IntPtr handle) : base (handle) {
			
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.tableViewSource = new SearchResultsTableViewSource (this, new List<Movie> (), this.Configuration);
		}

		public void Reload (List<Movie> newItems) {
			this.tableViewSource.Reload (newItems);
			this.TableView.ReloadData ();
		}

		class SearchResultsTableViewSource : UITableViewSource
		{		
			private List<Movie> items;
			private ConfigurationResponse configuration;
			private SearchResultsTableViewController controller;

			public SearchResultsTableViewSource (SearchResultsTableViewController controller, List<Movie> newItems, ConfigurationResponse configuration) {
				this.controller = controller;
				this.configuration = configuration;
				this.items = newItems;
			}
				
			#region implemented abstract members of UITableViewSource
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath) {
				var dataItem = this.items [indexPath.Row];
				var cell = tableView.DequeueReusableCell (AppleConstants.MovieCollectionViewCell_ReuseKey) as IMovieCell;
				cell.Bind (dataItem, this.configuration);
				return cell as UITableViewCell;
			}
			public override nint RowsInSection (UITableView tableview, nint section) {
				return this.items == null ? 0 : this.items.Count;
			}
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath) {
				var newDetail = PresentationUtility.CreateFromStoryboard<MovieDetailViewController> ("MovieDetail");
				newDetail.MovieDetail = this.items[indexPath.Row];
				newDetail.Configuration = this.configuration;
				this.controller.NavigationController.PushViewController (newDetail, true);
			}
			#endregion

			public void Reload (List<Movie> newItems) {
				this.items = newItems;
			}
		}
	}
}