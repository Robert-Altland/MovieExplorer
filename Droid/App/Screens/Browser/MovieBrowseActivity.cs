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

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	[Activity ()]
	public class MovieBrowseActivity : Activity
	{
		#region Private fields
		private RecyclerView categoryList;
		private ConfigurationResponse configuration;
		private List<MovieCategory> categories;
		private RecyclerView.LayoutManager categoryLayoutManager;
		private MovieCategoryRecyclerViewAdapter categoryAdapter;
		#endregion

		#region Activity overrides
		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.create ();
		}
		#endregion

		#region Private methods
		private async void create () {
			// Set our view from the "main" layout resource
			this.SetContentView (Resource.Layout.movie_browse);

			//If the device is portrait, then show the RecyclerView in a vertical list,
			//else show it in horizontal list.
			this.categoryLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);

			//Create a reference to our RecyclerView and set the layout manager;
			this.categoryList = FindViewById<RecyclerView>(Resource.Id.movie_category_recyclerView);
			this.categoryList.SetLayoutManager(this.categoryLayoutManager);

			this.configuration = await Data.Current.GetConfigurationAsync ();
			this.categories = await Data.Current.GetMoviesByCategoryAsync ();
			if (this.categoryList.GetAdapter () == null) {
				this.categoryAdapter = new MovieCategoryRecyclerViewAdapter (this, this.categories, this.configuration);
				this.categoryList.SetAdapter (this.categoryAdapter);
			} else {
				this.categoryAdapter.Reload (this.categories);
			}
		}
		#endregion
	}
}