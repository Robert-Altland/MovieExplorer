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
	[Register ("SearchResultMovieCell")]
	partial class SearchResultMovieCell
	{
		[Outlet]
		UIKit.UIImageView imgPoster { get; set; }

		[Outlet]
		UIKit.UILabel lblReleaseDate { get; set; }

		[Outlet]
		UIKit.UILabel lblTitle { get; set; }

		[Outlet]
		UIKit.UIView vwFavoriteIndicator { get; set; }

		[Outlet]
		UIKit.UIView vwHighlight { get; set; }

		[Outlet]
		UIKit.UIView vwSelected { get; set; }

		[Outlet]
		UIKit.UIView vwVoteAverageContainer { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (imgPoster != null) {
				imgPoster.Dispose ();
				imgPoster = null;
			}

			if (vwFavoriteIndicator != null) {
				vwFavoriteIndicator.Dispose ();
				vwFavoriteIndicator = null;
			}

			if (vwHighlight != null) {
				vwHighlight.Dispose ();
				vwHighlight = null;
			}

			if (vwSelected != null) {
				vwSelected.Dispose ();
				vwSelected = null;
			}

			if (lblReleaseDate != null) {
				lblReleaseDate.Dispose ();
				lblReleaseDate = null;
			}

			if (lblTitle != null) {
				lblTitle.Dispose ();
				lblTitle = null;
			}

			if (vwVoteAverageContainer != null) {
				vwVoteAverageContainer.Dispose ();
				vwVoteAverageContainer = null;
			}
		}
	}
}
