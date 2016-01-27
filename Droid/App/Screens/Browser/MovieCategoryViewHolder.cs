using System;
using Android.Support.V7.Widget;
using Android.Widget;
using Android.Views;
using Android.Content.Res;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	public class MovieCategoryViewHolder : RecyclerView.ViewHolder
	{
		public TextView CategoryName { get; set; }
		public RecyclerView MovieList { get; set; }

		public MovieCategoryViewHolder (View itemView, int viewType) : base (itemView) {
			//Creates and caches our views defined in our layout
			if (viewType == 0) // TODO: Calculate this. I don't think this is a good way to ensure consistent presentation across different device resolutions
				itemView.SetPadding (itemView.PaddingLeft, itemView.PaddingTop + 110, itemView.PaddingRight, itemView.PaddingBottom);
			this.CategoryName = itemView.FindViewById<TextView>(Resource.Id.movie_category_item_txtCategoryName);
			this.MovieList = itemView.FindViewById<RecyclerView>(Resource.Id.movie_category_item_lstMovies);
		}
	}
}