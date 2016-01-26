using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

using UIKit;

using com.interactiverobert.prototypes.movieexplorer.apple.lib;
using com.interactiverobert.prototypes.movieexplorer.shared;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class SplashViewController : UIViewController
	{
		#region Constructor
		public SplashViewController (IntPtr handle) : base (handle) {

		}
		#endregion

		#region View lifecycle
		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			this.NavigationController.SetNavigationBarHidden (true, false);
		}

		public override void ViewDidAppear (bool animated) {
			base.ViewDidAppear (animated);

			this.startup ();
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);
		}
		#endregion
			
		#region Navigation
		public override void PerformSegue (string identifier, Foundation.NSObject sender) {
			base.PerformSegue (identifier, sender);

			var navigationList = new List<UIViewController> (this.NavigationController.ViewControllers);
			navigationList.RemoveAt (0);
			this.NavigationController.ViewControllers = navigationList.ToArray();
		}
		#endregion

		#region Private methods
		private void startup () {
			Api.Current.GetConfigurationAsync (e => {
				this.InvokeOnMainThread(() => this.PerformSegue ("ShowMovieList", this));
			});
		}
		#endregion
	}
}