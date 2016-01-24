﻿
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

		private void startup () {
			this.simulateStartupDelay ();
		}

		private void simulateStartupDelay () {
			Task.Factory.StartNew (() => {
				Thread.Sleep ((int)TimeSpan.FromSeconds (3).TotalMilliseconds);
				this.RunOnUiThread(() => {
					this.StartActivity (typeof (MovieListActivity));
				});
			});
		}
	}
}