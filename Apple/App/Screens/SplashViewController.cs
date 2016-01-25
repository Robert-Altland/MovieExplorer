using System;

using UIKit;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using com.interactiverobert.prototypes.movieexplorer.apple.lib;
using com.interactiverobert.prototypes.movieexplorer.shared;

namespace com.interactiverobert.prototypes.movieexplorer.apple
{
	public partial class SplashViewController : UIViewController
	{

		private CrossFadeTransitionAnimator animator;

		public IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController (UIViewController presented, UIViewController presenting, UIViewController source) {
			this.animator = new CrossFadeTransitionAnimator ();
			return this.animator;
		}



		public SplashViewController (IntPtr handle) : base (handle) {

		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			this.NavigationController.SetNavigationBarHidden (true, false);
		}

		public override void ViewDidAppear (bool animated) {
			base.ViewDidAppear (animated);

			Api.Current.GetConfigurationCompleted += this.getConfigurationCompleted;
			this.startup ();
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);

			Api.Current.GetConfigurationCompleted -= this.getConfigurationCompleted;
		}

		public override void DidReceiveMemoryWarning () {
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
			
		public override void PerformSegue (string identifier, Foundation.NSObject sender) {
			base.PerformSegue (identifier, sender);

			var navigationList = new List<UIViewController> (this.NavigationController.ViewControllers);
			navigationList.RemoveAt (0);
			this.NavigationController.ViewControllers = navigationList.ToArray();
		}

		private void startup () {
			Api.Current.GetConfigurationAsync ();
		}
			
		private void getConfigurationCompleted (object sender, ConfigurationResponse e) {
			this.PerformSegue ("ShowMovieList", this);
		}
	}
}