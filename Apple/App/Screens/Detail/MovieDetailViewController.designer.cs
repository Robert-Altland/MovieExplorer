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
		UIKit.UIButton btnBack { get; set; }

		[Outlet]
		UIKit.UIButton btnPlay { get; set; }

		[Outlet]
		UIKit.UIButton btnToggleSave { get; set; }

		[Outlet]
		UIKit.UICollectionView cvSimilarMovies { get; set; }

		[Outlet]
		UIKit.UIImageView imgBackground { get; set; }

		[Outlet]
		UIKit.UIImageView imgPoster { get; set; }

		[Outlet]
		UIKit.UILabel lblReleaseDate { get; set; }

		[Outlet]
		UIKit.UILabel lblTitle { get; set; }

		[Outlet]
		UIKit.UILabel lblVoteCount { get; set; }

		[Outlet]
		UIKit.UITextView tvOverview { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint tvOverviewHeightConstraint { get; set; }

		[Outlet]
		UIKit.UIView vwSimilarMovies { get; set; }

		[Outlet]
		UIKit.UIView vwVoteAverageContainer { get; set; }

		[Action ("btnBack_Click:")]
		partial void btnBack_Click (Foundation.NSObject sender);

		[Action ("btnClose_Click:")]
		partial void btnClose_Click (Foundation.NSObject sender);

		[Action ("btnPlay_Click:")]
		partial void btnPlay_Click (Foundation.NSObject sender);

		[Action ("btnToggleSave_Click:")]
		partial void btnToggleSave_Click (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (btnBack != null) {
				btnBack.Dispose ();
				btnBack = null;
			}

			if (btnPlay != null) {
				btnPlay.Dispose ();
				btnPlay = null;
			}

			if (btnToggleSave != null) {
				btnToggleSave.Dispose ();
				btnToggleSave = null;
			}

			if (cvSimilarMovies != null) {
				cvSimilarMovies.Dispose ();
				cvSimilarMovies = null;
			}

			if (imgBackground != null) {
				imgBackground.Dispose ();
				imgBackground = null;
			}

			if (imgPoster != null) {
				imgPoster.Dispose ();
				imgPoster = null;
			}

			if (lblReleaseDate != null) {
				lblReleaseDate.Dispose ();
				lblReleaseDate = null;
			}

			if (lblTitle != null) {
				lblTitle.Dispose ();
				lblTitle = null;
			}

			if (lblVoteCount != null) {
				lblVoteCount.Dispose ();
				lblVoteCount = null;
			}

			if (tvOverview != null) {
				tvOverview.Dispose ();
				tvOverview = null;
			}

			if (tvOverviewHeightConstraint != null) {
				tvOverviewHeightConstraint.Dispose ();
				tvOverviewHeightConstraint = null;
			}

			if (vwSimilarMovies != null) {
				vwSimilarMovies.Dispose ();
				vwSimilarMovies = null;
			}

			if (vwVoteAverageContainer != null) {
				vwVoteAverageContainer.Dispose ();
				vwVoteAverageContainer = null;
			}
		}
	}
}
