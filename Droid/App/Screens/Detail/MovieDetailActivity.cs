using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.Animations;
using Android.Support.V7.Widget;

using com.interactiverobert.prototypes.movieexplorer.shared;
using UniversalImageLoader.Core;
using com.interactiverobert.prototypes.movieexplorer.droid.lib;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	[Activity ()]			
	public class MovieDetailActivity : Activity, INotifyDataSetChangedReceiver
	{
		#region Private fields
		private List<Video> videos = new List<Video> ();
		private List<Movie> similarMovies = new List<Movie>();
		private ConfigurationResponse configuration;
		private Movie movieDetail;

		private Button btnPlay;
		private Button btnFavorite;
		private ImageButton btnClose;
		private TextView txtTitle;
		private TextView txtReleaseDate;
		private RatingBar ratingBar;
		private TextView txtVoteCount;
		private TextView txtOverview;
		private ImageView imgPoster;
		private RelativeLayout vwSimilarMovies;
		private RecyclerView movieList;
		private LinearLayoutManager movieLayoutManager;
		private MovieRecyclerViewAdapter movieAdapter;
		#endregion

		#region Activity overrides
		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			this.SetContentView (Resource.Layout.movie_detail);

			// Create your application here
			var strConfig = this.Intent.Extras.GetString ("Configuration");
			var strSelectedMovie = this.Intent.Extras.GetString ("SelectedMovie");
			this.configuration = JsonConvert.DeserializeObject<ConfigurationResponse> (strConfig);
			this.movieDetail = JsonConvert.DeserializeObject<Movie> (strSelectedMovie);

			this.btnPlay = this.FindViewById<Button> (Resource.Id.movie_detail_btnPlay);
			this.btnPlay.Click += this.btnPlay_Click;
			this.btnFavorite = this.FindViewById<Button> (Resource.Id.movie_detail_btnFavorite);
			this.btnFavorite.Click += this.btnFavorite_Click;
			this.btnClose = this.FindViewById<ImageButton> (Resource.Id.movie_detail_close);
			this.btnClose.Click += this.btnClose_Click;
			this.txtTitle = this.FindViewById<TextView> (Resource.Id.movie_detail_txtTitle);
			this.txtReleaseDate = this.FindViewById<TextView> (Resource.Id.movie_detail_txtReleaseDate);
			this.ratingBar = this.FindViewById<RatingBar> (Resource.Id.movie_detail_ratingBar);
			this.txtVoteCount = this.FindViewById<TextView> (Resource.Id.movie_detail_txtVoteCount);
			this.txtOverview = this.FindViewById<TextView> (Resource.Id.movie_detail_txtOverview);
			this.imgPoster = this.FindViewById<ImageView> (Resource.Id.movie_detail_imgPoster);
			this.vwSimilarMovies = this.FindViewById<RelativeLayout> (Resource.Id.movie_detail_vwSimilarMovies);
			this.movieList = this.FindViewById<RecyclerView>(Resource.Id.movie_detail_lstSimilarMovies);
			this.movieLayoutManager = new LinearLayoutManager (this, LinearLayoutManager.Horizontal, false);
			this.movieList.SetLayoutManager (this.movieLayoutManager);

			this.updateLayout ();
		}
		#endregion

		#region INotifyDataSetChangedReceiver implementation
		public void NotifyDataSetChanged () {
			this.movieList.GetAdapter ().NotifyDataSetChanged();
		}
		#endregion

		#region Private methods
		private void playFirstVideo () {
			if (this.videos != null && this.videos.Count > 0) {
				if (this.videos [0].Site.ToLower () == "youtube") {
					var uri = Data.Current.GetOpenInYoutubeUri (this.videos [0].Key);
					this.StartActivity (new Intent(Intent.ActionView, Android.Net.Uri.Parse (uri)));
				}
			}
   		}

		private async void updateLayout () {
			this.btnPlay.Alpha = 0;
			this.vwSimilarMovies.Alpha = 0;

			if (this.movieDetail != null) {
				var videoResponse = await Data.Current.GetVideosForMovieAsync (this.movieDetail.Id);
				if (videoResponse != null && videoResponse.Results != null) {
					this.videos = videoResponse.Results;
					if (videoResponse.Results.Count > 0 && videoResponse.Results [0].Site.ToLower () == "youtube")
						this.btnPlay.Alpha = 1;
//						this.btnPlay.StartAnimation (new AlphaAnimation (this.btnPlay.Alpha, 1.0f) { Duration = 300, FillAfter = true });
					else
						this.btnPlay.Alpha = 0;
//						this.btnPlay.StartAnimation (new AlphaAnimation (this.btnPlay.Alpha, 0.0f) { Duration = 300, FillAfter = true });
				}
				var similarMoviesResponse = await Data.Current.GetSimilarForMovieAsync (this.movieDetail.Id);
				if (similarMoviesResponse != null && similarMoviesResponse.Results != null) {
					this.similarMovies = similarMoviesResponse.Results;
					if (similarMoviesResponse.Results.Count == 0)
						this.vwSimilarMovies.Alpha = 0;
//						this.vwSimilarMovies.StartAnimation (new AlphaAnimation (vwSimilarMovies.Alpha, 0.0f) { Duration = 300, FillAfter = true });
					else {
						if (this.movieAdapter == null) {
							this.movieAdapter = new MovieRecyclerViewAdapter (this, this, this.similarMovies, this.configuration);
							this.movieList.SetAdapter (this.movieAdapter);
						} else {
							this.movieAdapter.Reload (this.similarMovies);
						}
						this.vwSimilarMovies.Alpha = 1;
//						this.vwSimilarMovies.StartAnimation (new AlphaAnimation (this.vwSimilarMovies.Alpha, 1.0f) { Duration = 300, FillAfter = true });
					}
				}

				this.txtTitle.Text = this.movieDetail.Title;
				this.txtReleaseDate.Text = String.Format ("Release Date: {0}", this.movieDetail.ReleaseDate.ToShortDateString ());
				this.ratingBar.NumStars = 5;
				this.ratingBar.Rating = (float) this.movieDetail.VoteAverage / 2;
				this.txtVoteCount.Text = String.Format ("(from {0} votes)", this.movieDetail.VoteCount.ToString ());
				this.txtOverview.Text = this.movieDetail.Overview;

				ImageLoader.Instance.DisplayImage (String.Concat (this.configuration.Images.BaseUrl, this.configuration.Images.PosterSizes [1], this.movieDetail.PosterPath), this.imgPoster);

				this.startPulseBackground ();
				this.updateSaveButtonState ();
			}
		}

		private void updateSaveButtonState () {
			if (Data.Current.IsInFavorites (this.movieDetail)) 
				this.btnFavorite.Text = "Remove from Favorites";
			else
				this.btnFavorite.Text = "Save to Favorites";
		}
			
		private void startPulseBackground () {
//			UIImage cachedImage;
//			this.imgBackground.Alpha = 0;
//			if (MovieDetailViewController.backgroundImages.TryGetValue (this.MovieDetail.Id, out cachedImage)) {
//				this.imgBackground.Image = cachedImage;
//				this.pulseBackground (0.3f, this.pulseBackground);
//			}
//			else {
//				Task.Factory.StartNew (() =>  {
//					var backgroundUri = new Uri (String.Concat (this.Configuration.Images.BaseUrl, this.Configuration.Images.BackdropSizes [0], this.MovieDetail.BackdropPath));
//					Thread.Sleep (TimeSpan.FromMilliseconds (500));
//					this.InvokeOnMainThread (() =>  {
//						var image = ImageManipulation.ScaleAndBlurImage (this.squareSize (this.imgBackground.Frame.Size), ImageLoader.DefaultRequestImage (backgroundUri, this));
//						if (MovieDetailViewController.backgroundImages.ContainsKey (this.MovieDetail.Id))
//							MovieDetailViewController.backgroundImages [this.MovieDetail.Id] = image;
//						else
//							MovieDetailViewController.backgroundImages.Add (this.MovieDetail.Id, image);
//						this.imgBackground.Image = image;
//						this.pulseBackground (0.3f, this.pulseBackground);
//					});
//				});
//			}
		}
		#endregion

		#region Event handlers
		private void btnPlay_Click (object sender, EventArgs e) {
			this.playFirstVideo ();
		}

		private void btnFavorite_Click (object sender, EventArgs e) {
			if (Data.Current.IsInFavorites (this.movieDetail))
				Data.Current.RemoveFromFavorites (this.movieDetail);
			else
				Data.Current.AddToFavorites (this.movieDetail);

			this.btnFavorite.Text = Data.Current.IsInFavorites (this.movieDetail) ? "Remove from Favorites" : "Add to Favorites";
		}

		private void btnClose_Click (object sender, EventArgs e) {
			Intent intent = new Intent(this, typeof (MovieBrowseActivity));
			intent.SetFlags(ActivityFlags.ClearTop);   
			this.StartActivity(intent);
		}
		#endregion
	}
}