using System;

using UIKit;
using Foundation;

namespace com.interactiverobert.prototypes.movieexplorer.apple.lib
{
	[Register ("CrossFadeTransitioningDelegate")]
	public class CrossFadeTransitioningDelegate : UINavigationControllerDelegate
	{
		private CrossFadeTransitionAnimator animator;

		public CrossFadeTransitioningDelegate () {
			this.animator = new CrossFadeTransitionAnimator ();
		}

		public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForOperation (UINavigationController navigationController, UINavigationControllerOperation operation, UIViewController fromViewController, UIViewController toViewController) {
			return this.animator;
//			if (fromViewController.Title == "Splash") {
//				return this.animator;
//			} else {
//				return null;
//			}
		}
	}
}