using System;

using PatridgeDev;
using CoreGraphics;
using Foundation;
using UIKit;
using MonoTouch.Dialog.Utilities;

using com.interactiverobert.prototypes.movieexplorer.apple.lib.Contracts;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Movie;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Configuration;
using com.interactiverobert.prototypes.movieexplorer.shared.Services;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class SearchResultMovieCell : UITableViewCell, IImageUpdated, IMovieCell
	{
		#region Private fields
		private ConfigurationResponse configuration;
		private Movie data;
		private PDRatingView ratingView;
		#endregion

		public SearchResultMovieCell (IntPtr handle) : base (handle) {
			
		}

		#region IImageUpdated implementation
		public void UpdatedImage (Uri uri) {
			if (this.data != null && this.configuration != null) {
				var spotlightUri = new Uri (String.Concat (this.configuration.Images.BaseUrl, this.configuration.Images.BackdropSizes [0], this.data.BackdropPath));
				if (String.Compare(spotlightUri.AbsoluteUri.ToLower(), uri.AbsoluteUri.ToLower()) == 0)
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
			var spotlightUri = new Uri (String.Concat (this.configuration.Images.BaseUrl, this.configuration.Images.BackdropSizes [0], this.data.BackdropPath));
			this.imgPoster.Image = ImageLoader.DefaultRequestImage (spotlightUri, this);

			this.lblTitle.Text = this.data.Title;
			this.lblReleaseDate.Text = String.Format ("Released in {0}", this.data.ReleaseDate.Year);

			var ratingConfig = new RatingConfig(UIImage.FromBundle("star_empty"), UIImage.FromBundle("star_filled"), UIImage.FromBundle("star_filled"));
			var averageRating = (decimal)this.data.VoteAverage / 2;
			this.ratingView = new PDRatingView (new CGRect(0f, 0f, this.vwVoteAverageContainer.Frame.Width, this.vwVoteAverageContainer.Frame.Height), ratingConfig, averageRating);
			this.vwVoteAverageContainer.Add(this.ratingView);
		}
		#endregion
	}
}
