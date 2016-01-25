using System;
using System.Linq;

using Foundation;
using CoreImage;
using CoreGraphics;
using MonoTouch.Dialog.Utilities;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieDetailViewController : UIViewController, IImageUpdated
	{
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
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			if (this.MovieDetail != null) {
				this.lblTitle.Text = this.MovieDetail.Title;
				this.lblReleaseDate.Text = this.MovieDetail.ReleaseDate.ToShortDateString ();
				this.lblPopularity.Text = this.MovieDetail.Popularity.ToString ();
				this.lblVoteAverage.Text = this.MovieDetail.VoteAverage.ToString ();
				this.lblVoteCount.Text = this.MovieDetail.VoteCount.ToString ();
				this.lblOverview.Text = this.MovieDetail.Overview;

				var posterUri = new Uri (String.Concat (this.Configuration.Images.BaseUrl, this.Configuration.Images.PosterSizes[1], this.MovieDetail.PosterPath));
				this.imgPoster.Image = ImageLoader.DefaultRequestImage (posterUri, this);

				var backgroundUri = new Uri (String.Concat (this.Configuration.Images.BaseUrl, this.Configuration.Images.BackdropSizes[0], this.MovieDetail.BackdropPath));
				this.imgBackground.Image = ImageManipulation.ScaleAndBlurImage(this.imgBackground.Frame.Size, ImageLoader.DefaultRequestImage (backgroundUri, this));

				this.updateSaveButtonState ();
			}
		}
		#endregion

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
		private void updateImageViewImage (Uri uri, UIImageView imageView, bool scale, bool animated) {
			var duration = animated ? 0.3f : 0.0f;
			if (scale)
				imageView.Image = ImageManipulation.ScaleAndBlurImage (this.imgBackground.Frame.Size, ImageLoader.DefaultRequestImage (uri, this));
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


	public static class ImageManipulation
	{
		public static UIImage ScaleAndBlurImage (CGSize size, UIImage img) {
			return PerformContextOperations((float)size.Width, (float)size.Height, (context) => {
				var gaussianBlur = CIFilter.FromName ("CIGaussianBlur");
				gaussianBlur.SetDefaults ();
				var inputImage = CIImage.FromCGImage (img.CGImage);
				gaussianBlur.Image = inputImage;
				gaussianBlur.SetValueForKey (new NSNumber (10), CIFilterInputKey.Radius);
				var blurredImage = gaussianBlur.OutputImage;
				var result = UIImage.FromImage (blurredImage);
				context.DrawImage (new CGRect (-size.Width/2, -size.Height/2, size.Width*2, size.Height*2), result.CGImage);
			});
		}

		/// <summary>
		/// Gets the main screen scale factor (for Retina support).
		/// Value is cached for use on background threads, as UIScreen.MainScreen.Scale must be peformed on the main thread. If the property is accessed for the first
		/// time on the background thread, it will call on the main thread and block the background thread until the value is populated.
		/// </summary>
		public static float ScaleFactor
		{
			get
			{
				if (_scaleFactor > 0.0f)
					return _scaleFactor;

				if (NSRunLoop.Current == NSRunLoop.Main)
				{
					_scaleFactor = (float)UIScreen.MainScreen.Scale;
					return _scaleFactor;
				}
				else
				{
					NSRunLoop.Main.InvokeOnMainThread(() =>
						{
							if (_scaleFactor == 0.0f)
							{
								var x = ScaleFactor;
							}
						});

					while (_scaleFactor == 0.0f)
						System.Threading.Thread.Sleep(50);

					return _scaleFactor;
				}
			}
		}
		/// <summary>
		/// Backing field for ScaleFactor property
		/// </summary>
		private static float _scaleFactor = 0.0f;

		/// <summary>
		/// Performs a delegate-supplied set of operations on a CGBitmapContext
		/// </summary>
		/// <param name="canvasWidth">Width of context in display-scaled units (scaling will be applied to go from display units to pixels for Retina support)</param>
		/// <param name="canvasHeight">Height of context in display-scaled units (scaling will be applied to go from display units to pixels for Retina support)</param>
		/// <param name="operations">delegate that will perform context operations</param>
		/// <returns>UIImage containing result of context operations</returns>
		public static UIImage PerformContextOperations(float canvasWidth, float canvasHeight, Action<CGBitmapContext> operations) {
			var scaleFactor = ScaleFactor;
			var contextWidth = (int)(canvasWidth * scaleFactor);
			var contextHeight = (int)(canvasHeight * scaleFactor);

			var colorSpace = CoreGraphics.CGColorSpace.CreateDeviceRGB();
			var context = new CGBitmapContext(null, contextWidth, contextHeight, 8, 0, colorSpace, CGBitmapFlags.PremultipliedFirst);
			context.ScaleCTM(scaleFactor, scaleFactor);

			operations(context);

			var image = new UIImage(context.ToImage(), scaleFactor, UIImageOrientation.Up);

			context.Dispose();
			colorSpace.Dispose();

			return image;
		}

		/// <summary>
		/// Performs a delegate-supplied set of operations on a CGBitmapContext
		/// </summary>
		/// <param name="canvasSize">Size of context in display units (scaling will be applied to go from display units to pixels for Retina support)</param>
		/// <param name="operations">delegate that will perform context operations</param>
		/// <returns>UIImage containing result of context operations</returns>
		public static UIImage PerformContextOperations(CGSize canvasSize, Action<CGBitmapContext> operations) {
			return PerformContextOperations((float)canvasSize.Width, (float)canvasSize.Height, operations);
		}
	}
}