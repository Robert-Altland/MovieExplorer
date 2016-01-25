using System;
using System.Linq;

using Foundation;
using CoreImage;
using CoreGraphics;
using MonoTouch.Dialog.Utilities;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared;
using System.Threading.Tasks;
using System.Threading;
using com.interactiverobert.prototypes.movieexplorer.apple.lib;
using System.Collections.Generic;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieDetailViewController : UIViewController, IImageUpdated
	{
		private static Dictionary<int, UIImage> backgroundImages = new Dictionary<int, UIImage>();

		#region Public properties
		public Movie MovieDetail { get; set; }
		public ConfigurationResponse Configuration { get; set; }
		#endregion

		#region Constructor
		public MovieDetailViewController (IntPtr handle) : base (handle) {
			
		}
		#endregion

		#region View lifecycle
		public override void ViewDidLoad () {
			base.ViewDidLoad ();
			this.imgBackground.Alpha = 0.3f;
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			if (this.MovieDetail != null) {
				// TODO:
				// movie/this.MovieDetail.Id/videos
				// movie/this.MovieDetail.Id/similar
				// movie/this.MovieDetail.Id/reviews
				// movie/this.MovieDetail.Id/credits

				this.lblTitle.Text = this.MovieDetail.Title;
				this.lblReleaseDate.Text = this.MovieDetail.ReleaseDate.ToShortDateString ();
				this.lblPopularity.Text = this.MovieDetail.Popularity.ToString ();
				this.lblVoteAverage.Text = this.MovieDetail.VoteAverage.ToString ();
				this.lblVoteCount.Text = this.MovieDetail.VoteCount.ToString ();
				this.lblOverview.Text = this.MovieDetail.Overview;

				var posterUri = new Uri (String.Concat (this.Configuration.Images.BaseUrl, this.Configuration.Images.PosterSizes[1], this.MovieDetail.PosterPath));
				this.imgPoster.Image = ImageLoader.DefaultRequestImage (posterUri, this);

				UIImage cachedImage;
				this.imgBackground.Alpha = 0;
				if (MovieDetailViewController.backgroundImages.TryGetValue (this.MovieDetail.Id, out cachedImage)) {
					this.imgBackground.Image = cachedImage;
					this.pulseBackground (0.3f, this.pulseBackground);
				} else {
					Task.Factory.StartNew (() => {
						var backgroundUri = new Uri (String.Concat (this.Configuration.Images.BaseUrl, this.Configuration.Images.BackdropSizes [0], this.MovieDetail.BackdropPath));
						Thread.Sleep (TimeSpan.FromMilliseconds (500));
						this.InvokeOnMainThread (() => {
							var image = ImageManipulation.ScaleAndBlurImage (this.squareSize (this.imgBackground.Frame.Size), ImageLoader.DefaultRequestImage (backgroundUri, this));
							if (MovieDetailViewController.backgroundImages.ContainsKey (this.MovieDetail.Id)) 
								MovieDetailViewController.backgroundImages[this.MovieDetail.Id] = image;
							else 
								MovieDetailViewController.backgroundImages.Add (this.MovieDetail.Id, image);
							this.imgBackground.Image = image;
							this.pulseBackground (0.3f, this.pulseBackground);
						});
					});
				}

				this.updateSaveButtonState ();
			}
		}
		#endregion

		private double nextDouble(Random rng, double min, double max) {
			return min + (rng.NextDouble() * (max - min));
		}

		private void pulseBackground () {
			var randomAlpha = new Random ();
			var alpha = this.nextDouble (randomAlpha, 0.2, 0.5);
			this.pulseBackground ((float)alpha, this.pulseBackground);
		}
		private void pulseBackground (float alpha, Action completionAction) {
			var randomDuration = new Random();
			var duration = randomDuration.Next (2, 5);
			this.InvokeOnMainThread (() => 
				UIView.Animate (duration, () => this.imgBackground.Alpha = alpha, () => {
				if (completionAction != null)
					completionAction.Invoke ();
			}));
		}

		#region IImageUpdated implementation
		public void UpdatedImage (Uri uri) {
			if (this.MovieDetail != null && this.Configuration != null) {
				var posterUri = new Uri (String.Concat (this.Configuration.Images.BaseUrl, this.Configuration.Images.PosterSizes[1], this.MovieDetail.PosterPath));
				var backgroundUri = new Uri (String.Concat (this.Configuration.Images.BaseUrl, this.Configuration.Images.BackdropSizes[0], this.MovieDetail.BackdropPath));
				if (String.Compare (posterUri.AbsoluteUri.ToLower (), uri.AbsoluteUri.ToLower ()) == 0) {
					this.updateImageViewImage (uri, this.imgPoster, false, false);
				}
				if (String.Compare (backgroundUri.AbsoluteUri.ToLower (), uri.AbsoluteUri.ToLower ()) == 0) {
					this.updateImageViewImage (uri, this.imgBackground, true, true);
				}
			}
		}
		#endregion

		#region Private methods
		private CGSize squareSize (CGSize size) {
			return new CGSize(size.Width > size.Height ? size.Width : size.Height, size.Width > size.Height ? size.Width : size.Height);
		}

		private void updateImageViewImage (Uri uri, UIImageView imageView, bool scale, bool animated) {
			var duration = animated ? 0.3f : 0.0f;
			if (scale)
				imageView.Image = ImageManipulation.ScaleAndBlurImage (this.squareSize(imageView.Frame.Size), ImageLoader.DefaultRequestImage (uri, this));
			else
				imageView.Image = ImageLoader.DefaultRequestImage (uri, this);

			UIView.Animate (duration, () => imageView.Alpha = 1.0f);
   		}

		private void updateSaveButtonState () {
			if (Data.Current.IsInFavorites (this.MovieDetail)) 
				this.btnToggleSave.SetTitle ("Remove from Favorites", UIControlState.Normal);
			else
				this.btnToggleSave.SetTitle ("Save to Favorites", UIControlState.Normal);
		}
		#endregion

		#region Event handlers
		partial void btnToggleSave_Click (NSObject sender) {
			if (Data.Current.IsInFavorites (this.MovieDetail))
				Data.Current.RemoveFromFavorites (this.MovieDetail);
			else
				Data.Current.AddToFavorites (this.MovieDetail);
			
			this.updateSaveButtonState ();
		}

		partial void btnClose_Click (NSObject sender) {
			this.NavigationController.PopViewController (true);
		}
		#endregion
	}


}