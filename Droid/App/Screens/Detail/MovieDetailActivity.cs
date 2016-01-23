
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using com.interactiverobert.prototypes.movieexplorer.shared;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	[Activity (Label = "Movie Details")]			
	public class MovieDetailActivity : Activity
	{
		internal Movie Data { get; private set; }

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			this.SetContentView (Resource.Layout.movie_detail);

			// Create your application here
			var item = this.Intent.Extras.GetString ("Data");
			this.Data = JsonConvert.DeserializeObject<Movie> (item);
			this.FindViewById<TextView> (Resource.Id.movie_detail_txtTitle).Text = this.Data.Title;
		}
	}
}