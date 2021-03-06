using System;

using MonoTouch.Dialog.Utilities;
using Foundation;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Configuration;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Movie;
using com.interactiverobert.prototypes.movieexplorer.shared.Services;
using com.interactiverobert.prototypes.movieexplorer.apple.lib.Contracts;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieCollectionViewCell : UICollectionViewCell, IImageUpdated, IMovieCell
	{
		#region Private fields
		private ConfigurationResponse configuration;
		private Movie data;
		#endregion

		#region Constructor
		public MovieCollectionViewCell (IntPtr handle) : base (handle) {
			
		}
		#endregion

		#region UICollectionViewCell overrides
		public override void PrepareForReuse () {
			this.data = null;
			this.configuration = null;
			this.Selected = false;

			base.PrepareForReuse ();
		}
		#endregion

		#region IImageUpdated implementation
		public void UpdatedImage (Uri uri) {
			if (this.data != null && this.configuration != null) {
				var imageUri = new Uri (String.Concat (this.configuration.Images.BaseUrl, this.configuration.Images.PosterSizes [0], this.data.PosterPath));
				if (String.Compare(imageUri.AbsoluteUri.ToLower(), uri.AbsoluteUri.ToLower()) == 0)
					this.imgPoster.Image = ImageLoader.DefaultRequestImage (uri, this);
			}
		}
		#endregion

		#region Public methods
		public void SetSelected (bool isSelected, bool animated, Action completionBlock) {
			this.Selected = isSelected;
			UIView.Animate (
				animated ? 0.3f : 0.0f, 
				() => this.vwSelected.Alpha = !isSelected ? 0.0f : 0.3f,
				() => {
					if (completionBlock != null)
						completionBlock.Invoke ();
				});
		}

		public void SetHighlighted (bool isHighlighted, bool animated = false, Action completionBlock = null) {
			UIView.Animate (
				animated ? 0.3f : 0.0f, 
				() => this.vwHighlight.Alpha = isHighlighted ? 0.3f : 0.0f,
				() => {
					if (completionBlock != null)
						completionBlock.Invoke ();
				});
		}

		public void Bind (Movie movie, ConfigurationResponse configuration) {
			this.data = movie;
			this.configuration = configuration;

			this.vwFavoriteIndicator.Hidden = !Data.Current.IsInFavorites (this.data);
			var imageUri = new Uri (String.Concat (this.configuration.Images.BaseUrl, this.configuration.Images.PosterSizes [0], this.data.PosterPath));
			this.imgPoster.Image = ImageLoader.DefaultRequestImage (imageUri, this);
		}
		#endregion
	}
}