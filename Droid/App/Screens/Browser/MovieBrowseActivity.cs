using Android.App;
using Android.Widget;
using Android.OS;
using com.interactiverobert.prototypes.movieexplorer.shared;
using Android.Views;
using Android.Content;
using Java.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	[Activity ()]
	public class MovieBrowseActivity : Activity
	{
		private ListView categoryList;
		private List<MovieCategory> categories;
		private MovieCategoryAdapter adapter;

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			// remove title
			this.RequestWindowFeature (WindowFeatures.NoTitle);

			// Set our view from the "main" layout resource
			this.SetContentView (Resource.Layout.movie_browse);

			Api.Current.GetMoviesByCategoryAsync (categories => {
				this.categories = categories;
				if (this.categoryList.Adapter == null) {
					this.adapter = new MovieCategoryAdapter (this, this.categories);	
					this.categoryList.Adapter = this.adapter;
//					this.categoryList.ItemClick += this.movieCategory_ItemClick;
				} else {
					this.adapter.Reload (this.categories);
					this.categoryList.InvalidateViews ();
				}
			});

			// Get our button from the layout resource,
			// and attach an event to it
			this.categoryList = FindViewById<ListView> (Resource.Id.movie_list_listView);
		}

		private void movieCategory_ItemClick (object sender, AdapterView.ItemClickEventArgs e) {
			var selectedMovie = this.adapter [e.Position];
			var detailActivity = new Intent (this, typeof(MovieDetailActivity));
			var serialized = JsonConvert.SerializeObject (selectedMovie);
			detailActivity.PutExtra ("Data", serialized);
			StartActivity (detailActivity);
		}
	}
}