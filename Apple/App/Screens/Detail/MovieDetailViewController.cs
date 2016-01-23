using System;

using Foundation;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieDetailViewController : UIViewController
	{
		public Movie Data { get; set; }

		public MovieDetailViewController (IntPtr handle) : base (handle) {
			
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			if (this.Data != null) {
				this.NavigationItem.Title = this.Data.Title;
			}
		}
	}
}
