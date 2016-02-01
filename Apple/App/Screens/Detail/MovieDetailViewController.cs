using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Drawing;

using Foundation;
using CoreImage;
using CoreGraphics;
using MonoTouch.Dialog.Utilities;
using UIKit;
using PatridgeDev;

using com.interactiverobert.prototypes.movieexplorer.apple.lib;
using com.interactiverobert.prototypes.movieexplorer.apple.lib.Imaging;
using com.interactiverobert.prototypes.movieexplorer.shared.Resources;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Video;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Movie;
using com.interactiverobert.prototypes.movieexplorer.shared.Services;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Configuration;
using com.interactiverobert.prototypes.movieexplorer.shared;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieDetailViewController : UIViewController, IImageUpdated
	{
		#region Shared members
		private static Dictionary<int, UIImage> backgroundImages = new Dictionary<int, UIImage>();
		private static UIColor solidBlackTwentyPercentScreen = UIColor.FromRGBA (0, 0, 0, 0.2f);
		#endregion

		#region Private fields
		private PDRatingView ratingView;
		private bool shouldStopPulseBackground;
		private List<Video> videos;
		private List<Movie> similarMovies;
		private MovieCollectionViewSource collectionViewSource;
		private UILongPressGestureRecognizer longPressRecognizer;
		#endregion

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
			this.longPressRecognizer = new UILongPressGestureRecognizer (() => {
				if (this.longPressRecognizer.NumberOfTouches > 0) {
					var point = this.longPressRecognizer.LocationOfTouch (0, this.cvSimilarMovies);
					var indexPath = this.cvSimilarMovies.IndexPathForItemAtPoint (point);
					if (indexPath != null) {
						var cell = this.cvSimilarMovies.CellForItem (indexPath) as MovieCollectionViewCell;
						if (this.longPressRecognizer.State == UIGestureRecognizerState.Began) {
							cell.SetHighlighted (true, true);
						} else if (this.longPressRecognizer.State == UIGestureRecognizerState.Ended) {
							cell.SetHighlighted (false, true, () => {
								var movie = this.similarMovies [indexPath.Row];
								Data.Current.ToggleFavorite (movie);
							});
						}
					} else {
						foreach (MovieCollectionViewCell cell in this.cvSimilarMovies.VisibleCells)
							cell.SetHighlighted (false, false);
					}
				} 
			});
			this.cvSimilarMovies.AddGestureRecognizer (this.longPressRecognizer);
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			if (this.collectionViewSource != null)
				this.collectionViewSource.MovieSelected += this.collectionViewSource_MovieSelected;

			Data.Current.FavoriteChanged += this.favoriteChanged;

			this.updateLayout ();
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);

			if (this.collectionViewSource != null)
				this.collectionViewSource.MovieSelected -= this.collectionViewSource_MovieSelected;

			Data.Current.FavoriteChanged -= this.favoriteChanged;
		}
		#endregion

		private NSIndexPath scrollIndex;
		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration) {
			base.WillRotate (toInterfaceOrientation, duration);

			if (this.collectionViewSource.GetItemsCount (this.cvSimilarMovies, 0) > 0) {
				this.scrollIndex = this.cvSimilarMovies.IndexPathsForVisibleItems.First ();
				UIView.Animate (duration, () => this.cvSimilarMovies.Alpha = 0.0f, null);
			}
		}

		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation) {
			base.DidRotate (fromInterfaceOrientation);

			this.updateSimilarMoviesLayout ();
			this.cvSimilarMovies.ScrollToItem (this.scrollIndex, this.TraitCollection.VerticalSizeClass == UIUserInterfaceSizeClass.Compact ? UICollectionViewScrollPosition.Top : UICollectionViewScrollPosition.Left, false);
			UIView.Animate (0.3f, () => this.cvSimilarMovies.Alpha = 1.0f, null);
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
		private void updateSimilarMoviesLayout () {
			var flowLayout = this.cvSimilarMovies.CollectionViewLayout as UICollectionViewFlowLayout;
			if (this.TraitCollection.VerticalSizeClass == UIUserInterfaceSizeClass.Compact) {
				if (this.InterfaceOrientation.IsLandscape ()) {
					this.vwSimilarMovies.BackgroundColor = solidBlackTwentyPercentScreen;
					flowLayout.ScrollDirection = UICollectionViewScrollDirection.Vertical;
				} else {
					this.vwSimilarMovies.BackgroundColor = UIColor.Clear;
					flowLayout.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
				}
			} else {
				this.vwSimilarMovies.BackgroundColor = UIColor.Clear;
				flowLayout.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
			}
		}

		private async void updateLayout () {
			this.btnPlay.Alpha = 0;
			this.vwSimilarMovies.Alpha = 0;
			if (this.MovieDetail != null) {
				var videoResponse = await Data.Current.GetVideosForMovieAsync (this.MovieDetail.Id);
				if (videoResponse != null && videoResponse.Results != null) {
					this.videos = videoResponse.Results;
					if (videoResponse.Results.Count > 0 && videoResponse.Results [0].Site.ToLower () == "youtube")
						UIView.Animate (0.3f, () => this.btnPlay.Alpha = 1.0f);
					else
						UIView.Animate (0.3f, () => this.btnPlay.Alpha = 0.0f);
				}
				var similarMoviesResponse = await Data.Current.GetSimilarForMovieAsync (this.MovieDetail.Id);
				if (similarMoviesResponse != null && similarMoviesResponse.Results != null) {
					this.similarMovies = similarMoviesResponse.Results;
					if (similarMoviesResponse.Results.Count == 0)
						UIView.Animate(0.3f, () => this.vwSimilarMovies.Alpha = 0.0f);
					else {
						if (this.collectionViewSource == null) {
							this.collectionViewSource = new MovieCollectionViewSource (this.similarMovies, this.Configuration);
							this.collectionViewSource.MovieSelected += this.collectionViewSource_MovieSelected;
							this.cvSimilarMovies.Source = this.collectionViewSource;
							this.cvSimilarMovies.ReloadData ();
						} else {
							this.collectionViewSource.Reload (this.similarMovies);
							this.cvSimilarMovies.ReloadData ();
						}
						this.updateSimilarMoviesLayout ();
						UIView.Animate(0.3f, () => this.vwSimilarMovies.Alpha = 1.0f);
					}
				}

				this.lblTitle.Text = this.MovieDetail.Title;
				this.lblReleaseDate.Text = String.Format ("Release Date: {0}", this.MovieDetail.ReleaseDate.ToShortDateString ());

				var ratingConfig = new RatingConfig(UIImage.FromBundle("star_empty"), UIImage.FromBundle("star_filled"), UIImage.FromBundle("star_filled"));
				var averageRating = (decimal)this.MovieDetail.VoteAverage / 2;
				this.ratingView = new PDRatingView (new CGRect(0f, 0f, this.vwVoteAverageContainer.Frame.Width, this.vwVoteAverageContainer.Frame.Height), ratingConfig, averageRating);
				this.vwVoteAverageContainer.Add(this.ratingView);

				this.lblVoteCount.Text = String.Format ("(from {0} votes)", this.MovieDetail.VoteCount.ToString ());
				this.tvOverview.Text = this.MovieDetail.Overview;

				this.tvOverviewHeightConstraint.Constant = this.tvOverview.SizeThatFits (new CGSize (this.tvOverview.Frame.Width, float.MaxValue)).Height;

				var posterUri = new Uri (String.Concat (this.Configuration.Images.BaseUrl, this.Configuration.Images.PosterSizes[1], this.MovieDetail.PosterPath));
				this.imgPoster.Image = ImageLoader.DefaultRequestImage (posterUri, this);

				this.btnBack.Hidden = this.NavigationController.ViewControllers.Length < 3;

				this.startPulseBackground ();
				this.updateSaveButtonState ();
			}
		}

		private void playFirstVideo () {
			if (this.videos != null && this.videos.Count > 0) {
				if (this.videos [0].Site.ToLower () == "youtube") {
					var uri = Data.Current.GetOpenInYoutubeUri (this.videos [0].Key);
					UIApplication.SharedApplication.OpenUrl (new NSUrl (uri));
				}
			}
		}

		private double nextDouble(Random rng, double min, double max) {
			return min + (rng.NextDouble() * (max - min));
		}
			
		private void startPulseBackground () {
			UIImage cachedImage;
			this.imgBackground.Alpha = 0;
			if (MovieDetailViewController.backgroundImages.TryGetValue (this.MovieDetail.Id, out cachedImage)) {
				this.imgBackground.Image = cachedImage;
				this.pulseBackground (0.3f, this.pulseBackground);
			}
			else {
				Task.Factory.StartNew (() =>  {
					var backgroundUri = new Uri (String.Concat (this.Configuration.Images.BaseUrl, this.Configuration.Images.BackdropSizes [0], this.MovieDetail.BackdropPath));
					Thread.Sleep (TimeSpan.FromMilliseconds (500));
					this.InvokeOnMainThread (() =>  {
						var image = ImageManipulation.ScaleAndBlurImage (this.squareSize (this.imgBackground.Frame.Size), ImageLoader.DefaultRequestImage (backgroundUri, this));
						if (MovieDetailViewController.backgroundImages.ContainsKey (this.MovieDetail.Id))
							MovieDetailViewController.backgroundImages [this.MovieDetail.Id] = image;
						else
							MovieDetailViewController.backgroundImages.Add (this.MovieDetail.Id, image);
						this.imgBackground.Image = image;
						this.pulseBackground (0.3f, this.pulseBackground);
					});
				});
			}
		}

		private void pulseBackground () {
			var randomAlpha = new Random ();
			var alpha = this.nextDouble (randomAlpha, 0.2, 0.4);
			this.pulseBackground ((float)alpha, this.pulseBackground);
		}

		private void pulseBackground (float alpha, Action completionAction) {
			UIView.Animate (3, 
				() => this.imgBackground.Alpha = alpha, 
				() => {
					if (completionAction != null && !this.shouldStopPulseBackground)
						completionAction.Invoke ();
				});
		}

		private void stopPulseBackground () {
			this.shouldStopPulseBackground = true;
		}

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
				this.btnToggleSave.SetTitle (SharedConstants.RemoveFromFavoritesText, UIControlState.Normal);
			else
				this.btnToggleSave.SetTitle (SharedConstants.SaveToFavoritesText, UIControlState.Normal);
		}
		#endregion

		#region Event handlers
		private void favoriteChanged (object sender, FavoriteChangedEventArgs e) {
			if (this.similarMovies.FirstOrDefault (x => x.Id == e.FavoriteMovie.Id) != null)
				this.cvSimilarMovies.ReloadData ();
		}
			
		private void collectionViewSource_MovieSelected (object sender, Movie e) {
			this.stopPulseBackground ();
			var newDetail = PresentationUtility.CreateFromStoryboard<MovieDetailViewController> ("MovieDetail");
			newDetail.MovieDetail = e;
			newDetail.Configuration = this.Configuration;
			this.NavigationController.PushViewController (newDetail, true);
		}

		partial void btnToggleSave_Click (NSObject sender) {
			Data.Current.ToggleFavorite (this.MovieDetail);
			this.updateSaveButtonState ();
		}

		partial void btnClose_Click (NSObject sender) {
			this.NavigationController.PopToRootViewController (true);
		}

		partial void btnBack_Click (NSObject sender) {
			this.NavigationController.PopViewController (true);
		}

		partial void btnPlay_Click (NSObject sender) {
			this.playFirstVideo ();
		}
		#endregion
	}
}