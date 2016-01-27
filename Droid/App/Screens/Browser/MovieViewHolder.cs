using System;

using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	public class MovieViewHolder : RecyclerView.ViewHolder
	{
		public ImageView PosterImage { get; set; }

		public View FavoriteIndicator { get; set; }

		public View HighlightIndicator { get; set; }

		public View SelectedIndicator { get; set; }


		public MovieViewHolder (View itemView) : base (itemView) {		
			this.PosterImage = itemView.FindViewById<ImageView>(Resource.Id.movie_list_item_imgPoster);
			this.FavoriteIndicator = itemView.FindViewById<View>(Resource.Id.movie_list_item_vwFavoriteIndicator);
			this.HighlightIndicator = itemView.FindViewById<View>(Resource.Id.movie_list_item_vwHighlight);
			this.SelectedIndicator = itemView.FindViewById<View>(Resource.Id.movie_list_item_vwSelected);
		}
	}
}