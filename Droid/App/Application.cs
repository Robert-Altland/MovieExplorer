
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
using com.interactiverobert.prototypes.movieexplorer.shared.services;
using UniversalImageLoader.Core;

namespace com.interactiverobert.prototypes.movieexplorer.droid.app
{
	/// <summary>
	/// This is not recommended best practice.
	/// </summary>
	#if DEBUG
	[Application(Debuggable=true)]
	#else
	[Application(Debuggable=false)]
	#endif
	public class Application : global::Android.App.Application
	{
		protected Application (IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) {
			
		}

		public override void OnCreate () {
			base.OnCreate ();
			// Create your application here

			// Use default options
			var config = ImageLoaderConfiguration.CreateDefault(ApplicationContext);
			// Initialize ImageLoader with configuration.
			ImageLoader.Instance.Init(config);

			DependencyManager.RegisterHttpClientFactory (new HttpClientFactory ());
			DependencyManager.RegisterCache (new Cache ());
		}
	}
}