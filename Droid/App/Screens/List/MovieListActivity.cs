using Android.App;
using Android.Widget;
using Android.OS;
using com.interactiverobert.prototypes.movieexplorer.shared;
using Android.Views;
using Android.Content;
using Java.IO;
using Newtonsoft.Json;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	[Activity (Label = "Movie Explorer", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MovieListActivity : Activity
	{
		private ListView movieList;
		private MovieListAdapter movieListAdapter;

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			// remove title
			this.RequestWindowFeature (WindowFeatures.NoTitle);

			// Set our view from the "main" layout resource
			this.SetContentView (Resource.Layout.movie_list);

			Api.GetMoviesCompleted += this.getMoviesCompleted;
			Api.GetMoviesAsync ();

			// Get our button from the layout resource,
			// and attach an event to it
			this.movieList = FindViewById<ListView> (Resource.Id.movie_list_listView);
		}

		protected override void OnDestroy () {
			base.OnDestroy ();

			Api.GetMoviesCompleted -= this.getMoviesCompleted;
		}
			
		private void getMoviesCompleted (object sender, MovieDiscoverResponse e) {
			if (this.movieList.Adapter == null) {
				this.movieListAdapter = new MovieListAdapter (this, e.Results);	
				this.movieList.Adapter = this.movieListAdapter;
				this.movieList.ItemClick += this.movieList_ItemClick;
			} else {
				this.movieListAdapter.Reload (e.Results);
				this.movieList.InvalidateViews ();
			}
		}

		private void movieList_ItemClick (object sender, AdapterView.ItemClickEventArgs e) {
			var selectedMovie = this.movieListAdapter [e.Position];
			var detailActivity = new Intent (this, typeof(MovieDetailActivity));
			var serialized = JsonConvert.SerializeObject (selectedMovie);
			detailActivity.PutExtra ("Data", serialized);
			StartActivity (detailActivity);
		}
	}
}