using System;

using Foundation;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieTableViewCell : UITableViewCell
	{
		public static readonly string ReuseKey = "MovieItem";

		public MovieTableViewCell (IntPtr handle) : base (handle) {
			
		}

		public void Bind (Movie movie) {
			this.lblTitle.Text = movie.Title;
		}
	}
}