using System.Collections.Generic;
using Newtonsoft.Json;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Support.V7.Widget;
using Java.IO;

using com.interactiverobert.prototypes.movieexplorer.shared;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Configuration;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Movie;
using com.interactiverobert.prototypes.movieexplorer.shared.Services;
using System.Threading.Tasks;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	[Activity ()]
	public class MovieBrowseActivity : Activity
	{
		#region Private fields
		private RecyclerView categoryList;
		private ConfigurationResponse configuration;
		private List<MovieCategory> categories;
//		private List<Movie> spotlightMovies;
		#endregion

		#region Activity overrides
		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			this.SetContentView (Resource.Layout.movie_browse);
			this.categoryList = FindViewById<RecyclerView>(Resource.Id.movie_category_recyclerView);

			this.create ();
		}
		#endregion

		#region Private methods
		private async void create () {
			this.configuration = await Data.Current.GetConfigurationAsync ();
			this.categories = await Data.Current.GetMoviesByCategoryAsync ();
//			this.spotlightMovies = await Data.Current.GetSpotlightMoviesAsync ();

			if (this.categoryList.GetLayoutManager () == null)
				this.categoryList.SetLayoutManager (new LinearLayoutManager (this, LinearLayoutManager.Vertical, false));
		
			if (this.categoryList.GetAdapter () == null) 
				this.categoryList.SetAdapter (new MovieCategoryRecyclerViewAdapter (this, this.categories, this.configuration));
		}
		#endregion
	}
}