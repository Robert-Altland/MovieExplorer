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
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Configuration;
using com.interactiverobert.prototypes.movieexplorer.shared.Entities.Movie;
using com.interactiverobert.prototypes.movieexplorer.shared.Services;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	public class MovieCategoryRecyclerViewAdapter : RecyclerView.Adapter
	{
		private ConfigurationResponse configuration;
		private readonly List<MovieCategory> categories;
		private Activity context;
		private readonly int favoritesCategoryIndex = 4; // Wrong scope, put this somewhere else

		public MovieCategoryRecyclerViewAdapter(Activity context, List<MovieCategory> items, ConfigurationResponse configuration) : base() {
			this.context = context;
			this.configuration = configuration;
			this.categories = items;
		}
			
		public override int ItemCount {	
			get { return this.categories == null ? 0 : this.categories.Count; }
		}
			
		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			//Inflate our CrewMemberItem Layout
			var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.movie_category_item, parent, false);
			return new MovieCategoryViewHolder(itemView, viewType);
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			var viewHolder = holder as MovieCategoryViewHolder;
			var thisCategory = this.categories [position];
			viewHolder.CategoryName.Text = thisCategory.CategoryName;
			// If the view holder is being reused, the layout manager will have already been set
			if (viewHolder.MovieList.GetLayoutManager () == null) 
				viewHolder.MovieList.SetLayoutManager (new LinearLayoutManager (context, LinearLayoutManager.Horizontal, false));
			// If the view holder is being reused, the adapter will have already been set
			// Is a reload necessary? Theory is the view holder being bound may not be 
			// at the same position and so the data might be out of symcmight be out of syn
			var adapter = viewHolder.MovieList.GetAdapter() as MovieRecyclerViewAdapter;
			if (adapter == null) {
				var movieAdapter = new MovieRecyclerViewAdapter (this.context, thisCategory.Movies, this.configuration, position == this.favoritesCategoryIndex);
				viewHolder.MovieList.SetAdapter (movieAdapter);
			} else {
				Console.WriteLine ("MovieCategoryRecyclerViewAdapter.OnBindViewHolder MovieList adapter != null");
//				adapter.Reload (thisCategory.Movies);
			}
		}

		public override int GetItemViewType (int position) {
			if (position == 0)
				return 0;

			return 1;
   		}
	}
}