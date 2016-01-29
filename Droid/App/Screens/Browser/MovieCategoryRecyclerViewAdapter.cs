using System;
using System.Collections.Generic;

using Android.Widget;
using Android.App;
using Android.Views;
using Android.Content;
using Android.Support.V7.Widget;
using Newtonsoft.Json;

using com.interactiverobert.prototypes.movieexplorer.shared;
using Android.Content.Res;
using com.interactiverobert.prototypes.movieexplorer.droid.lib;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	public class MovieCategoryRecyclerViewAdapter : RecyclerView.Adapter, INotifyDataSetChangedReceiver
	{
		private ConfigurationResponse configuration;
		private readonly List<MovieCategory> categories;
		private Activity context;

		public MovieCategoryRecyclerViewAdapter(Activity context, List<MovieCategory> items, ConfigurationResponse configuration) : base() {
			this.context = context;
			this.configuration = configuration;
			this.categories = items;
		}
			
		public override int ItemCount {	
			get { return this.categories.Count; }
		}
			
		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			//Inflate our CrewMemberItem Layout
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.movie_category_item, parent, false);
			var viewHolder = new MovieCategoryViewHolder(itemView, viewType);
			return viewHolder;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			var viewHolder = holder as MovieCategoryViewHolder;
			var thisCategory = this.categories [position];
			viewHolder.CategoryName.Text = thisCategory.CategoryName;
			if (viewHolder.MovieList.GetLayoutManager () == null) {
				var movieLayoutManager = new LinearLayoutManager (context, LinearLayoutManager.Horizontal, false);
				viewHolder.MovieList.SetLayoutManager (movieLayoutManager);
			}
			var adapter = viewHolder.MovieList.GetAdapter() as MovieRecyclerViewAdapter;
			if (adapter == null) {
				var movieAdapter = new MovieRecyclerViewAdapter (this, this.context, thisCategory.Movies, this.configuration);
				viewHolder.MovieList.SetAdapter (movieAdapter);
			} else {
				adapter.Reload (thisCategory.Movies);
			}
		}

		public override int GetItemViewType (int position) {
			if (position == 0)
				return 0;

			return 1;
		}

		public void Reload (List<MovieCategory> newCategories) {
//			this.categories.Clear ();
//			this.categories.AddRange (newCategories.ToArray ());
			this.NotifyDataSetChanged();
		}
	}
}