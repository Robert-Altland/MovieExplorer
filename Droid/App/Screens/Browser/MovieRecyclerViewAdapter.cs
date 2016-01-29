using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using Android.Widget;
using Android.App;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Content;
using Android.Views.Animations;

using com.interactiverobert.prototypes.movieexplorer.shared;
using UniversalImageLoader.Core;
using com.interactiverobert.prototypes.movieexplorer.droid.lib;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	public class MovieRecyclerViewAdapter : RecyclerView.Adapter
	{
		#region Private fields
		private INotifyDataSetChangedReceiver receiver;
		private ConfigurationResponse configuration;
		private readonly List<Movie> movies;
		private Activity context;
		#endregion

		#region Constructor
		public MovieRecyclerViewAdapter(INotifyDataSetChangedReceiver receiver, Activity context, List<Movie> items, ConfigurationResponse configuration) : base() {
			this.receiver = receiver;
			this.context = context;
			this.configuration = configuration;
			this.movies = items;
		}
		#endregion

		#region RecyclerView.Adapter overrides
		public override int ItemCount {	
			get { return this.movies.Count; }
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			//Inflate our CrewMemberItem Layout
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.movie_list_item, parent, false);
			var viewHolder = new MovieViewHolder(itemView);
			viewHolder.PosterImage.Click += this.imgPoster_Click;
			viewHolder.PosterImage.LongClick += this.imgPoster_LongClick;
			return viewHolder;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			var viewHolder = holder as MovieViewHolder;
			var thisMovie = this.movies [position];
			viewHolder.PosterImage.Tag = viewHolder;

			ImageLoader.Instance.DisplayImage (String.Concat (this.configuration.Images.BaseUrl, this.configuration.Images.PosterSizes [0], this.movies[position].PosterPath), viewHolder.PosterImage);

			viewHolder.FavoriteIndicator.Alpha = Data.Current.IsInFavorites (thisMovie) ? 1.0f : 0.0f;
			viewHolder.SelectedIndicator.Alpha = 0.0f;
			viewHolder.HighlightIndicator.Alpha = 0.0f;
		}


		#endregion

		#region Public methods
		public void Reload (List<Movie> newMovies) {
//			this.movies.Clear ();
//			this.movies.AddRange (newMovies.ToArray ());
			this.NotifyDataSetChanged ();
		}
		#endregion

		#region Event handlers
		private void imgPoster_LongClick (object sender, View.LongClickEventArgs e) {
			var typedSender = sender as ImageView;
			var viewHolder = typedSender.Tag as MovieViewHolder;
			if (viewHolder.AdapterPosition >= 0) {
				viewHolder.HighlightIndicator.Alpha = 0.6f;

				var selectedMovie = this.movies [viewHolder.AdapterPosition];
				if (Data.Current.IsInFavorites (selectedMovie))
					Data.Current.RemoveFromFavorites (selectedMovie);
				else
					Data.Current.AddToFavorites (selectedMovie);

				viewHolder.HighlightIndicator.StartAnimation (new AlphaAnimation (viewHolder.HighlightIndicator.Alpha, 0) {
					Duration = 300,
					StartOffset = 1000,
					FillAfter = true
				});
				viewHolder.FavoriteIndicator.Alpha = Data.Current.IsInFavorites (selectedMovie) ? 1.0f : 0.0f;

				if (this.receiver != null)
					this.receiver.NotifyDataSetChanged ();
			}
		}

		private void imgPoster_Click (object sender, EventArgs e) {
			var typedSender = sender as ImageView;
			var viewHolder = typedSender.Tag as MovieViewHolder;
			viewHolder.SelectedIndicator.Alpha = 0.4f;

			var selectedMovie = this.movies [viewHolder.AdapterPosition];
			var detailActivity = new Intent (context, typeof(MovieDetailActivity));
			var strConfig = JsonConvert.SerializeObject (this.configuration);
			var strMovie = JsonConvert.SerializeObject (selectedMovie);
			detailActivity.PutExtra ("Configuration", strConfig);
			detailActivity.PutExtra ("SelectedMovie", strMovie);
			context.StartActivity (detailActivity);

			viewHolder.SelectedIndicator.StartAnimation (new AlphaAnimation (viewHolder.SelectedIndicator.Alpha, 0f) { Duration = 300, FillAfter = true });
		}
		#endregion
	}
}