using System;

using Foundation;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared;
using MonoTouch.Dialog.Utilities;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieCollectionViewCell : UICollectionViewCell, IImageUpdated
	{
		public static readonly string ReuseKey = "MovieCell";

		private ConfigurationResponse configuration;
		private Movie data;

		public MovieCollectionViewCell (IntPtr handle) : base (handle) {
			
		}

		#region IImageUpdated implementation
		public void UpdatedImage (Uri uri) {
			if (this.data != null && this.configuration != null) {
				var imageUri = new Uri (String.Concat (this.configuration.Images.BaseUrl, this.configuration.Images.PosterSizes [0], this.data.PosterPath));
				if (String.Compare(imageUri.AbsoluteUri.ToLower(), uri.AbsoluteUri.ToLower()) == 0)
					this.imgPoster.Image = ImageLoader.DefaultRequestImage (uri, this);
			}
		}
		#endregion

		public void Bind (Movie movie, ConfigurationResponse configuration) {
			this.data = movie;
			this.configuration = configuration;

			this.vwFavoriteIndicator.Hidden = !Data.Current.IsInFavorites (this.data);
			var imageUri = new Uri (String.Concat (this.configuration.Images.BaseUrl, this.configuration.Images.PosterSizes [0], this.data.PosterPath));
			this.imgPoster.Image = ImageLoader.DefaultRequestImage (imageUri, this);
		}
	}
}