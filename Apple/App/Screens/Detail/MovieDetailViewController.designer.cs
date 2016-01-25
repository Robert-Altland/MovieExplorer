// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	[Register ("MovieDetailViewController")]
	partial class MovieDetailViewController
	{
		[Outlet]
		UIKit.UIButton btnToggleSave { get; set; }

		[Outlet]
		UIKit.UIImageView imgBackground { get; set; }

		[Outlet]
		UIKit.UIImageView imgPoster { get; set; }

		[Outlet]
		UIKit.UILabel lblOverview { get; set; }

		[Outlet]
		UIKit.UILabel lblPopularity { get; set; }

		[Outlet]
		UIKit.UILabel lblReleaseDate { get; set; }

		[Outlet]
		UIKit.UILabel lblTitle { get; set; }

		[Outlet]
		UIKit.UILabel lblVoteAverage { get; set; }

		[Outlet]
		UIKit.UILabel lblVoteCount { get; set; }

		[Action ("btnClose_Click:")]
		partial void btnClose_Click (Foundation.NSObject sender);

		[Action ("btnToggleSave_Click:")]
		partial void btnToggleSave_Click (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (imgBackground != null) {
				imgBackground.Dispose ();
				imgBackground = null;
			}

			if (imgPoster != null) {
				imgPoster.Dispose ();
				imgPoster = null;
			}

			if (lblOverview != null) {
				lblOverview.Dispose ();
				lblOverview = null;
			}

			if (lblPopularity != null) {
				lblPopularity.Dispose ();
				lblPopularity = null;
			}

			if (lblReleaseDate != null) {
				lblReleaseDate.Dispose ();
				lblReleaseDate = null;
			}

			if (lblTitle != null) {
				lblTitle.Dispose ();
				lblTitle = null;
			}

			if (lblVoteAverage != null) {
				lblVoteAverage.Dispose ();
				lblVoteAverage = null;
			}

			if (lblVoteCount != null) {
				lblVoteCount.Dispose ();
				lblVoteCount = null;
			}

			if (btnToggleSave != null) {
				btnToggleSave.Dispose ();
				btnToggleSave = null;
			}
		}
	}
}
