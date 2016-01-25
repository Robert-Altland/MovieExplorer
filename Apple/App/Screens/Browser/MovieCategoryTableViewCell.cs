using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieCategoryTableViewCell : UITableViewCell
	{
		public static readonly string ReuseKey = "MovieCategoryItem";

		private MovieCollectionViewSource collectionViewSource;
		private Action<Movie> selectionAction;
		private ConfigurationResponse configuration;
		private MovieCategory category;


		public MovieCategoryTableViewCell (IntPtr handle) : base (handle) {
			
		}

		public override void PrepareForReuse () {
			if (this.collectionViewSource != null)
				this.collectionViewSource.MovieSelected -= this.collectionViewSource_MovieSelected;
			this.collectionViewSource = null;
			this.category = null;
			this.configuration = null;
			this.selectionAction = null;

			base.PrepareForReuse ();
		}

		public void Bind (MovieCategory data, ConfigurationResponse configuration, Action<Movie> selectionAction) {
			this.category = data;
			this.configuration = configuration;
			this.selectionAction = selectionAction;

			this.lblCategoryName.Text = this.category.CategoryName;
			this.collectionViewSource = new MovieCollectionViewSource (this.category.Movies, this.configuration);
			this.collectionViewSource.MovieSelected += this.collectionViewSource_MovieSelected;
			this.cvMovies.Source = this.collectionViewSource;
			this.cvMovies.ReloadData ();
		}

		private void collectionViewSource_MovieSelected (object sender, Movie e) {
			if (this.selectionAction != null)
				this.selectionAction (e);			
		}
	}
}