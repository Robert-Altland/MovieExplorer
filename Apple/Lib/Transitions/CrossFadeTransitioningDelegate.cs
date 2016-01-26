using System;

using UIKit;
using Foundation;

namespace com.interactiverobert.prototypes.movieexplorer.apple.lib.Transitions
{
	[Register ("CrossFadeTransitioningDelegate")]
	public class CrossFadeTransitioningDelegate : UINavigationControllerDelegate
	{
		#region Private fields
		private CrossFadeTransitionAnimator animator;
		#endregion

		#region Constructor
		public CrossFadeTransitioningDelegate () {
			this.animator = new CrossFadeTransitionAnimator ();
		}
		#endregion

		#region UINavigationControllerDelegate overrides
		/// <summary>
		/// Gets the animation controller for operation.
		/// </summary>
		/// <returns>The animation controller for operation.</returns>
		/// <param name="navigationController">Navigation controller.</param>
		/// <param name="operation">Operation.</param>
		/// <param name="fromViewController">From view controller.</param>
		/// <param name="toViewController">To view controller.</param>
		public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForOperation (UINavigationController navigationController, UINavigationControllerOperation operation, UIViewController fromViewController, UIViewController toViewController) {
			return this.animator;
//			if (fromViewController.Title == "Splash") {
//				return this.animator;
//			} else {
//				return null;
//			}
		}
		#endregion
	}
}