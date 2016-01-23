using System;
using Android.Widget;
using com.interactiverobert.prototypes.movieexplorer.shared;
using System.Collections.Generic;
using Android.App;
using Android.Views;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	public class MovieListAdapter : BaseAdapter<Movie>
	{
		private List<Movie> movies;
		private Activity context;

		public MovieListAdapter(Activity context, List<Movie> items) : base() {
			this.context = context;
			this.movies = items;
		}

		public override long GetItemId(int position) {
			return position;
		}

		public override Movie this[int position] {
			get { return this.movies[position]; }
		}

		public override int Count {
			get { return this.movies.Count; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent) {
			var view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Resource.Layout.movie_list_item, null);
			view.FindViewById<TextView>(Resource.Id.movie_list_item_txtTitle).Text = this.movies[position].Title;
			return view;
		}

		public void Reload (List<Movie> movies) {
			this.movies = movies;
		}
	}
}