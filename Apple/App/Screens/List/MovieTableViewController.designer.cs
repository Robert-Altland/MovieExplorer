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
	[Register ("MovieTableViewController")]
	partial class MovieTableViewController
	{
		[Outlet]
		UIKit.UITableView tblMovies { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tblMovies != null) {
				tblMovies.Dispose ();
				tblMovies = null;
			}
		}
	}
}
