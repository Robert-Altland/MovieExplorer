using System;

using Foundation;
using UIKit;

using com.interactiverobert.prototypes.movieexplorer.shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class MovieBrowserViewController : UIViewController
	{
		private MovieCategoryTableViewSource tableViewSource;
		private GetMoviesResponse topRatedMovies;
		private GetMoviesResponse popularMovies;
		private GetMoviesResponse nowPlayingMovies;
		private GetMoviesResponse upcomingMovies;
		private List<Movie> favorites;
		private ConfigurationResponse configuration;
		private Movie selectedMovie;

		public MovieBrowserViewController (IntPtr handle) : base (handle) {
			
		}
			
		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.tblMovieCategories.RowHeight = 200;
			this.tblMovieCategories.EstimatedRowHeight = 200;
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			if (this.tableViewSource != null)
				this.tableViewSource.MovieSelected += this.tableViewSource_MovieSelected;

			Api.Current.GetConfigurationAsync ((config) => {
				this.configuration = config;
				Api.Current.GetTopRatedMoviesAsync ((topRated) => {
					this.topRatedMovies = topRated;
					Api.Current.GetPopularMoviesAsync(popular => {
						this.popularMovies = popular;
						Api.Current.GetNowPlayingMoviesAsync(nowPlaying => {
							this.nowPlayingMovies = nowPlaying;
							Api.Current.GetUpcomingMoviesAsync(upcoming => {
								this.upcomingMovies = upcoming;
								Data.Current.GetFavoritesAsync (faves => {
									this.favorites = faves;
									var data = this.aggregateResults ();
									if (this.tableViewSource == null) {
										this.tableViewSource = new MovieCategoryTableViewSource (this, this.configuration, data);
										this.tableViewSource.MovieSelected += this.tableViewSource_MovieSelected;
										this.tblMovieCategories.Source = this.tableViewSource;
										this.tblMovieCategories.ReloadData ();
									} else {
										this.tableViewSource.Reload (data);
										this.tblMovieCategories.ReloadData ();
									}
								});
							});
						});
					});
				});
			});
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);

			if (this.tableViewSource != null)
				this.tableViewSource.MovieSelected -= this.tableViewSource_MovieSelected;
		}


		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender) {
			var destination = segue.DestinationViewController as MovieDetailViewController;
			destination.MovieDetail = this.selectedMovie;
			destination.Configuration = this.configuration;
			base.PrepareForSegue (segue, sender);
		}


		private List<MovieCategory> aggregateResults ()  {
			var result = new List<MovieCategory> ();
			result.Add (new MovieCategory ("Your Favorites", this.favorites));
			result.Add (new MovieCategory ("Top Rated", this.topRatedMovies.Results)); 
			result.Add (new MovieCategory ("Popular", this.popularMovies.Results));
			result.Add (new MovieCategory ("Now Playing", this.nowPlayingMovies.Results));
			result.Add (new MovieCategory ("Upcoming", this.upcomingMovies.Results));
			return result;
		}
			

		private void tableViewSource_MovieSelected (object sender, Movie e) {
			this.selectedMovie = e;
			this.PerformSegue ("ShowMovieDetail", this);
		}

		partial void btnSearch_Click (NSObject sender) {
			// TODO: Show search 
		}
	}
}
