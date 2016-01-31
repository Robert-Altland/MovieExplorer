
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
using com.interactiverobert.prototypes.movieexplorer.shared;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using com.interactiverobert.prototypes.movieexplorer.shared.Services;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	[Activity (MainLauncher = true, NoHistory = true)]			
	public class SplashActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			// remove title
			this.RequestWindowFeature (WindowFeatures.NoTitle);

			// Set our view from the "main" layout resource
			this.SetContentView (Resource.Layout.splash);

			this.startup ();
		}

		private async void startup () {
			var configuration = await Data.Current.GetConfigurationAsync ();
			var categories = await Data.Current.GetMoviesByCategoryAsync ();
			var activity = new Intent (this, typeof(MovieBrowseActivity));
			var strConfig = JsonConvert.SerializeObject (configuration);
			var strCategories = JsonConvert.SerializeObject (categories);
			activity.PutExtra ("Configuration", strConfig);
			activity.PutExtra ("Categories", strCategories);
			this.StartActivity (activity);
		}
	}
}