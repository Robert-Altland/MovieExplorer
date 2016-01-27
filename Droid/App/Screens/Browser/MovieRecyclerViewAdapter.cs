using System;
using System.Collections.Generic;

using Android.Widget;
using Android.App;
using Android.Views;
using Android.Support.V7.Widget;

using com.interactiverobert.prototypes.movieexplorer.shared;
using UrlImageViewHelper;
using Android.Content;
using Newtonsoft.Json;
using Android.Views.Animations;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	public class MovieRecyclerViewAdapter : RecyclerView.Adapter
	{
		private ConfigurationResponse configuration;
		private List<Movie> movies;
		private Activity context;

		public MovieRecyclerViewAdapter(Activity context, List<Movie> items, ConfigurationResponse configuration) : base() {
			this.context = context;
			this.configuration = configuration;
			this.movies = items;
		}

		public override int ItemCount {	
			get { return this.movies.Count; }
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			//Inflate our CrewMemberItem Layout
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.movie_list_item, parent, false);
			var viewHolder = new MovieViewHolder(itemView);
			return viewHolder;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			var viewHolder = holder as MovieViewHolder;
			var thisMovie = this.movies [position];
			viewHolder.PosterImage.Tag = viewHolder;
			viewHolder.PosterImage.SetUrlDrawable (String.Concat (this.configuration.Images.BaseUrl, this.configuration.Images.PosterSizes [0], this.movies[position].PosterPath), Resource.Drawable.DefaultUrlImage);
			viewHolder.PosterImage.Click += ImgBtn_Click;
			viewHolder.PosterImage.LongClick += ImgBtn_LongClick;
			viewHolder.FavoriteIndicator.Visibility = Data.Current.IsInFavorites (thisMovie) ? ViewStates.Visible : ViewStates.Gone;
		}

		private void ImgBtn_LongClick (object sender, View.LongClickEventArgs e) {
			var typedSender = sender as ImageView;
			var viewHolder = typedSender.Tag as MovieViewHolder;
			viewHolder.HighlightIndicator.Alpha = 0.6f;

			var selectedMovie = this.movies [viewHolder.AdapterPosition];
			if (Data.Current.IsInFavorites (selectedMovie)) 
				Data.Current.RemoveFromFavorites (selectedMovie);
			else 
				Data.Current.AddToFavorites (selectedMovie);

			viewHolder.HighlightIndicator.StartAnimation (new AlphaAnimation (viewHolder.HighlightIndicator.Alpha, 0) { Duration = 300, StartOffset = 1000, FillAfter = true });
			viewHolder.FavoriteIndicator.Alpha = Data.Current.IsInFavorites (selectedMovie) ? 1.0f : 0.0f;
		}

		private void ImgBtn_Click (object sender, EventArgs e) {
			var typedSender = sender as ImageView;
			var viewHolder = typedSender.Tag as MovieViewHolder;
			viewHolder.SelectedIndicator.Alpha = 0.4f;

			var selectedMovie = this.movies [viewHolder.AdapterPosition];
			var detailActivity = new Intent (context, typeof(MovieDetailActivity));
			var serializedMovie = JsonConvert.SerializeObject (selectedMovie);
			var serializedConfig = JsonConvert.SerializeObject (this.configuration);
			var list = new List<string> ();
			list.Add (serializedMovie);
			list.Add (serializedConfig);
			var serializedList = JsonConvert.SerializeObject (list);
			detailActivity.PutExtra ("Data", serializedList);
			context.StartActivity (detailActivity);

			viewHolder.SelectedIndicator.StartAnimation (new AlphaAnimation (viewHolder.SelectedIndicator.Alpha, 0f) { Duration = 300, FillAfter = true });
		}

		public void Reload (List<Movie> movies) {
			this.movies = movies;
		}
	}
}