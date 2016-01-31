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
	[Register ("MovieBrowserViewController")]
	partial class MovieBrowserViewController
	{
		[Outlet]
		UIKit.UICollectionView cvSpotlight { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint cvSpotlightHeightConstraint { get; set; }

		[Outlet]
		UIKit.UITableView tblMovieCategories { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint tblMovieCategoriesHeightConstraint { get; set; }

		[Action ("btnSearch_Click:")]
		partial void btnSearch_Click (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (cvSpotlight != null) {
				cvSpotlight.Dispose ();
				cvSpotlight = null;
			}

			if (tblMovieCategories != null) {
				tblMovieCategories.Dispose ();
				tblMovieCategories = null;
			}

			if (tblMovieCategoriesHeightConstraint != null) {
				tblMovieCategoriesHeightConstraint.Dispose ();
				tblMovieCategoriesHeightConstraint = null;
			}

			if (cvSpotlightHeightConstraint != null) {
				cvSpotlightHeightConstraint.Dispose ();
				cvSpotlightHeightConstraint = null;
			}
		}
	}
}
