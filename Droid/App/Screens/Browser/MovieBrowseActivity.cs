using System.Collections.Generic;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Support.V7.Widget;
using Java.IO;
using Newtonsoft.Json;

using com.interactiverobert.prototypes.movieexplorer.shared;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	[Activity ()]
	public class MovieBrowseActivity : Activity
	{
		private RecyclerView categoryList;
		private ConfigurationResponse configuration;
		private List<MovieCategory> categories;
		private RecyclerView.LayoutManager categoryLayoutManager;
		private MovieCategoryRecyclerViewAdapter categoryAdapter;

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			// remove title
			this.RequestWindowFeature (WindowFeatures.NoTitle);

			// Set our view from the "main" layout resource
			this.SetContentView (Resource.Layout.movie_browse);

			//If the device is portrait, then show the RecyclerView in a vertical list,
			//else show it in horizontal list.
			this.categoryLayoutManager = Resources.Configuration.Orientation == Android.Content.Res.Orientation.Portrait 
				? new LinearLayoutManager(this, LinearLayoutManager.Vertical, false) 
				: new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);

			//Create a reference to our RecyclerView and set the layout manager;
			this.categoryList = FindViewById<RecyclerView>(Resource.Id.movie_category_recyclerView);
			this.categoryList.SetLayoutManager(this.categoryLayoutManager);

			Api.Current.GetConfigurationAsync (configurationResponse => {
				this.configuration = configurationResponse;
				Api.Current.GetMoviesByCategoryAsync (categories => {
					this.categories = categories;
					if (this.categoryList.GetAdapter () == null) {
						this.categoryAdapter = new MovieCategoryRecyclerViewAdapter (this, this.categories, this.configuration);
						this.categoryList.SetAdapter (this.categoryAdapter);
					} else {
						this.categoryAdapter.Reload (this.categories);
					}
				});
			});
		}
	}
}