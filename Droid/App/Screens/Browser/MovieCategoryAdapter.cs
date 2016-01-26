using System;
using System.Collections.Generic;

using Android.Widget;
using Android.App;
using Android.Views;

using com.interactiverobert.prototypes.movieexplorer.shared;
using Newtonsoft.Json;
using Android.Content;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	public class MovieCategoryAdapter : BaseAdapter<MovieCategory>
	{
		private List<MovieCategory> categories;
		private Activity context;

		public MovieCategoryAdapter(Activity context, List<MovieCategory> items) : base() {
			this.context = context;
			this.categories = items;
		}

		public override long GetItemId(int position) {
			return position;
		}

		public override MovieCategory this[int position] {
			get { return this.categories[position]; }
		}

		public override int Count {
			get { return this.categories.Count; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent) {
			var view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Resource.Layout.movie_category_item, null);

			var thisCategory = this.categories [position];
			var movies = thisCategory.Movies;
			var title = view.FindViewById<TextView> (Resource.Id.movie_category_item_txtCategoryName);
			var gallery = view.FindViewById<Gallery> (Resource.Id.movie_category_item_gallery);

			title.Text = thisCategory.CategoryName;
			var adapter = new MovieListAdapter (context, movies);	
			gallery.Adapter = adapter;
			gallery.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				var selectedMovie = adapter [e.Position];
				var detailActivity = new Intent (context, typeof(MovieDetailActivity));
				var serialized = JsonConvert.SerializeObject (selectedMovie);
				detailActivity.PutExtra ("Data", serialized);
				context.StartActivity (detailActivity);
			};

			return view;
		}

		public void Reload (List<MovieCategory> movies) {
			this.categories = movies;
		}
	}
}