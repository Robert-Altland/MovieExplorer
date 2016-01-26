using System;

using MonoTouch.Dialog.Utilities;
using Foundation;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieCollectionViewCell : UICollectionViewCell, IImageUpdated
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
			this.SetHighlighted (false, false);
			this.Selected = false;

			base.PrepareForReuse ();
		}

		public override bool Selected {
			get {
				return base.Selected;
			}
			set {
				if (base.Selected != value) {
					base.Selected = value;
					UIView.Animate(0.0f, () => this.vwSelected.Alpha = !value ? 0.0f : 0.3f);
				}
			}
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
		public void SetHighlighted (bool highlighted, bool animated = true) {
			var duration = animated ? 0.2f : 0.0f;
			this.Highlighted = highlighted;
			UIView.Animate(duration, () => this.vwHighlight.Alpha = !highlighted ? 0.0f : 0.3f);
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