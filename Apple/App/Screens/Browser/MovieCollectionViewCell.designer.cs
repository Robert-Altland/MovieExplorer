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
	[Register ("MovieCollectionViewCell")]
	partial class MovieCollectionViewCell
	{
		[Outlet]
		UIKit.UIImageView imgPoster { get; set; }

		[Outlet]
		UIKit.UIView vwFavoriteIndicator { get; set; }
		
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
		}
	}
}
