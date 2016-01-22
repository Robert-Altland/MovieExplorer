using System;

using Foundation;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared;


namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieTableViewController : UITableViewController
	{
		public MovieTableViewController (IntPtr handle) : base (handle) {
			
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			Api.GetMoviesCompleted += this.getMoviesCompleted;
			Api.GetMoviesAsync ();
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);

			Api.GetMoviesCompleted -= this.getMoviesCompleted;
		}

		private void getMoviesCompleted (object sender, MovieDiscoverResponse e) {
			Console.WriteLine (e.Results);			
		}
	}
}